// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// 
    /// </summary>
    /// @since 4.0.0
    public class SbMessageCollectionHandler
    {
        /// @since 4.0.0
        public delegate void ChannelUpdatedDelegate(SbMessageContext inContext, SbGroupChannel inUpdatedChannel);
        
        /// @since 4.0.0
        public delegate void ChannelDeletedDelegate(SbMessageContext inContext, string inDeletedChannelUrl);
        
        /// @since 4.0.0
        public delegate void MessagesAddedDelegate(SbMessageContext inContext, IReadOnlyList<SbBaseMessage> inAddedMessages);
        
        /// @since 4.0.0
        public delegate void MessagesUpdatedDelegate(SbMessageContext inContext, IReadOnlyList<SbBaseMessage> inUpdatedMessages);
        
        /// @since 4.0.0
        public delegate void MessagesDeletedDelegate(SbMessageContext inContext, IReadOnlyList<SbBaseMessage> inDeletedMessages);
        
        /// @since 4.0.0
        public delegate void HugeGapDetectedDelegate();

        /// <summary>
        /// Called when there's a change in the channel this collection holds.
        /// </summary>
        /// @since 4.0.0
        public ChannelUpdatedDelegate OnChannelUpdated { get; set; }

        /// <summary>
        /// Called when the channel this collection holds is deleted.
        /// </summary>
        /// @since 4.0.0
        public ChannelDeletedDelegate OnChannelDeleted { get; set; }

        /// <summary>
        /// Called when one or more SbBaseMessage is added to this collection.
        /// </summary>
        /// @since 4.0.0
        public MessagesAddedDelegate OnMessagesAdded { get; set; }

        /// <summary>
        /// Called when one or more SbBaseMessage is update in this collection.
        /// </summary>
        /// @since 4.0.0
        public MessagesUpdatedDelegate OnMessagesUpdated { get; set; }

        /// <summary>
        /// Called when one or more SbBaseMessage is deleted from this collection.
        /// </summary>
        /// @since 4.0.0
        public MessagesDeletedDelegate OnMessagesDeleted { get; set; }

        /// <summary>
        /// Called when the collection has detected a huge gap between current message list. This can happen SDK checks for missing messages, which occurs in two cases.
        /// </summary>
        /// @since 4.0.0
        public HugeGapDetectedDelegate OnHugeGapDetected { get; set; }
    }
}