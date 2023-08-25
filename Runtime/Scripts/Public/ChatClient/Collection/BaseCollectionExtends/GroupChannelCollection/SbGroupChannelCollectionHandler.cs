// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// An handler used in GroupChannelCollection.
    /// </summary>
    /// @since 4.0.0
    public class SbGroupChannelCollectionHandler
    {
        /// @since 4.0.0
        public delegate void ChannelsAddedDelegate(SbGroupChannelContext inContext, IReadOnlyList<SbGroupChannel> inAddedChannels);
        
        /// @since 4.0.0
        public delegate void ChannelsUpdatedDelegate(SbGroupChannelContext inContext, IReadOnlyList<SbGroupChannel> inUpdatedChannels);
        
        /// @since 4.0.0
        public delegate void ChannelsDeletedDelegate(SbGroupChannelContext inContext, IReadOnlyList<string> inDeletedChannelUrls);

        /// <summary>
        /// Called when there are newly added GroupChannels.
        /// </summary>
        /// @since 4.0.0
        public ChannelsAddedDelegate OnChannelsAdded { get; set; }

        /// <summary>
        /// Called when one or more of the GroupChannels the SbGroupChannelCollection holds has been deleted.
        /// </summary>
        /// @since 4.0.0
        public ChannelsUpdatedDelegate OnChannelsUpdated { get; set; }

        /// <summary>
        /// Called when there's an update in one or more of the GroupChannels the GroupChannelCollection holds.
        /// </summary>
        /// @since 4.0.0
        public ChannelsDeletedDelegate OnChannelsDeleted { get; set; }
    }
}