using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Helpers;
using DarkSky.Core.Messages;
using DarkSky.Core.Services;
using DarkSky.Core.ViewModels;
using DarkSky.Helpers;
using FishyFlip.Models;
using Google.Protobuf.Compiler;
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
using Microsoft.Toolkit.Uwp;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using Windows.UI.Xaml.Media;
using FishyFlip.Lexicon.App.Bsky.Feed;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DarkSky.UserControls
{
	public sealed partial class FeedListView : UserControl
	{
		public IFeedCursorSource FeedSource
		{
			get => (IFeedCursorSource)GetValue(FeedSourceProperty);
			set => SetValue(FeedSourceProperty, value);
		}

		public static readonly DependencyProperty FeedSourceProperty =
			DependencyProperty.Register(nameof(FeedSource), typeof(IFeedCursorSource), typeof(FeedListView), new PropertyMetadata(null, OnFeedSourceChanged));
		
		// Clear the old CursorSource and start loading items from the new one
		private static async void OnFeedSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((IFeedCursorSource)e.OldValue)?.Clear();
			await ((IFeedCursorSource)e.NewValue)?.GetMoreItemsAsync();
		} 

		public FeedListView()
		{
			this.InitializeComponent();
		}

		private void ListView_ItemClick(object sender, ItemClickEventArgs e)
		{
			WeakReferenceMessenger.Default.Send(new TemporaryOpenPostMessage((e.ClickedItem as FeedViewPost).Post));
		}

		private async void RefreshContainer_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
			=> await FeedSource.RefreshAsync();

		private ScrollViewer _scrollViewer;
		private void ListView_Loaded(object sender, RoutedEventArgs e)
		{
			_scrollViewer = GetScrollViewer(FeedList);

			if (_scrollViewer != null)
			{
				_scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
			}
		}

		private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			if (_scrollViewer.VerticalOffset >= _scrollViewer.ScrollableHeight - 10) // Threshold to trigger loading
				await FeedSource.GetMoreItemsAsync();
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
	}
}
