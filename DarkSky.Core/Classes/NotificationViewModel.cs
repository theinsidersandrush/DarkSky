using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.Classes
{
	public record NotificationViewModel(string Text, DateTime? indexedAt);
}
