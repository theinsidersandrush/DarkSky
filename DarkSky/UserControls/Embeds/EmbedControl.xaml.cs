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
using static System.Net.Mime.MediaTypeNames;
using FishyFlip.Lexicon.Tools.Ozone.Team;
using Application = Windows.UI.Xaml.Application;
using System.Threading.Tasks;

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

		public EmbedControl()
		{
			this.InitializeComponent();
		}

		private async void SetEmbed(ATObject embed)
		{
			Container.Children.Clear(); // fix duplicates bug
			if (embed is not null)
			{
				if (embed.Type == "app.bsky.embed.images#view")
					addImages(embed);
				else if (embed.Type == "app.bsky.embed.external#view")
				{
					LinkEmbed link = new();
					link.AddLink(embed as ViewExternal);
					Container.Children.Add(link);
				}
				else if (embed.Type == "app.bsky.embed.record#view")
					await addQuoteAsync(embed);
				else if (embed.Type == "app.bsky.embed.recordWithMedia#view") // quote post with media
				{
					var x = (embed as ViewRecordWithMedia);
					// Add images if quote post has
					if (x.Media.Type == "app.bsky.embed.images#view")
						addImages(x.Media);
					// TO-DO: Add video check
					await addQuoteAsync(x.Record);
				}
				else
				{
					Debug.WriteLine(embed.Type);
					Container.Children.Clear();
					Container.Visibility = Visibility.Collapsed;
				}
			}
		}

		private async void addImages(ATObject embed)
		{
			/*if (IsPreview)
			{*/
				ImagePreviewEmbed image = new();
				image.AddImages(embed as ViewImages);
				Container.Children.Add(image);
			/*}
			else
			{
				ImageEmbed image = new();
				image.AddImages(embed as ViewImages);
				Container.Children.Add(image);
			}*/
		}

		private async Task addQuoteAsync(ATObject embed)
		{
			try
			{
				QuoteEmbed quote = new();
				quote.setpost(await PostFactory.CreateAsync(((ViewRecord)(embed as ViewRecordDef).Record)));
				Container.Children.Add(quote);
			}
			catch (Exception ex)
			{
				WeakReferenceMessenger.Default.Send(new ErrorMessage(ex));
			}
		}
	}
}
