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

		public ProfileViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			setup();
		}

		private async void setup()
		{
			var profiles = await atProtoService.ATProtocolClient.Actor.GetProfileAsync(atProtoService.Session.Did);
			CurrentProfile = profiles.AsT0;
		}
	}
}
