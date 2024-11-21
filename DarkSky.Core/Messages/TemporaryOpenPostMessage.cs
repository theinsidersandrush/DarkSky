using CommunityToolkit.Mvvm.Messaging.Messages;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.Messages
{
	public class TemporaryOpenPostMessage : ValueChangedMessage<PostView>
	{
		// TEMPORARY
		public TemporaryOpenPostMessage(PostView msg) : base(msg)
		{
		}
	}
}
