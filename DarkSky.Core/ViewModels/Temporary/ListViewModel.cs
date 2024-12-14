using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Cursors.Feeds;
using FishyFlip.Lexicon.App.Bsky.Graph;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.ViewModels.Temporary
{
	public partial class ListViewModel : ObservableObject
	{
		[ObservableProperty]
		private string name;

		[ObservableProperty]
		private string description;

		[ObservableProperty]
		private string avatar;

		[ObservableProperty]
		private DateTime createdAt;

		[ObservableProperty]
		private ListFeedCursorSource listFeedCursorSource;

		[ObservableProperty]
		private ListView listView;
		public ListViewModel(ListView listView)
		{
			this.ListView = listView;
			this.Name = listView.Name ?? "";
			this.Description = listView.Description ?? "";
			this.Avatar = listView.Avatar ?? "";
			this.CreatedAt = listView.IndexedAt ?? DateTime.Now;
			if(listView.Uri is not null)
				this.listFeedCursorSource = new ListFeedCursorSource(listView.Uri.ToString());
		}
	}
}
