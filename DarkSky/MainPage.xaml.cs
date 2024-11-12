using Cube.UI.Services;
using DarkSky.API;
using DarkSky.API.ATProtocol;
using DarkSky.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DarkSky
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		public BlueSkyClient Client;
        public MainPage()
        {
            this.InitializeComponent();
            WindowService.Initialize(AppTitleBar, AppTitle);
            AppNavigation.SelectedItem = AppNavigation.MenuItems[0];
			login();
		}

		public async void login()
		{
			// Refresh token and login
			if (CredentialHelper.Count() != 0)
			{
				var credential = CredentialHelper.RetrieveCredential();
				try
				{
					await App.DarkSkyClient.RefreshManualAsync(credential.Resource);
				}
				catch (Exception ex)
				{ // try full login again
					await App.DarkSkyClient.LoginAsync(credential.UserName, credential.Password);
				}
				Client = App.DarkSkyClient;
				Bindings.Update();
			}
			else
				((Frame)Window.Current.Content).Navigate(typeof(LoginPage));
		}
		// used by URL
		public ImageSource img(Uri uri)
		{
			if (uri == null)
				throw new ArgumentNullException(nameof(uri));

			// Create a BitmapImage and set its UriSource to the provided Uri
			var bitmapImage = new BitmapImage(uri);
			return bitmapImage;
		}
		private void AppNavigation_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
		{

		}
	}
}
