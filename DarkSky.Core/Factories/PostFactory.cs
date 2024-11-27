using DarkSky.Core.Services;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Embed;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.Com.Atproto.Sync;
using FishyFlip.Models;
using Ipfs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Factories
{
	/*
	 * Factory method to return PostViewModels from FeedViewPost, ViewRecord, links etc
	 */
	public class PostFactory
	{
		public static PostViewModel Create(FeedViewPost post)
		{
			PostViewModel ViewModel = new(post.Post) { InternalFeedViewPost = post };
			if (post.Reason is not null) // Post might be reposted TODO: (or pinned)
			{
				ViewModel.RepostedBy = (post.Reason as ReasonRepost).By.DisplayName;
			}
			if (post.Reply is not null) // the post is a reply to another post
				ViewModel.IsReply = true;
			return ViewModel;
		}

		public async static Task<PostViewModel> Create(ViewRecord viewRecord)
		{
			var p = ServiceContainer.Services.GetService<ATProtoService>();
			var proto = p.ATProtocolClient;
			var list = new List<ATUri>();
			list.Add(viewRecord.Uri);
			var result = await proto.GetPostsAsync(list);
			return new PostViewModel(result.AsT0.Posts[0]);
		}
	}
}
