using FishyFlip.Lexicon.App.Bsky.Embed;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DarkSky.UserControls.Embeds
{
	public sealed partial class ImageEmbed : UserControl
	{
		private List<ViewImage> Images = new();
		public ImageEmbed()
		{
			this.InitializeComponent();
		}

		public void AddImages(ViewImages embed)
		{
			foreach (ViewImage imageView in embed.Images)
				Images.Add(imageView);
		}
	}
}
