using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Cursors;
using DarkSky.Core.Cursors.Feeds;
using DarkSky.Core.Messages;
using DarkSky.Core.ViewModels.Temporary;
using DarkSky.Templates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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

namespace DarkSky.UserControls
{
	public partial class CursorListView : UserControl
	{
		public ICursorSource CursorSource
		{
			get => (ICursorSource)GetValue(CursorSourceProperty);
			set => SetValue(CursorSourceProperty, value);
		}

		public static readonly DependencyProperty CursorSourceProperty =
			DependencyProperty.Register(nameof(CursorSource), typeof(ICursorSource), typeof(CursorListView), new PropertyMetadata(null, OnFeedSourceChanged));

		// Clear the old CursorSource and start loading items from the new one
		private static async void OnFeedSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ICursorSource)e.OldValue)?.Clear();
			await ((ICursorSource)e.NewValue)?.GetMoreItemsAsync();
		}

		public static readonly DependencyProperty ItemsSourceProperty =
				DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(CursorListView), new PropertyMetadata(null));

		public IEnumerable ItemsSource
		{
			get => (IEnumerable)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register(nameof(Header), typeof(object), typeof(CursorListView), new PropertyMetadata(null));

		public object Header
		{
			get => GetValue(HeaderProperty);
			set => SetValue(HeaderProperty, value);
		}

		public static readonly DependencyProperty ItemTemplateProperty =
			DependencyProperty.Register("ItemTemplate",typeof(DataTemplate),typeof(CursorListView),new PropertyMetadata(null, OnItemTemplateChanged));

		private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is CursorListView c)
			{
				c.CursorList.ItemTemplateSelector = null;
				c.CursorList.ItemTemplate = e.NewValue as DataTemplate;
			}
		}

		public DataTemplate ItemTemplate
		{
			get => (DataTemplate)GetValue(ItemTemplateProperty);
			set => SetValue(ItemTemplateProperty, value);
		}

		public event EventHandler<ItemClickEventArgs> ItemClicked;

		public CursorListView()
		{
			this.InitializeComponent();
			//CursorList.ItemTemplateSelector = new CursorItemSelector();
		}

		private void ListView_ItemClick(object sender, ItemClickEventArgs e)
		{
			ItemClicked?.Invoke(this, e);
		}

		private async void RefreshContainer_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
			=> await CursorSource.RefreshAsync();

		#region Incremental loading code
		/*
		 * When Listview reaches bottom call FeedSource to generate more posts
		 */
		private ScrollViewer _scrollViewer;
		private void ListView_Loaded(object sender, RoutedEventArgs e)
		{
			_scrollViewer = GetScrollViewer(CursorList);

			if (_scrollViewer != null)
			{
				_scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
			}
		}

		private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			if (_scrollViewer.VerticalOffset >= _scrollViewer.ScrollableHeight - 10) // Threshold to trigger loading
				await CursorSource.GetMoreItemsAsync();
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
