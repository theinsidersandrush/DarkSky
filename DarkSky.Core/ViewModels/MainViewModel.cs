using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Services;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.ViewModels
{
	public partial class MainViewModel : ObservableObject
	{
		private ATProtoService atProtoService;
		private INavigationService navigationService;

		[ObservableProperty]
		private FeedProfile currentProfile;

		public MainViewModel(ATProtoService atProtoService, INavigationService navigationService)
		{
			this.atProtoService = atProtoService;
			this.navigationService = navigationService;
			if (atProtoService.Session is null)
				navigationService.NavigateTo<LoginViewModel>();
			else
				setup();
		}

		private async void setup()
		{
			var profiles = await atProtoService.ATProtocolClient.Actor.GetProfileAsync(atProtoService.Session.Did);
			CurrentProfile = profiles.AsT0;
		}
	}
}
