// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Collection that handles channel lists.
    /// </summary>
    /// @since 4.0.0
    public partial class SbGroupChannelCollection : SbBaseCollection
    {
        /// @since 4.0.0
        public SbGroupChannelCollectionHandler GroupChannelCollectionHandler { get => _groupChannelCollectionHandler; set => _groupChannelCollectionHandler = value; }

        /// <summary>
        /// Total channel list.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<SbGroupChannel> ChannelList => _cachedChannelList;

        /// <summary>
        /// Whether there's more data to load.
        /// </summary>
        /// @since 4.0.0
        public bool HasNext => _hasNext;

        /// <summary>
        /// Loads next channel lists, depending on the SbGroupChannelListQueryOrder set in the current collection.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void LoadMore(SbGroupChannelListHandler inCompletionHandler)
        {
            LoadMoreInternal(inCompletionHandler);
        }

        /// <summary>
        /// Disposes current SbGroupChannelCollection and stops all events from being received.
        /// </summary>
        /// @since 4.0.0
        public void Dispose()
        {
            DisposeInternal();
        }
    }
}