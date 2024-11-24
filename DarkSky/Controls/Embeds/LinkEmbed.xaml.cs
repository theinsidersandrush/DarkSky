using CommunityToolkit.Mvvm.ComponentModel;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DarkSky.Controls.Embeds
{
	[INotifyPropertyChanged]
	public sealed partial class LinkEmbed : UserControl
	{
		[ObservableProperty]
		private ExternalView embed;
		public LinkEmbed()
		{
			this.InitializeComponent();
		}

		public void AddLink(ExternalViewEmbed EmbedView)
		{
			this.Embed = EmbedView.External;
			Title.Text = String.IsNullOrEmpty(Embed.Title) ? Embed.Uri : Embed.Title;

			if (!String.IsNullOrEmpty(Embed.Thumb))
			{
				EmbedImage.Visibility = Visibility.Visible;
				EmbedImage.Source = new BitmapImage(new Uri(Embed.Thumb));
			}
		}
	}
}
