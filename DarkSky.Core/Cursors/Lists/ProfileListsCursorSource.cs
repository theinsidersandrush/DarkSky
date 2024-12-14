using DarkSky.Core.Cursors.Feeds;
using DarkSky.Core.Factories;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.App.Bsky.Graph;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Lists
{
	public class ProfileListsCursorSource : AbstractCursorSource<ListViewModel>
	{
		private ProfileViewModel Profile;
		public ProfileListsCursorSource(ProfileViewModel profile) : base()
		{
			Profile = profile;
		}

		protected override async Task OnGetMoreItemsAsync(int limit = 50)
		{
			GetListsOutput lists = (await atProtoService.ATProtocolClient.Graph.GetListsAsync(Profile.Handle, limit, Cursor)).AsT0;
			Cursor = lists.Cursor;

			foreach (var item in lists.Lists)
				Items.Add(new ListViewModel(item));
		}
	}
}
