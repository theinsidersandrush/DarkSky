using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Services;
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
		[ObservableProperty]
		private ObservableCollection<FeedViewPost> timelineFeed = new();

		private ATProtoService atProtoService;
		public HomeFeedViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			setup();
		}

		private async void setup()
		{
			var feed = (await atProtoService.ATProtocolClient.Feed.GetTimelineAsync()).AsT0.Feed;
			foreach (var item in feed){
				TimelineFeed.Add(item);
			}
		}
	}
}
