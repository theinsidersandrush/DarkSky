using DarkSky.Core.Factories;
using DarkSky.Core.Services;
using DarkSky.Core.ViewModels;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Helpers
{
	/*
	 * Load posts from a Profile
	 */
	public class ProfileFeedCursorSource : AbstractFeedCursorSource
	{
		private string Filter;
		public ProfileFeedCursorSource(ATProtoService atProtoService, string filter) : base(atProtoService)
		{
			this.Filter = filter;
		}

		public override async Task GetMoreItemsAsync(int limit = 50)
		{
			if(IsLoading) return; // Don't load if items are currently loading
			IsLoading = true;
			GetAuthorFeedOutput timeLine = (await atProtoService.ATProtocolClient.Feed.GetAuthorFeedAsync(atProtoService.Session.Handle, limit, Cursor, Filter)).AsT0;
			Cursor = timeLine.Cursor;
			foreach (var item in timeLine.Feed)
			{
				Feed.Add(PostFactory.Create(item));
			}
			IsLoading = false;
		}
	}
}
