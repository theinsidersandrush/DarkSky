using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Cursors;
using DarkSky.Core.Messages;
using DarkSky.Core.Services;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.App.Bsky.Graph;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors
{
	/*
	 * Implements ICursorSource with common logic for refreshing and a constructor with ATProtocol service
	 * https://docs.bsky.app/docs/tutorials/viewing-feeds
	 */
	public abstract partial class AbstractCursorSource<T> : ObservableObject, ICursorSource<T>
	{
		[ObservableProperty]
		private bool isLoading = false;

		public ObservableCollection<T> Feed { get; } = new();

		protected string Cursor = "";
		protected ATProtoService atProtoService = ServiceContainer.Services.GetService<ATProtoService>();

		public virtual async Task GetMoreItemsAsync(int limit = 50)
		{
			try
			{
				if (IsLoading) return; // Don't load if items are currently loading
				IsLoading = true;

				// Call the inner implementation method for inheriting classes to load items
				// GetMoreItemsAsync() is virtual so specialised classes can override even this logic
				await OnGetMoreItemsAsync(limit);
				IsLoading = false;
			}
			catch (Exception ex)
			{
				WeakReferenceMessenger.Default.Send(new ErrorMessage(ex));
			}
		}

		protected abstract Task OnGetMoreItemsAsync(int limit = 50);

		public async Task RefreshAsync()
		{
			Cursor = "";
			Feed.Clear();
			await GetMoreItemsAsync();
		}

		public void Clear()
		{
			Feed.Clear();
			Cursor = "";
		}
	}
}
