using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DarkSky.Core.Classes;
using DarkSky.Core.Services;
using FishyFlip.Lexicon;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.App.Bsky.Graph;
using FishyFlip.Lexicon.Com.Atproto.Repo;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.ViewModels.Temporary
{
	/*
	 * Temporary location
	 */
	public partial class PostViewModel : ObservableObject
	{
		#region
		/*
		 * For backwards-compatibility
		 */
		[ObservableProperty]
		private PostView internalPost;

		[ObservableProperty]
		private FeedViewPost internalFeedViewPost;

		#endregion

		[ObservableProperty]
		private string text;

		[ObservableProperty]
		private bool isReply = false; // Determines if the post is a reply, used by the reposted by "replying to" label in PostControl

		[ObservableProperty]
		private bool hasReply = false; // Determines whether to show the reply bar or not, related with FeedViewPost

		[ObservableProperty]
		private string repostedBy; // If the post has been reposted by someone, used with FeedViewPost

		#region Built in properties

		[ObservableProperty]
		private string? cid;

		[ObservableProperty]
		private DateTime createdAt;

		[ObservableProperty]
		private long likeCount;

		[ObservableProperty]
		private long repostCount;

		[ObservableProperty]
		private long quoteCount;

		[ObservableProperty]
		private long replyCount;

		#endregion

		[ObservableProperty]
		private bool isLiked;

		[ObservableProperty]
		private bool isReposted;

		[ObservableProperty]
		private bool isPinned;

		[ObservableProperty]
		private bool canReply;

		[ObservableProperty]
		private RichText richText;

		private Post PostRecord;
		private ATUri PostUri;
		private ATUri LikeUri;
		private ATUri RepostUri;
		private ATProtoService ATProtoService = ServiceContainer.Services.GetService<ATProtoService>();

		public PostViewModel(PostView post)
		{
			this.InternalPost = post;
			this.Cid = post.Cid;
			this.PostRecord = post.Record as Post;
			this.Text = PostRecord.Text ?? "";
			this.CreatedAt = post.IndexedAt ?? DateTime.Now;
			this.LikeCount = post.LikeCount ?? 0;
			this.ReplyCount = post.ReplyCount ?? 0;
			this.QuoteCount = post.QuoteCount ?? 0;
			this.RepostCount = post.RepostCount ?? 0;
			this.PostUri = post.Uri;
			this.richText = new RichText(this.text, this.PostRecord.Facets);
			if (post.Viewer is not null)
			{
				IsLiked = post.Viewer.Like is not null; // Post is liked by current user
				IsReposted = post.Viewer.Repost is not null; // Post is reposted by current user
				IsPinned = post.Viewer.Pinned ?? false;
				CanReply = !post.Viewer.ReplyDisabled ?? true;
				LikeUri = post.Viewer.Like;
				RepostUri = post.Viewer.Repost;
			}
		}

		[RelayCommand]
		public async Task<ThreadViewPost> GetThreadAsync()
		{
			var result = await ATProtoService.ATProtocolClient.Feed.GetPostThreadAsync(PostUri);
			if(result.IsT0)
			{
				return result.AsT0.Thread as ThreadViewPost;
			}
			throw new Exception();
		}

		[RelayCommand]
		public async Task Like()
		{
			if (IsLiked) // Unlike if it is liked
			{
				var result = await ATProtoService.ATProtocolClient.Feed.DeleteLikeAsync(LikeUri.Rkey);
				if (result.IsT0)
				{
					IsLiked = false;
					LikeUri = null;
					LikeCount--;
				}
			}
			else
			{
				Like record = new Like();
				record.CreatedAt = DateTime.Now;
				record.Subject = new StrongRef(InternalPost.Uri, InternalPost.Cid);
				var result = await ATProtoService.ATProtocolClient.Feed.CreateLikeAsync(record);
				if (result.IsT0)
				{
					IsLiked = true;
					LikeUri = result.AsT0.Uri;
					LikeCount++;
				}
			}
		}

		[RelayCommand]
		public async Task Repost()
		{
			if (IsReposted) // Unrepost if it is reposted
			{
				var result = await ATProtoService.ATProtocolClient.Feed.DeleteRepostAsync(RepostUri.Rkey);
				if (result.IsT0)
				{
					IsReposted = false;
					RepostUri = null;
					RepostCount--;
				}
			}
			else
			{
				Repost record = new Repost();
				record.CreatedAt = DateTime.Now;
				record.Subject = new StrongRef(InternalPost.Uri, InternalPost.Cid);
				var result = await ATProtoService.ATProtocolClient.Feed.CreateRepostAsync(record);
				if (result.IsT0)
				{
					IsReposted = true;
					RepostUri = result.AsT0.Uri;
					RepostCount++;
				}
			}
		}
	}
}
