using FishyFlip.Lexicon.App.Bsky.Richtext;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.Helpers
{
	/*
	 * This helper class contains code for Facets and RichText
	 * Facets work with UTF-8 encoding and use byte indices.
	 * Inclusive start index and an exclusive end index.
	 * https://docs.bsky.app/docs/advanced-guides/post-richtext
	 */
	public class RichTextHelper
	{
		// Convert byte index to a char index
		public static int ByteIndexToCharIndex(string Text, long Index)
		{
			byte[] TextUTF8 = Encoding.UTF8.GetBytes(Text); // Convert text into a UTF-8 byte array

			if (Index < 0 || Index > TextUTF8.Length)   // Check if the Index is out of bounds
				throw new ArgumentOutOfRangeException(nameof(Index), "Index is out of bounds.");

			int charIndex = 0;
			long byteIndex = 0;			
			while (byteIndex < Index)   // Loop through each character in the string
			{
				// Get the byte size of the current character
				int charByteSize = GetUtf8CharByteSize(TextUTF8, byteIndex);

				// Move byteIndex by the number of bytes of the current character
				byteIndex += charByteSize;
				charIndex++;
			}
			return charIndex;
		}

		// Helper method to determine the byte size of a UTF-8 character
		private static int GetUtf8CharByteSize(byte[] text, long byteIndex)
		{
			// Determine the number of bytes based on the leading byte
			byte firstByte = text[byteIndex];

			if ((firstByte & 0x80) == 0)
				return 1;	// 1 byte for ASCII characters (0x00 - 0x7F)
			else if ((firstByte & 0xE0) == 0xC0)
				return 2;	// 2 bytes for characters in the range 0x80 - 0x7FF
			else if ((firstByte & 0xF0) == 0xE0)
				return 3;   // 3 bytes for characters in the range 0x800 - 0xFFFF
			else if ((firstByte & 0xF8) == 0xF0)
				return 4;   // 4 bytes for characters in the range 0x10000 - 0x10FFFF

			return 1; // Default case, should not happen in well-formed UTF-8 text.
		}
	}
}
