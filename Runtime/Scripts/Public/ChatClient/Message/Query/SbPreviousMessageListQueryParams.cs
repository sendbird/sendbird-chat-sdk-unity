// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Params for creating a SbPreviousMessageListQuery object.
    /// </summary>
    /// @since 4.0.0
    public class SbPreviousMessageListQueryParams
    {
        /// <summary>
        /// Indicates whether the queried result will be reversed. If true, the result will be returned by creation time descending order.
        /// </summary>
        /// @since 4.0.0
        public bool Reverse { get; set; } = false;

        /// <summary>
        /// Message type filter. SbMessageTypeFilter
        /// </summary>
        /// @since 4.0.0
        public SbMessageTypeFilter MessageTypeFilter { get; set; } = SbMessageTypeFilter.All;

        /// <summary>
        /// The custom type filter of the message. When set, only messages with customType that equals to one of customTypes will be returned. When a customType is an empty string (""), all messages without a custom type will be returned.
        /// </summary>
        /// @since 4.0.0
        public List<string> CustomTypesFilter { get; set; }

        /// <summary>
        /// Sender user ids filter.
        /// </summary>
        /// @since 4.0.0
        public List<string> SenderUserIdsFilter { get; set; }

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
        /// Determines the reply types to include in the results.
        /// </summary>
        /// @since 4.0.0
        public SbReplyType ReplyType { get; set; } = SbReplyType.None;

        /// <summary>
        /// If set to true, only messages that belong to current user's subchannel is fetched. If set to false, all messages will be fetched. Default is false. Takes effect only when the requested channel is a dynamically partitioned open channel.
        /// </summary>
        /// @since 4.0.0
        public bool ShowSubChannelMessagesOnly { get; set; } = false;

        /// <summary>
        /// The maximum number of items per queried page.
        /// </summary>
        /// @since 4.0.0
        public int Limit { get; set; } = SendbirdChatMainContext.QUERY_DEFAULT_LIMIT;
    }
}