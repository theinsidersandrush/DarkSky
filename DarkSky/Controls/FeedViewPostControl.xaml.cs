using FishyFlip.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DarkSky.Controls
{
	public sealed partial class FeedViewPostControl : UserControl
	{
		public FeedViewPost FeedPost
		{
			get => (FeedViewPost)GetValue(FeedPostProperty);
			set
			{
				SetValue(FeedPostProperty, value);
				if (value is not null)
					SetFeedPost(value);
			}
		}

		public static readonly DependencyProperty FeedPostProperty =
				   DependencyProperty.Register("FeedPost", typeof(FeedViewPost), typeof(FeedViewPostControl), null);

		public FeedViewPostControl()
		{
			this.InitializeComponent();
		}

		private void SetFeedPost(FeedViewPost post)
		{
		
		}
	}
}
