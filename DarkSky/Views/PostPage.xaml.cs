using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Factories;
using DarkSky.Core.Messages;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.Com.Atproto.Repo;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DarkSky.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	[INotifyPropertyChanged]
	public sealed partial class PostPage : Page
	{
		[ObservableProperty]
		PostViewModel post;
		public PostPage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			Post = e.Parameter as PostViewModel;
			SetPost(Post.InternalPost);
		}

		public void SetPost(PostView post)
		{
			if (post.Embed is null) return;
			if (post.Embed.Type == "app.bsky.embed.images#view")
			{
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			WeakReferenceMessenger.Default.Send(new SecondaryNavigationMessage(null));
		}

		private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			WeakReferenceMessenger.Default.Send(
				new SecondaryNavigationMessage(
					new SecondaryNavigation(typeof(ProfileViewModel), await ProfileFactory.Create(Post.InternalPost.Author))));
		}
	}
}
