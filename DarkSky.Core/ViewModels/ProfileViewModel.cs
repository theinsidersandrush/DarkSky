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
		private FeedViewPost[] timelineFeed;

		public ProfileViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			setup();
		}

		private async void setup()
		{
			var profiles = await atProtoService.ATProtocolClient.Actor.GetProfileAsync(atProtoService.Session.Did);
			CurrentProfile = profiles.AsT0;
			List<ATUri> x = new();
			x.Add(currentProfile.PinnedPost.Uri);
			var p = await atProtoService.ATProtocolClient.Feed.GetPostsAsync(x);
			var c = p.AsT0;
			PinnedProfilePost = c.Posts[0];

			var g = await atProtoService.ATProtocolClient.Feed.GetTimelineAsync();

			var y = g.AsT0;
			TimelineFeed = y.Feed;
		}
	}
}
