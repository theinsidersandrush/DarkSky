using DarkSky.Core.Factories;
using DarkSky.Core.Services;
using DarkSky.Core.ViewModels.Temporary;
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

			/*
			 * The timeline may contain a post then a reply chain to that post which basically duplicates it
			 * This logic aims to remove duplicates to show reply chains to the original post instead
			 */
			foreach (var item in timeLine.Feed)
			{
				try
				{
					if (item.Reply is null)
					{ // add regular posts
					  // only add if it did not appear before, maybe as part of a reply chain
						if (!postID.Contains(item.Post.Cid))
							Feed.Add(PostFactory.Create(item));
					}
					else // the post is a reply, use logic to filter
					{
						FishyFlip.Lexicon.App.Bsky.Feed.ReplyRef reply = item.Reply;
						PostView root = (PostView)reply.Root;
						PostView parent = (PostView)reply.Parent;
						// only allow replies if it replies to same author
						if (root.Author.Did.Handler == item.Post.Author.Did.Handler)
						{
							// only add if it did not appear before, as part of a reply chain
							if (!postID.Contains(root.Cid))
							{
								// if a reply was retweeted then do not show parent or root posts
								if (item.Reason is null)
								{  
									if (parent.Cid != root.Cid)  // only show root reply if parent is not root
										Feed.Add(new PostViewModel((PostView)reply.Root) { HasReply = true });
									Feed.Add(new PostViewModel((PostView)reply.Parent) { HasReply = true, IsReply = true });
								}
								Feed.Add(PostFactory.Create(item)); // Show the regular post

								postID.Add(root.Cid); // add root to hashset so we can filter if it appears later
							}
						}
					}
				}
				catch { }
			}
			IsLoading = false;
		}
	}
}
