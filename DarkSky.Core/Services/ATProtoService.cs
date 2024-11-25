using DarkSky.Core.Classes;
using FishyFlip;
using DarkSky.Core.ViewModels;
using FishyFlip.Models;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FishyFlip.Lexicon.Com.Atproto.Server;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DarkSky.Core.Services
{
	public partial class ATProtoService : ObservableObject
	{
		//  .WithInstanceUrl(new Uri(Host)) for PDS https://bsky.social";
		public ATProtocol ATProtocolClient;
		public Session Session { get; set; }

		private INavigationService navigationService;
		private ICredentialService credentialService;
		public ATProtoService(INavigationService navigationService, ICredentialService credentialService)
		{
			this.navigationService = navigationService;
			this.credentialService = credentialService;

			if (credentialService.Count() == 0)
			{
				// DO NOT CHANGE, THIS FIXES A VERY WEIRD BUG WHERE
				// IF I WENT DIRECTLY TO LOGINVIEWMODEL THEN TO MAINVIEWMODEL, THEN SESSION WOULD ALWAYS BE NULL
				navigationService.NavigateTo<MainViewModel>(); // dont login, this fixes bug
				navigationService.NavigateTo<LoginViewModel>(); // show login
			}
			else
				setup(); // login if credential found
		}

		private async void setup()
		{
			Credential credentials = credentialService.GetCredential();
			await LoginAsync(credentials.username, credentials.password);
			navigationService.NavigateTo<MainViewModel>();
		}

		public async Task LoginAsync(string username, string password)
		{
			ATProtocolClient = new ATProtocolBuilder().EnableAutoRenewSession(true).Build();
			Session = await ATProtocolClient.AuthenticateWithPasswordAsync(username, password);
			if (Session is not null)
			{
				// code for refresh token
				/*Session session2 = new Session(session1.Did, session1.DidDoc, session1.Handle, session1.Email, session1.AccessJwt, session1.RefreshJwt);
				var authSession = new AuthSession(session2);
				Session session3 = await ATProtocolClient.AuthenticateWithPasswordSessionAsync(authSession);*/

				// Send authentication message
				WeakReferenceMessenger.Default.Send(new AuthenticationSessionMessage(Session));
			}
		}
	}
}
