using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Services;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Helpers
{
	/*
	 * Implements IFeedCursorSource with common logic for refreshing and a constructor with ATProtocol service
	 * https://docs.bsky.app/docs/tutorials/viewing-feeds
	 */
	public abstract partial class AbstractFeedCursorSource : ObservableObject, IFeedCursorSource
	{
		[ObservableProperty]
		private bool isLoading = false;

		public ObservableCollection<FeedViewPost> Feed { get; } = new();

		protected string Cursor = "";
		protected ATProtoService atProtoService;

		public AbstractFeedCursorSource(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
		}

		public abstract Task GetMoreItemsAsync(int limit = 50);

		public async Task RefreshAsync()
		{
			Cursor = "";
			Feed.Clear();
			await GetMoreItemsAsync();
		}
	}
}
