using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Services;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
		private FeedViewPost[] currentProfilePosts;

		public ProfileViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			setup();
		}

		private async void setup()
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
			var t = await atProtoService.ATProtocolClient.Feed.GetAuthorFeedAsync(atProtoService.Session.Handle, 50, null, default);
			var f = t.AsT0;
			CurrentProfilePosts = f.Feed;
		}
	}
}
