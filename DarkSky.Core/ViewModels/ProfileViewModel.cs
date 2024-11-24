using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Helpers;
using DarkSky.Core.Services;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DarkSky.Core.ViewModels
{
	public partial class ProfileViewModel : ObservableObject
	{
		private ATProtoService atProtoService;

		[ObservableProperty]
		private FeedProfile currentProfile;

		[ObservableProperty]
		private PostView pinnedProfilePost;

		[ObservableProperty]
		private IFeedCursorSource currentProfilePosts;

		public ProfileViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			CurrentProfilePosts = new ProfileFeedCursorSource(atProtoService, AuthorFeedFilterType.PostsNoReplies);
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

			var preferencesx = await atProtoService.ATProtocolClient.Actor.GetPreferencesAsync();
			var preferences = preferencesx.AsT0;
			foreach (var p in preferences.Preferences) {
				Debug.WriteLine(p.Type);
				if(p.Type == "app.bsky.actor.defs#savedFeedsPrefV2")
				{
					UnknownRecord pp = p as UnknownRecord;
				}
			}
		}
	}
}
