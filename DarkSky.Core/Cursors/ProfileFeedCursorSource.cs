using DarkSky.Core.Factories;
using DarkSky.Core.Cursors;
using DarkSky.Core.Services;
using DarkSky.Core.ViewModels;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors
{
	/*
	 * Load posts from a Profile
	 */
	public class ProfileFeedCursorSource : AbstractCursorSource<PostViewModel>, IFeedCursorSource
	{
		private string Filter;
		private string PinnedPostCID;
		public ProfileFeedCursorSource(ATProtoService atProtoService, string filter) : base(atProtoService)
		{
			Filter = filter;
		}

		protected override async Task OnGetMoreItemsAsync(int limit = 50)
		{
			GetAuthorFeedOutput timeLine = (await atProtoService.ATProtocolClient.Feed.GetAuthorFeedAsync(atProtoService.Session.Handle, limit, Cursor, Filter, false)).AsT0;
			Cursor = timeLine.Cursor;
			if(Feed.Count == 0) // Add pinned post if it exists
			{

			}
			foreach (var item in timeLine.Feed)
			{
				if(item.Post.Cid != PinnedPostCID) // Ignore post if it was a pinned post
					Feed.Add(PostFactory.Create(item));
			}
		}
	}
}
