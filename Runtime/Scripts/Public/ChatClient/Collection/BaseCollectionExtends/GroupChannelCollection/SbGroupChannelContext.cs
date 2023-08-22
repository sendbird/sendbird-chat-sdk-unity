// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The context of a channel, used in SbGroupChannelCollectionHandler.
    /// </summary>
    /// @since 4.0.0
    public partial class SbGroupChannelContext
    {
        /// <summary>
        /// the SbCollectionEventSource of current context.
        /// </summary>
        /// @since 4.0.0
        public SbCollectionEventSource CollectionEventSource => _collectionEventSource;

        /// <summary>
        /// Whether the SbCollectionEventSource of this context is from the real-time events.
        /// </summary>
        /// @since 4.0.0
        public bool IsFromEvent()
        {
            return IsFromEventInternal();
        }
    }
}