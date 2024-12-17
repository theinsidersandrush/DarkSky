using DarkSky.Core.Factories;
using DarkSky.Core.Cursors;
using DarkSky.Core.Services;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.App.Bsky.Notification;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DarkSky.Core.Cursors
{
	public class NotificationsCursorSource : AbstractCursorSource<Notification>
	{
		public NotificationsCursorSource() : base() { }

		protected override async Task OnGetMoreItemsAsync(int limit = 50)
		{
			ListNotificationsOutput notifications = (await atProtoService.ATProtocolClient.Notification.ListNotificationsAsync(limit, false, Cursor)).AsT0;
			Cursor = notifications.Cursor;
			foreach (var item in notifications.Notifications)
			{
				Add(item);
			}
		}
	}
}
