using DarkSky.Core.ViewModels.Temporary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Services.Interfaces
{
	public interface IAccountService
	{
		Task<ProfileViewModel> GetCurrentProfileAsync();
	}
}
