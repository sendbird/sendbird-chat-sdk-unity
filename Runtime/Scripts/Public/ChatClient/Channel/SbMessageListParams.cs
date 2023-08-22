// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// An object contains set of options for get message methods
    /// </summary>
    /// @since 4.0.0
    public partial class SbMessageListParams
    {
        /// <summary>
        /// Determines whether to sort the retrieved messages in reverse order. If `false`, the results are in ascending order.
        /// Default is false
        /// </summary>
        /// @since 4.0.0
        public bool Reverse { get; set; } = false;
        
        /// <summary>
        /// Determines whether to include the messages with the matching timestamp or message ID in the results.
        /// Default is false
        /// </summary>
        /// @since 4.0.0
        public bool IsInclusive { get; set; } = false;

        /// <summary>
        /// Restricts the search scope only to retrieve messages with the specified message type.
        /// Default is All
        /// </summary>
        /// @since 4.0.0
        public SbMessageTypeFilter MessageTypeFilter { get; set; } = SbMessageTypeFilter.All;
        
        /// <summary>
        /// Restricts the search scope only to retrieve the messages with the multiple specified custom message types.
        /// When the custom type filtering is not needed, the value should be set to `null.
        /// </summary>
        /// @since 4.0.0
        public List<string> CustomTypes { get; set; }
        
        /// <summary>
        /// Restricts the search scope only to retrieve the messages sent by the users with the specified user IDs.
        /// When the user ID filtering is not needed, the value should be set to null.
        /// </summary>
        /// @since 4.0.0
        public List<string> SenderUserIds { get; set; }

        /// <summary>
        /// Determines whether to include the metaarray information of the messages in the results.
        /// Default is false
        /// </summary>
        /// @since 4.0.0
        public bool IncludeMetaArray { get; set; } = false;

        /// <summary>
        /// Determines whether to include the reactions to the messages in the results.
        /// Default is false
        /// </summary>
        /// @since 4.0.0
        public bool IncludeReactions { get; set; } = false;

        /// <summary>
        /// Determines whether to include the thread information of the messages in the results when the results contain root messages.
        /// Default is false
        /// </summary>
        /// @since 4.0.0
        public bool IncludeThreadInfo { get; set; } = false;

        /// <summary>
        /// Determines whether to include parent message info.
        /// Default is false
        /// </summary>
        /// @since 4.0.0
        public bool IncludeParentMessageInfo { get; set; } = false;

        /// <summary>
        /// Determines which reply types to include.
        /// Default is None
        /// </summary>
        /// @since 4.0.0
        public SbReplyType ReplyType { get; set; } = SbReplyType.None;
        
        /// <summary>
        /// Determines whether to include only messages from the subChannel to which you belong in the results.
        /// Default is false
        /// </summary>
        /// @since 4.0.0
        public bool ShowSubChannelMessagesOnly { get; set; } = false;

        /// <summary>
        /// The number of messages to retrieve that were sent before the specified timestamp or message ID.
        /// Default is 20
        /// </summary>
        /// @warning This value is only used in open channels.
        /// @since 4.0.0
        public int PreviousResultSize { get; set; } = SendbirdChatMainContext.QUERY_DEFAULT_LIMIT;

        /// <summary>
        /// The number of messages to retrieve that were sent after the specified timestamp or message ID.
        /// Default is 0
        /// </summary>
        /// @since 4.0.0
        public int NextResultSize { get; set; } = 0;

        /// <summary>
        /// Checks whether given message is belonged to this params
        /// </summary>
        /// <param name="inMessage">SbBaseMessage instance</param>
        /// <returns>true if it is belonged, otherwise false</returns>
        /// @since 4.0.0
        public bool BelongsTo(SbBaseMessage inMessage)
        {
            return BelongsToInternal(inMessage);
        }

        /// <summary>
        /// Checks whether the given params is belonged to this object
        /// </summary>
        /// <param name="inParams">SbBaseMessageCreateParams (either user or file) instance</param>
        /// <returns>true if the given params is belonged to this object, otherwise false</returns>
        /// @since 4.0.0
        public bool BelongsTo(SbBaseMessageCreateParams inParams)
        {
            return BelongsToInternal(inParams);
        }
    }
}