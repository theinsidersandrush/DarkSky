using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Factories;
using DarkSky.Core.Messages;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Models;
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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DarkSky.UserControls.Embeds
{
	[INotifyPropertyChanged]
	public sealed partial class QuoteEmbed : UserControl
	{
		[ObservableProperty]
		private PostViewModel post;
		public QuoteEmbed()
		{
			this.InitializeComponent();
		}

		public void setpost(PostViewModel post)
		{
			Post = post;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (Post is null) return;
			WeakReferenceMessenger.Default.Send(new SecondaryNavigationMessage(
				new SecondaryNavigation(typeof(PostViewModel), Post)));
		}
	}
}
