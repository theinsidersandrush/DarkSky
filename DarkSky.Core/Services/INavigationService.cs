using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.Services
{
	public interface INavigationService
	{
		void NavigateTo<T>();
	}
}
