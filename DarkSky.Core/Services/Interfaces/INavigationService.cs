using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.Services.Interfaces
{
	public interface INavigationService
	{
		void NavigateTo<T>();
	}
}
