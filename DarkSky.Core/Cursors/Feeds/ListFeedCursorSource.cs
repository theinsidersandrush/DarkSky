using DarkSky.Core.Factories;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Feeds
{
	/*
	 * Get posts from a List
	 * https://docs.bsky.app/docs/api/app-bsky-feed-get-list-feed
	 */
	public class ListFeedCursorSource : AbstractCursorSource<PostViewModel>, IFeedCursorSource
	{
		private string ListUri;
		public ListFeedCursorSource(string list) : base()
		{
			ListUri = list;
		}

		protected override async Task OnGetMoreItemsAsync(int limit = 20)
		{
			GetListFeedOutput list = (await atProtoService.ATProtocolClient.Feed.GetListFeedAsync(new ATUri(ListUri), limit, Cursor)).AsT0;
			Cursor = list.Cursor;
			foreach (var item in list.Feed)
				Items.Add(PostFactory.Create(item));
		}
	}
}
