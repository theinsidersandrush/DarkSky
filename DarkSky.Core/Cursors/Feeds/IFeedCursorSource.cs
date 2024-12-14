using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Feeds
{
	/*
	 * An interface for loading FeedViewPost items using a Cursor with refresh and incremental loading support
	 * https://docs.bsky.app/docs/tutorials/viewing-feeds
	 */
	public interface IFeedCursorSource : ICursorSource<PostViewModel>, INotifyPropertyChanged;
}
