using DarkSky.Core.ViewModels;
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

		private async void RefreshContainer_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
			=> await ViewModel.ListsSource.RefreshAsync();

		#region Incremental loading code
		/*
		 * When Listview reaches bottom call FeedSource to generate more posts
		 */
		private ScrollViewer _scrollViewer;
		private void ListView_Loaded(object sender, RoutedEventArgs e)
		{
			_scrollViewer = GetScrollViewer(ListsList);

			if (_scrollViewer != null)
			{
				_scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
			}
		}

		private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			if (_scrollViewer.VerticalOffset >= _scrollViewer.ScrollableHeight - 10) // Threshold to trigger loading
				await ViewModel.ListsSource.GetMoreItemsAsync();
		}

		private ScrollViewer GetScrollViewer(DependencyObject element)
		{
			if (element is ScrollViewer)
				return (ScrollViewer)element;

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
			{
				var child = VisualTreeHelper.GetChild(element, i);
				var result = GetScrollViewer(child);
				if (result != null)
					return result;
			}
			return null;
		}

		#endregion
	}
}
