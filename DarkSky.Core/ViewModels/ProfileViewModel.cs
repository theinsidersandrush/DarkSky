using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Classes;
using DarkSky.Core.Cursors;
using DarkSky.Core.Messages;
using DarkSky.Core.Services;
using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DarkSky.Core.ViewModels
{
	public partial class ProfileViewModel : ObservableObject
	{
		public ObservableCollection<FeedNavigationItem> ProfileNavigationItems = new();

		[ObservableProperty]
		private ProfileViewDetailed currentProfile;

		[ObservableProperty]
		private FeedNavigationItem selectedProfileNavigationItem;

		private ATProtoService atProtoService;
		public ProfileViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			WeakReferenceMessenger.Default.Register<AuthenticationSessionMessage>(this, async (r, m) =>
			{
				Setup(m.Value);
			});
			if(atProtoService.Session is not null)
				Setup(atProtoService.Session);
		}

		// todo fix
		private async void Setup(Session session)
		{
			try
			{
				ProfileNavigationItems.Clear();
				SelectedProfileNavigationItem = null;
		
				var profiles = await atProtoService.ATProtocolClient.Actor.GetProfileAsync(session.Did);
				CurrentProfile = profiles.AsT0;
				ProfileNavigationItems.Add(new FeedNavigationItem("Posts", new ProfileFeedCursorSource(atProtoService, "posts_no_replies")));
				ProfileNavigationItems.Add(new FeedNavigationItem("Replies", new ProfileFeedCursorSource(atProtoService, "posts_with_replies")));
				ProfileNavigationItems.Add(new FeedNavigationItem("Media", new ProfileFeedCursorSource(atProtoService, "posts_with_media")));
				SelectedProfileNavigationItem = ProfileNavigationItems[0];
			}
			catch (Exception ex)
			{
				WeakReferenceMessenger.Default.Send(new ErrorMessage(ex));
			}
		}
	}
}
