using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Classes;
using DarkSky.Core.Cursors;
using DarkSky.Core.Messages;
using DarkSky.Core.Services;
using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace DarkSky.Core.ViewModels
{
	public partial class NotificationsViewModel : ObservableObject
	{
		[ObservableProperty]
		private NotificationsCursorSource notificationsSource = new NotificationsCursorSource();

		private ATProtoService atProtoService;
		public NotificationsViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			WeakReferenceMessenger.Default.Register<AuthenticationSessionMessage>(this, (r, m) =>
			{
				Setup(m.Value);
			});

			if (atProtoService.ATProtocolClient.Session is not null)
				Setup(atProtoService.ATProtocolClient.Session);
		}

		private async void Setup(Session session)
		{
			try
			{
			}
			catch (Exception e)
			{
				WeakReferenceMessenger.Default.Send(new ErrorMessage(e));
			}
		}
	}
}
