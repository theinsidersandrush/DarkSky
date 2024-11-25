using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DarkSky.Core.Classes;
using DarkSky.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.ViewModels
{
	public partial class SettingsViewModel : ObservableObject
	{
		private ATProtoService atProtoService;
		private INavigationService navigationService;
		private ICredentialService credentialService;
		public SettingsViewModel(ATProtoService atProtoService, INavigationService navigationService, ICredentialService credentialService)
		{
			this.atProtoService = atProtoService;
			this.navigationService = navigationService;
			this.credentialService = credentialService;
		}

		[RelayCommand]
		private void logout()
		{
			atProtoService.Session = null;
			credentialService.RemoveCredentials();
			navigationService.NavigateTo<LoginViewModel>();
		}
	}
}
