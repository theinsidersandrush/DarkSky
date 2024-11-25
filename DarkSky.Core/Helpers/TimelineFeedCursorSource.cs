using DarkSky.Core.Services;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Helpers
{
	public class TimelineFeedCursorSource : AbstractFeedCursorSource
	{
		public TimelineFeedCursorSource(ATProtoService atProtoService) : base(atProtoService) { }

		// Prevent duplicates loading with replies and posts
		private HashSet<string> postID = new HashSet<string>();

		public override async Task GetMoreItemsAsync(int limit = 5)
		{
			if (IsLoading) return; // Don't load if items are currently loading
			IsLoading = true;
			GetTimelineOutput timeLine = (await atProtoService.ATProtocolClient.Feed.GetTimelineAsync(limit:limit, cursor:Cursor)).AsT0;
			Cursor = timeLine.Cursor;
			foreach (var item in timeLine.Feed)
			{
				if (item.Reply is null)
				{ // add regular posts
				  // only add if it did not appear before, maybe as part of a reply chain
					if (!postID.Contains(item.Post.Cid))
						Feed.Add(item);
				}
				else // the post is a reply, use logic to filter
				{
					FishyFlip.Lexicon.App.Bsky.Feed.ReplyRef reply = item.Reply;
					PostView root = (PostView)reply.Root;
					PostView parent = (PostView)reply.Parent;
					// only allow replies if it replies to same author
					if (root.Author.Did.Handler == item.Post.Author.Did.Handler)
					{
						// only add if it did not appear before, maybe as part of a reply chain
						if (!postID.Contains(root.Cid))
						{
							Feed.Add(item); // add the reply

							postID.Add(root.Cid); // add parent to hashset so we can filter if it appears later
						}
					}
				}
			}
			IsLoading = false;
		}
	}
}
