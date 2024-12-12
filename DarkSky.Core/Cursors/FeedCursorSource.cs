using DarkSky.Core.Factories;
using DarkSky.Core.Cursors;
using DarkSky.Core.Services;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors
{
	public class FeedCursorSource : AbstractCursorSource<PostViewModel>, IFeedCursorSource
	{
		private string FeedUri;
		public FeedCursorSource(string feed) : base()
		{
			FeedUri = feed;
		}

		protected override async Task OnGetMoreItemsAsync(int limit = 20)
		{
			GetFeedOutput timeLine = (await atProtoService.ATProtocolClient.Feed.GetFeedAsync(new ATUri(FeedUri), limit, Cursor)).AsT0;
			Cursor = timeLine.Cursor;
			foreach (var item in timeLine.Feed)
				Feed.Add(PostFactory.Create(item));
		}
	}
}
