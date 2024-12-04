using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using DarkSky.Core.Services;
using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Lexicon.App.Bsky.Graph;
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
		private ProfileViewDetailed currentProfile;

		public MainViewModel(ATProtoService atProtoService, INavigationService navigationService)
		{
			this.atProtoService = atProtoService;
			this.navigationService = navigationService;
			WeakReferenceMessenger.Default.Register<AuthenticationSessionMessage>(this, (r, m) =>
			{
				setup(m.Value);
			});

			if (atProtoService.Session is not null)
				setup(atProtoService.Session);
		}

		private async void setup(Session session)
		{
			try
			{
				var profiles = await atProtoService.ATProtocolClient.Actor.GetProfileAsync(session.Did);
				CurrentProfile = profiles.AsT0;

		
				// follow firecube.bsky.social so users can get app updates TEMPORARY
				// move to OOBE and notify user about follow with a one-timeprompt instead
				var cube = (await atProtoService.ATProtocolClient.Actor.GetProfileAsync(ATIdentifier.Create("did:plc:y4pmm7ixx6u5gd7rtxe4rnpn"))).AsT0;
				if (cube.Viewer.Following is null)
				{
					var x = await atProtoService.ATProtocolClient.CreateFollowAsync(cube.Did);
				}
			}
			catch (Exception e)
			{
				WeakReferenceMessenger.Default.Send(new ErrorMessage(e));
			}
		}
	}
}
