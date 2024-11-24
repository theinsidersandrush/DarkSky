using DarkSky.Core.Services;
using DarkSky.Core.ViewModels;
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
		private AuthorFeedFilterType filter;
		public ProfileFeedCursorSource(ATProtoService atProtoService, AuthorFeedFilterType filter) : base(atProtoService)
		{
			this.filter = filter;
		}

		public override async Task GetMoreItemsAsync(int limit = 50)
		{
			if(IsLoading) return; // Don't load if items are currently loading
			IsLoading = true;
			var timeLine = (await atProtoService.ATProtocolClient.Feed.GetAuthorFeedAsync(atProtoService.Session.Handle, filter, limit:limit, cursor:Cursor)).AsT0;
			Cursor = timeLine.Cursor;
			foreach (var item in timeLine.Feed)
			{
				Feed.Add(item);
			}
			IsLoading = false;
		}
	}
}
