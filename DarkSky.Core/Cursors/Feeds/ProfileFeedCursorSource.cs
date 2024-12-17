﻿using DarkSky.Core.Factories;
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

namespace DarkSky.Core.Cursors.Feeds
{
	/*
	 * Load posts from a Profile
	 */
	public class ProfileFeedCursorSource : AbstractCursorSource<PostViewModel>, ICursorSource
	{
		private string Filter;
		private ProfileViewModel Profile;
		public ProfileFeedCursorSource(ProfileViewModel profile, string filter) : base()
		{
			Filter = filter;
			Profile = profile;
		}

		protected override async Task OnGetMoreItemsAsync(int limit = 50)
		{
			GetAuthorFeedOutput timeLine = (await atProtoService.ATProtocolClient.Feed.GetAuthorFeedAsync(Profile.Handle, limit, Cursor, Filter, false)).AsT0;
			Cursor = timeLine.Cursor;

			if (((ObservableCollection<PostViewModel>)Items).Count == 0 && Profile.PinnedPost is not null) // Add pinned post first if it exists
				Add(Profile.PinnedPost);

			foreach (var item in timeLine.Feed)
			{
				// Ignore pinned posts if there are any
				if (Profile.PinnedPost is null || item.Post.Cid != Profile.PinnedPost.Cid)
					Add(PostFactory.Create(item));
			}
		}
	}
}
