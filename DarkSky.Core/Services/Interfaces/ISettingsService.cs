using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.Services.Interfaces
{
	public interface ISettingsService
	{
		public bool IsLoggedIn { get; set; }
	}
}
