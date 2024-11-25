using FishyFlip.Lexicon;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DarkSky.UserControls
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

		private string ToRepostBy(ATObject? reason)
		{
			if (reason is null) return "";
			else return (reason as ReasonRepost).By.DisplayName;
		}

		private void SetFeedPost(FeedViewPost post)
		{
			if (post.Reply is not null)
			{
				FishyFlip.Lexicon.App.Bsky.Feed.ReplyRef reply = post.Reply;
				PostView root = (PostView)reply.Root;
				PostView parent = (PostView)reply.Parent;
				if (parent.Cid != root.Cid) // only show root reply if parent is not root
				{
					// post reply parent and root are not the same, show the root too
					FindName("ReplyRoot");
					ReplyRoot.Post = root;
				}

				FindName("ReplyParent"); // always show parent of reply
				ReplyParent.Post = parent;

				if (post.Reason is not null) {  // if a reply was retweeted then do not show parent or root posts
					if (ReplyRoot is not null) ReplyRoot.Visibility = Visibility.Collapsed;
					if (ReplyParent is not null) ReplyParent.Visibility = Visibility.Collapsed;
					UnloadObject(ReplyRoot);
					UnloadObject(ReplyParent);
				}
			}
		}
	}
}
