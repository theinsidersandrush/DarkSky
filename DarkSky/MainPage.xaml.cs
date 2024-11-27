using CommunityToolkit.Mvvm.Messaging;
using Cube.UI.Services;
using DarkSky.Core.Messages;
using DarkSky.Services;
using DarkSky.Core.ViewModels;
using DarkSky.Views;
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
using DarkSky.Core.ViewModels.Temporary;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DarkSky
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
	{
		private MainViewModel ViewModel = App.Current.Services.GetService<MainViewModel>();
		private readonly Dictionary<Type, Type> viewModelsToViews = new();
		public MainPage()
		{
			this.InitializeComponent();
            WindowService.Initialize(AppTitleBar, AppTitle);
            AppNavigation.SelectedItem = AppNavigation.MenuItems[0];
			Bindings.Update();

			// fix weird titlebar bug
			AppTitleBar.Height = 50;
			AppTitleBar.Height = 48;

			/*
			 * Navigate the secondary page
			 * The SecondaryNavigationMessage contains a "ViewModel" Type and a "payload" object
			 * The ViewModel type is mapped to a Page that is navigated to
			 */
			viewModelsToViews[typeof(PostViewModel)] = typeof(PostPage);
			WeakReferenceMessenger.Default.Register<SecondaryNavigationMessage>(this, (r, m) =>
			{
				if(m.Value.ViewModel is not null)
				{
					SecondaryPane.Visibility = Visibility.Visible;
					SecondaryPane.Navigate(viewModelsToViews[m.Value.ViewModel], m.Value.payload);
				}
				else //new SecondaryNavigation(null) go to null
					SecondaryPane.Visibility = Visibility.Collapsed;
			});

			WeakReferenceMessenger.Default.Register<ErrorMessage>(this, async (r, m) =>
			{

				Errorbar.IsOpen = true;
				Errorbar.Title = m.Value.Message;
				Errorbar.Content = m.Value.StackTrace;
				await Task.Delay(5000);
				Errorbar.IsOpen = false;
			});
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
			if (sender.SelectedItem is null) return;

			if(sender.SelectedItem == AppNavigation.MenuItems[0])
			{
				PrimaryPane.Navigate(typeof(HomePage));
			}
			else if (sender.SelectedItem == AppNavigation.MenuItems[1])
			{
				PrimaryPane.Navigate(typeof(NotificationPage));
			}
			else if (sender.SelectedItem == AppNavigation.MenuItems[2])
			{
				PrimaryPane.Navigate(typeof(ChatPage));
			}
			else if (sender.SelectedItem == AppNavigation.MenuItems[3])
			{
				PrimaryPane.Navigate(typeof(FeedsPage));
			}
			else if (sender.SelectedItem == AppNavigation.MenuItems[4])
			{
				PrimaryPane.Navigate(typeof(ListsPage));
			}
			else if (sender.SelectedItem == AppNavigation.FooterMenuItems[1])
			{
				PrimaryPane.Navigate(typeof(ProfilePage));
			}
			else if (sender.SelectedItem == AppNavigation.FooterMenuItems[2])
			{
				PrimaryPane.Navigate(typeof(SettingsPage));
			}
		}

		private void AppNavigation_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
		{
			if (args.InvokedItem is null) return;
			if (args.InvokedItem.ToString() == "New Post")
			{

				//WeakReferenceMessenger.Default.Send(new SecondaryNavigationMessage(1));
				SecondaryPane.Navigate(typeof(CreatePostPage));
			}
		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			// fix weird titlebar bug
			AppTitleBar.Height = 50;
			AppTitleBar.Height = 48;
		}
	}
}
