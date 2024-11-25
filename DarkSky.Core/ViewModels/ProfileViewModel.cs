using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Classes;
using DarkSky.Core.Helpers;
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
		private PostView pinnedProfilePost;

		[ObservableProperty]
		private FeedNavigationItem selectedProfileNavigationItem;

		private ATProtoService atProtoService;
		public ProfileViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			ProfileNavigationItems.Add(new FeedNavigationItem("Posts", new ProfileFeedCursorSource(atProtoService, "posts_no_replies")));
			ProfileNavigationItems.Add(new FeedNavigationItem("Replies", new ProfileFeedCursorSource(atProtoService, "posts_with_replies")));
			ProfileNavigationItems.Add(new FeedNavigationItem("Media", new ProfileFeedCursorSource(atProtoService, "posts_with_media")));
			SelectedProfileNavigationItem = ProfileNavigationItems[0];
			Setup();
		}

		// todo fix
		private async void Setup()
		{
			var profiles = await atProtoService.ATProtocolClient.Actor.GetProfileAsync(atProtoService.Session.Did);
			CurrentProfile = profiles.AsT0;
			
			try
			{
				List<ATUri> x = new();
				x.Add(currentProfile.PinnedPost.Uri);
				var p = await atProtoService.ATProtocolClient.Feed.GetPostsAsync(x);
				var c = p.AsT0;
				PinnedProfilePost = c.Posts[0];
			}
			catch (Exception ex) { }
		}
	}
}
