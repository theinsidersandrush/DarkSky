using DarkSky.API;
using DarkSky.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DarkSky
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
			if (CredentialHelper.Count() != 0)
				((Frame)Window.Current.Content).Navigate(typeof(MainPage));
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
            await App.DarkSkyClient.LoginAsync(UsernameBox.Text, PasswordBox.Text);
            CredentialHelper.SaveCredential(UsernameBox.Text, PasswordBox.Text, BlueSkyClient.ATProtoClient.Session.RefreshToken);
            ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
		}
    }
}
