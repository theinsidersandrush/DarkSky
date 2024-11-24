using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Helpers;
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
		private IFeedCursorSource timelinePosts;

		private ATProtoService atProtoService;
		public HomeFeedViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			TimelinePosts = new TimelineFeedCursorSource(atProtoService);
		}
	}
}
