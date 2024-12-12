using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Classes;
using DarkSky.Core.Cursors;
using DarkSky.Core.Factories;
using DarkSky.Core.Services;
using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Lexicon.App.Bsky.Richtext;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DarkSky.Core.ViewModels.Temporary
{
	public partial class ProfileViewModel : ObservableObject, INotifyPropertyChanged
	{
		[ObservableProperty]
		private ATHandle? handle;

		[ObservableProperty]
		private ATDid? did;

		#region Built in properties

		[ObservableProperty]
		private string? displayName;

		[ObservableProperty]
		private string? description;

		[ObservableProperty]
		private string? avatar;

		[ObservableProperty]
		private string? banner;

		[ObservableProperty]
		private long followsCount;

		[ObservableProperty]
		private long followersCount;

		[ObservableProperty]
		private long postsCount;

		#endregion

		[ObservableProperty]
		private PostViewModel? pinnedPost;

		[ObservableProperty]
		private ProfileViewDetailed profile;

		/* 
		 * Descriptions support Facets however the API only returns a plain string
		 * We need to manually parse the facets from the Description string
		 * We do this in the constructor with FishyFlip Facet.Parse method
		 */
		[ObservableProperty]
		private RichText richDescription;

		#region Feeds
		public ObservableCollection<FeedNavigationItem> ProfileNavigationItems = new();

		[ObservableProperty]
		private FeedNavigationItem selectedProfileNavigationItem;
		#endregion

		public ProfileViewModel(ProfileViewDetailed profileView) 
		{
			this.Profile = profileView;
			this.Handle = profileView.Handle;
			this.Did = profileView.Did;
			this.DisplayName = profileView.DisplayName ?? profileView.Handle.ToString();
			this.Avatar = profileView.Avatar;
			this.Banner = profileView.Banner;
			this.Description = profileView.Description ?? "";
			this.FollowersCount = profileView.FollowersCount ?? 0;
			this.FollowsCount = profileView.FollowsCount ?? 0;
			this.PostsCount = profileView.PostsCount ?? 0;
			ProfileNavigationItems.Add(new FeedNavigationItem("Posts", new ProfileFeedCursorSource(this, "posts_no_replies")));
			SelectedProfileNavigationItem = ProfileNavigationItems[0];
			ProfileNavigationItems.Add(new FeedNavigationItem("Replies", new ProfileFeedCursorSource(this, "posts_with_replies")));
			ProfileNavigationItems.Add(new FeedNavigationItem("Media", new ProfileFeedCursorSource(this, "posts_with_media")));

			// Profile Descriptions supports Facets but the API does not return them
			// To fix this we manually parse the Facets using FishyFlip Facet.Parse method
			RichDescription = new RichText(Description, Facet.Parse(Description, new ProfileViewBasic[0]).ToList());

			Setup();
		}

		private async void Setup()
		{
			if (Profile.PinnedPost is not null)
			{
				this.PinnedPost = await PostFactory.CreateAsync(Profile.PinnedPost.Uri);
				this.PinnedPost.IsPinned = true;
			}
		}
	}
}
