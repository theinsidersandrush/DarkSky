﻿using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using DarkSky.Core.ViewModels;
using DarkSky.Core.ViewModels.Temporary;
using Microsoft.Extensions.DependencyInjection;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DarkSky.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ListsPage : Page
	{
		private ListsViewModel ViewModel = App.Current.Services.GetService<ListsViewModel>();
		public ListsPage()
		{
			this.InitializeComponent();
			_ = ViewModel.ListsSource.GetMoreItemsAsync();
		}

		private void ListsList_ItemClick(object sender, ItemClickEventArgs e)
		{
			WeakReferenceMessenger.Default.Send(
				new SecondaryNavigationMessage(
					new SecondaryNavigation(typeof(ListViewModel), e.ClickedItem as ListViewModel)));
		}
	}
}
