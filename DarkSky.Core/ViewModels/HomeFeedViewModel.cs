using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Classes;
using DarkSky.Core.Helpers;
using DarkSky.Core.Services;
using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace DarkSky.Core.ViewModels
{
	public partial class HomeFeedViewModel : ObservableObject
	{
		public ObservableCollection<FeedNavigationItem> Feeds = new();

		[ObservableProperty]
		private FeedNavigationItem selectedFeed;

		private ATProtoService atProtoService;
		public HomeFeedViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			Setup();
		}

		// todo fix
		private async void Setup()
		{
			var x = await atProtoService.ATProtocolClient.Actor.GetPreferencesAsync();
			var preferences = x.AsT0;
			foreach (var p in preferences.Preferences)
			{
				Debug.WriteLine(p.Type);
				if (p.Type == "app.bsky.actor.defs#savedFeedsPrefV2")
				{
					SavedFeedsPrefV2 feeds = p as SavedFeedsPrefV2;
					foreach (var item in feeds.Items)
					{
						if ((bool)item.Pinned && item.TypeValue == "feed")
						{
							var f = (await atProtoService.ATProtocolClient.Feed.GetFeedGeneratorAsync(new ATUri(item.Value))).AsT0;
							Feeds.Add(new FeedNavigationItem(f.View.DisplayName, new FeedCursorSource(atProtoService, item.Value)));
						}

						if(item.TypeValue == "timeline")
							Feeds.Add(new FeedNavigationItem("Following", new TimelineFeedCursorSource(atProtoService)));
					}
				}
			}
			SelectedFeed = Feeds[0];
		}
	}
}
