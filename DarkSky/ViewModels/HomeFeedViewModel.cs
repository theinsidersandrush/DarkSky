using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Services;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace DarkSky.ViewModels
{
	public partial class HomeFeedViewModel : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<FeedViewPost> timelineFeed = new();

		private ATProtoService atProtoService;
		public HomeFeedViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			Setup();
		}

		private async void Setup()
		{
			var feed = (await atProtoService.ATProtocolClient.Feed.GetTimelineAsync()).AsT0.Feed;
			foreach (var item in feed){
				TimelineFeed.Add(item);
			}
		}
	}
}
