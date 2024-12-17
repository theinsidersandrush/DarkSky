﻿using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Factories;
using DarkSky.Core.Messages;
using DarkSky.Core.ViewModels.Temporary;
using DarkSky.UserControls.Embeds;
using FishyFlip.Lexicon;
using FishyFlip.Lexicon.App.Bsky.Embed;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DarkSky.UserControls
{
	public sealed partial class PostControl : UserControl
	{
		public PostViewModel Post
		{
			get => (PostViewModel)GetValue(PostProperty);
			set => SetValue(PostProperty, value);
		}

		public static readonly DependencyProperty PostProperty =
				   DependencyProperty.Register("Post", typeof(PostViewModel), typeof(PostControl), new PropertyMetadata(null, OnPostChanged));

		// Adding the PropertyChanged method fixes virtualisation duplication bug
		private static void OnPostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is PostControl p)
				p.Bindings.Update();
		}

		public PostControl()
		{
			this.InitializeComponent();
		}
	}
}
