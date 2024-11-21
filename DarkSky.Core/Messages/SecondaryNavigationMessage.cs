using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.Messages
{
	public class SecondaryNavigationMessage : ValueChangedMessage<int>
	{
		// TEMPORARY
		// 0 = CLOSE, 1 = OPEN
		public SecondaryNavigationMessage(int msg) : base(msg)
		{
		}
	}
}
