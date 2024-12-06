using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Cursors;
using DarkSky.Core.Factories;
using DarkSky.Core.Messages;
using FishyFlip.Lexicon;
using FishyFlip.Lexicon.App.Bsky.Embed;
using FishyFlip.Lexicon.App.Bsky.Feed;
using System.Diagnostics;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace DarkSky.UserControls.Embeds
{
	public sealed partial class EmbedControl : UserControl
	{
		public ATObject Embed
		{
			get => (ATObject)GetValue(EmbedProperty);
			set 
			{
				SetValue(EmbedProperty, value);
				SetEmbed(value);
			}
		}

		public static readonly DependencyProperty EmbedProperty = DependencyProperty.Register(nameof(Embed), typeof(ATObject), typeof(EmbedControl), new PropertyMetadata(null));

		public bool IsPreview
		{
			get => (bool)GetValue(IsPreviewProperty);
			set
			{
				SetValue(IsPreviewProperty, value);
				SetEmbed(Embed);
			}
		}

		public static readonly DependencyProperty IsPreviewProperty =
			DependencyProperty.Register(nameof(IsPreview), typeof(bool), typeof(EmbedControl), new PropertyMetadata(false));

		public EmbedControl()
		{
			this.InitializeComponent();
		}

		private async void SetEmbed(ATObject embed)
		{
			if (embed is not null)
			{
				if (embed.Type == "app.bsky.embed.images#view")
				{
					if (IsPreview)
					{
						ImagePreviewEmbed image = new();
						image.AddImages(embed as ViewImages);
						EmbedContent.Content = image;
					}
					else
					{
						ImageEmbed image = new();
						image.AddImages(embed as ViewImages);
						EmbedContent.Content = image;
					}
				}
				else if (embed.Type == "app.bsky.embed.external#view")
				{
					LinkEmbed link = new();
					link.AddLink(embed as ViewExternal);
					EmbedContent.Content = link;
				}
				else if (embed.Type == "app.bsky.embed.record#view")
				{
					try
					{
						PostControl quote = new();
						quote.Post = await PostFactory.Create(((ViewRecord)(embed as ViewRecordDef).Record));
						EmbedContent.Content = quote;
						Container.BorderThickness = new Thickness(1);
						//EmbedContent.BorderBrush = (Brush)Application.Current.Resources["MicaBorderBrush"];
					}
					catch (Exception ex)
					{
						WeakReferenceMessenger.Default.Send(new ErrorMessage(ex));
					}
				}
				else if (embed.Type == "app.bsky.embed.recordWithMedia")
				{
					/*EmbedContent.Visibility = Visibility.Visible;
					PostControl post = new();
					//embed.Post = (post.Embed as RecordWithMediaViewEmbed).Record.Post;
					EmbedContent.Content = post;*/
				}
				else
				{
					Debug.WriteLine(embed.Type);
					EmbedContent.Content = null;
					Container.Visibility = Visibility.Collapsed;
				}
			}
		}
	}
}
