using DarkSky.UserControls.Embeds;
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
		public PostView Post
		{
			get => (PostView)GetValue(PostProperty);
			set
			{
				SetValue(PostProperty, value);
				if(value is not null)
					SetPost(value);
			}
		}

		public static readonly DependencyProperty PostProperty =
				   DependencyProperty.Register("Post", typeof(PostView), typeof(PostControl), null);

		public bool ShowReplyBar
		{
			get => (bool)GetValue(ShowReplyBarProperty);
			set => SetValue(ShowReplyBarProperty, value);
		}

		public static readonly DependencyProperty ShowReplyBarProperty =
			DependencyProperty.Register(nameof(ShowReplyBar), typeof(bool), typeof(PostControl), new PropertyMetadata(false));


		public PostControl()
		{
			this.InitializeComponent();
		}

		public void SetPost(PostView post) 
		{
			if (post.Embed is not null)
			{
				if (post.Embed.Type == "app.bsky.embed.images#view")
				{
					EmbedContent.Visibility = Visibility.Visible;
					Embeds.ImageEmbed embed = new();
					embed.AddImages(post.Embed as ImageViewEmbed);
					EmbedContent.Content = embed;
				}
				else if (post.Embed.Type == "app.bsky.embed.external#view")
				{
					EmbedContent.Visibility = Visibility.Visible;
					Embeds.LinkEmbed embed = new();
					embed.AddLink(post.Embed as ExternalViewEmbed);
					EmbedContent.Content = embed;
				}
				else if (post.Embed.Type == "app.bsky.embed.record#view")
				{
					EmbedContent.Visibility = Visibility.Visible;
					PostControl embed = new();
					embed.Post = (post.Embed as RecordViewEmbed).Post;
					EmbedContent.Content = embed;
				}
				else if (post.Embed.Type == "app.bsky.embed.recordWithMedia")
				{
					EmbedContent.Visibility = Visibility.Visible;
					PostControl embed = new();
					embed.Post = (post.Embed as RecordWithMediaViewEmbed).Record.Post;
					EmbedContent.Content = embed;
				}
				else
				{
					Debug.WriteLine(post.Embed.Type);
					EmbedContent.Content = null;
					EmbedContent.Visibility = Visibility.Collapsed;
				}
			}
		}
	}
}
