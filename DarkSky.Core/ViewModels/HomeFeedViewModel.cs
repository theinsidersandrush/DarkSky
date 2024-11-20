using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Services;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.ViewModels
{
	public partial class HomeFeedViewModel : ObservableObject
	{
		[ObservableProperty]
		private FeedViewPost[] timelineFeed;

		private ATProtoService atProtoService;
		public HomeFeedViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			setup();
		}

		private async void setup()
		{
			TimelineFeed = (await atProtoService.ATProtocolClient.Feed.GetTimelineAsync()).AsT0.Feed;
		}
	}
}
