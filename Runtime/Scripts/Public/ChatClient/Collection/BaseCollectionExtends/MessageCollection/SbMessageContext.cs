// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The context of a channel, used in MessageCollectionHandler.
    /// </summary>
    /// @since 4.0.0
    public partial class SbMessageContext
    {
        /// <summary>
        /// The SbCollectionEventSource of current context
        /// </summary>
        /// @since 4.0.0
        public SbCollectionEventSource CollectionEventSource => _collectionEventSource;

        /// <summary>
        /// The sending status of the messages that's sent out from SbMessageCollectionHandler with this context.
        /// </summary>
        /// @since 4.0.0
        public SbSendingStatus MessageSendingStatus => _messagesSendingStatus;

        /// <summary>
        /// Whether the CollectionEventSource of this context is from the real-time events.
        /// </summary>
        /// <returns></returns>
        /// @since 4.0.0
        public bool IsFromEvent()
        {
            return IsFromEventInternal();
        }
    }
}