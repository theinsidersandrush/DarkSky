using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using DarkSky.Core.Services;
using DarkSky.Core.ViewModels;
using FishyFlip.Lexicon.App.Bsky.Feed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
	public sealed partial class CreatePostPage : Page
	{
		private ATProtoService ATProto = App.Current.Services.GetService<ATProtoService>();
		public CreatePostPage()
		{
			this.InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			WeakReferenceMessenger.Default.Send(new SecondaryNavigationMessage(new SecondaryNavigation(null)));
		}

		private async void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (PostText.Text.Length > 300) return;
			Post post = new Post();
			post.CreatedAt = DateTime.Now;
			post.Langs = new List<string>();
			post.Langs.Add("en");
			post.Text = PostText.Text;
			try
			{
				var x = (await ATProto.ATProtocolClient.CreatePostAsync(post)).AsT0;
				PostText.Text = "";
				WeakReferenceMessenger.Default.Send(new SecondaryNavigationMessage(new SecondaryNavigation(null)));
			}
			catch (Exception ex)
			{

			}
		}
    }
}
