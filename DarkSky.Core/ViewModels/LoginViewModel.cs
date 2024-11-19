using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DarkSky.Core.Services;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.ViewModels
{
	public partial class LoginViewModel : ObservableObject
	{
		[ObservableProperty]
		private string userName;
		[ObservableProperty]
		private string password;

		private ATProtoService atProtoService;
		private INavigationService navigationService;
		public LoginViewModel(ATProtoService atProtoService, INavigationService navigationService)
		{
			this.atProtoService = atProtoService;
			this.navigationService = navigationService;
		}

		[RelayCommand]
		private async Task login()
		{
			await atProtoService.LoginAsync(UserName, Password);
			if (atProtoService.Session is not null)
				navigationService.NavigateTo<MainViewModel>();
		}
	}
}
