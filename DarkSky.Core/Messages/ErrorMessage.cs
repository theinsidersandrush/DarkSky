using CommunityToolkit.Mvvm.Messaging.Messages;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.Messages
{
	/*
	 * Notifies the user of some sort of error
	 * Temporary
	 */
	public class ErrorMessage : ValueChangedMessage<Exception>
	{
		public ErrorMessage(Exception exception) : base(exception)
		{
		}
	}
}
