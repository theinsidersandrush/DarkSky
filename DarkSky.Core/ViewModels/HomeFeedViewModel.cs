using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Services;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace DarkSky.Core.ViewModels
{
	public partial class HomeFeedViewModel : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<FeedViewPost> timelineFeed = new();

		private HashSet<string> postID = new HashSet<string>();

		private ATProtoService atProtoService;
		public HomeFeedViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			Setup();
		}

		private async void Setup()
		{
			var feed = (await atProtoService.ATProtocolClient.Feed.GetTimelineAsync()).AsT0.Feed;
			foreach (var item in feed)
			{
				if (item.Reply is null) { // add regular posts
					// only add if it did not appear before, maybe as part of a reply chain
					if(!postID.Contains(item.Post.Cid.Hash.ToString()))
						TimelineFeed.Add(item);
				}
				else // the post is a reply, use logic to filter
				{
					// only allow replies if it replies to same author
					if(item.Reply.Parent.Author.Did.Handler == item.Post.Author.Did.Handler)
					{
						TimelineFeed.Add(item); // add the reply

						postID.Add(item.Post.Cid.Hash.ToString()); // add parent to hashset so we can filter if it appears later
					}
				}
			}
		}
	}
}
