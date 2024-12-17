﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DarkSky.Core.Classes;
using DarkSky.Core.Cursors.Feeds;
using DarkSky.Core.Cursors.Lists;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.App.Bsky.Graph;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.ViewModels.Temporary
{
	public partial class ListViewModel : ObservableObject
	{

		public ObservableCollection<CursorNavigationItem> Cursors = new();

		[ObservableProperty]
		private CursorNavigationItem selectedCursor;

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
		private ListUsersCursorSource listUsersCursorSource;

		[ObservableProperty]
		private ListView listView;

		public ListViewModel(ListView listView)
		{
			this.ListView = listView;
			this.Name = listView.Name ?? "";
			this.Description = listView.Description ?? "";
			this.Avatar = listView.Avatar ?? "https://raw.githubusercontent.com/FireCubeStudios/DarkSky/refs/heads/master/DarkSky/Assets/BlueSky/list.webp";
			this.CreatedAt = listView.IndexedAt ?? DateTime.Now;
			if (listView.Uri is not null)
			{
				Cursors.Add(new CursorNavigationItem("Posts", new ListFeedCursorSource(listView.Uri.ToString())));
				Cursors.Add(new CursorNavigationItem("Users", new ListUsersCursorSource(this)));
				SelectedCursor = Cursors[0];
			}	
		}
	}
}
