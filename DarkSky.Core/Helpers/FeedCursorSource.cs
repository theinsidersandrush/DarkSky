using DarkSky.Core.Services;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Helpers
{
	public class FeedCursorSource : AbstractFeedCursorSource
	{
		private string FeedUri;
		public FeedCursorSource(ATProtoService atProtoService, string feed) : base(atProtoService) 
		{ 
			this.FeedUri = feed;
		}

		public override async Task GetMoreItemsAsync(int limit = 50)
		{
			if (IsLoading) return; // Don't load if items are currently loading
			IsLoading = true;
			GetFeedOutput timeLine = (await atProtoService.ATProtocolClient.Feed.GetFeedAsync(new ATUri(FeedUri), limit, Cursor)).AsT0;
			Cursor = timeLine.Cursor;
			foreach (var item in timeLine.Feed)
			{
				Feed.Add(item);
			}
			IsLoading = false;
		}
	}
}
