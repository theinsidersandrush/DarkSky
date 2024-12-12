using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Classes;
using DarkSky.Core.Cursors;
using DarkSky.Core.Messages;
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
			WeakReferenceMessenger.Default.Register<AuthenticationSessionMessage>(this, (r, m) =>
			{
				Setup(m.Value);
			});

			if (atProtoService.ATProtocolClient.Session is not null)
				Setup(atProtoService.ATProtocolClient.Session);
		}

		// todo fix
		private async void Setup(Session session)
		{
			try
			{
				SelectedFeed = null;
				Feeds.Clear();
				var x = await atProtoService.ATProtocolClient.Actor.GetPreferencesAsync();
				var preferences = x.AsT0;
				foreach (var p in preferences.Preferences)
				{
					if (p.Type == "app.bsky.actor.defs#savedFeedsPrefV2")
					{
						SavedFeedsPrefV2 feeds = p as SavedFeedsPrefV2;
						foreach (var item in feeds.Items)
						{
							if ((bool)item.Pinned && item.TypeValue == "feed")
							{
								var f = (await atProtoService.ATProtocolClient.Feed.GetFeedGeneratorAsync(new ATUri(item.Value))).AsT0;
								Feeds.Add(new FeedNavigationItem(f.View.DisplayName, new FeedCursorSource(item.Value)));
							}

							if (item.TypeValue == "timeline")
								Feeds.Add(new FeedNavigationItem("Following", new TimelineFeedCursorSource()));

							if (SelectedFeed is null) // percieved faster performance
								SelectedFeed = Feeds[0];
						}
					}
				}
			}
			catch (Exception e)
			{
				WeakReferenceMessenger.Default.Send(new ErrorMessage(e));
			}
		}
	}
}
