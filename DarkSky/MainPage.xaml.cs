using Cube.UI.Services;
using DarkSky.Core.Services;
using DarkSky.Core.ViewModels;
using DarkSky.Pages;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
		private MainViewModel ViewModel = App.Current.Services.GetService<MainViewModel>();
		public MainPage()
		{
			this.InitializeComponent();
            WindowService.Initialize(AppTitleBar, AppTitle);
            AppNavigation.SelectedItem = AppNavigation.MenuItems[0];
			Bindings.Update();

			// fix weird titlebar bug
			AppTitleBar.Height = 50;
			AppTitleBar.Height = 48;
		}

		// used by URL
		public ImageSource img(string uri)
		{
			if (uri == null)
				throw new ArgumentNullException(nameof(uri));

			// Create a BitmapImage and set its UriSource to the provided Uri
			var bitmapImage = new BitmapImage(new Uri(uri));
			return bitmapImage;
		}

		private void AppNavigation_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
		{
			if(sender.SelectedItem == AppNavigation.MenuItems[0])
			{
				DualPane.Visibility = Visibility.Visible;
				WholePane.Visibility = Visibility.Collapsed;
				PrimaryPane.Navigate(typeof(FeedPage));
			}
			else if (sender.SelectedItem == AppNavigation.MenuItems[0])
			{
				DualPane.Visibility = Visibility.Visible;
				WholePane.Visibility = Visibility.Collapsed;
				PrimaryPane.Navigate(typeof(NotificationPage));
			}
			else if (sender.SelectedItem == AppNavigation.FooterMenuItems[0])
			{
				DualPane.Visibility = Visibility.Visible;
				WholePane.Visibility = Visibility.Collapsed;
				PrimaryPane.Navigate(typeof(ProfilePage));
			}
			else if (sender.SelectedItem == AppNavigation.FooterMenuItems[1])
			{
				WholePane.Visibility = Visibility.Visible;
				DualPane.Visibility = Visibility.Collapsed;
				PrimaryPane.Navigate(typeof(SettingsPage));
			}
		}
	}
}
