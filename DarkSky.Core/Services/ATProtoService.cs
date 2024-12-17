﻿using DarkSky.Core.Classes;
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
using IdentityModel.OidcClient;
using System.Diagnostics;

namespace DarkSky.Core.Services
{
	public partial class ATProtoService : ObservableObject
	{
		//  .WithInstanceUrl(new Uri(Host)) for PDS https://bsky.social";
		public ATProtocol ATProtocolClient = new ATProtocolBuilder().EnableAutoRenewSession(true).Build();

		public async Task LoginAsync(string username, string password)
		{
			try
			{
				ATProtocolClient = new ATProtocolBuilder().EnableAutoRenewSession(true).Build();
				// We use CreateSessionAsync to get error message if login fails to display to the user
				var result = await ATProtocolClient.AuthenticateWithPasswordResultAsync(username, password);
				if (result.IsT0 && result.AsT0 is not null)
				{
					// code for refresh token
					/*Session session2 = new Session(session1.Did, session1.DidDoc, session1.Handle, session1.Email, session1.AccessJwt, session1.RefreshJwt);
					var authSession = new AuthSession(session2);
					Session session3 = await ATProtocolClient.AuthenticateWithPasswordSessionAsync(authSession);*/
					WeakReferenceMessenger.Default.Send(new AuthenticationSessionMessage(ATProtocolClient.Session));
				}
				else
				{
					Debug.WriteLine(result.AsT1.StatusCode);
					Debug.WriteLine(result.AsT1.Detail?.Message);
					Debug.WriteLine(result.AsT1.Detail?.Error);
					WeakReferenceMessenger.Default.Send(new ErrorMessage(new Exception($"{result.AsT1.Detail?.Message} - {result.AsT1.StatusCode} {result.AsT1.Detail?.Error}")));
				}
			}
			catch (Exception ex) {
				WeakReferenceMessenger.Default.Send(new ErrorMessage(ex));
			}
		}
	}
}
