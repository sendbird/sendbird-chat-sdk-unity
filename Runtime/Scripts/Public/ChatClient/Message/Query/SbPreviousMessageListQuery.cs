// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// A class representing query to retrieve previous message list for channels.
    /// </summary>
    /// @since 4.0.0
    public partial class SbPreviousMessageListQuery
    {
        /// <summary>
        /// Indicates whether the queried result will be reversed. If true, the result will be returned by creation time descending order.
        /// </summary>
        /// @since 4.0.0
        public bool Reverse => _reverse;

        /// <summary>
        /// Message type filter. SbMessageTypeFilter
        /// </summary>
        /// @since 4.0.0
        public SbMessageTypeFilter MessageTypeFilter => _messageTypeFilter;

        /// <summary>
        /// The custom type filter of the message.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> CustomTypesFilter => _customTypesFilter;

        /// <summary>
        /// Sender user ids filter.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> SenderUserIdsFilter => _senderUserIdsFilter;

        /// <summary>
        /// Whether the meta arrays should be included in the results.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeMetaArray => _includeMetaArray;

        /// <summary>
        /// Whether the reaction data should be included in the results.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeReactions => _includeReactions;

        /// <summary>
        /// Whether the thread information should be included in the results.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeThreadInfo => _includeThreadInfo;

        /// <summary>
        /// Whether the information of a parent message should be included in the reply messages included in the results.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeParentMessageInfo => _includeParentMessageInfo;

        /// <summary>
        /// Determines the reply types to include in the results.
        /// </summary>
        /// @since 4.0.0
        public SbReplyType ReplyType => _replyType;

        /// <summary>
        /// If set to true, only messages that belong to current user's subchannel is fetched. If set to false, all messages will be fetched. Default is false. Takes effect only when the requested channel is a dynamically partitioned open channel.
        /// </summary>
        /// @since 4.0.0
        public bool ShowSubChannelMessagesOnly => _showSubChannelMessagesOnly;

        /// <summary>
        /// The maximum number of messages per queried page.
        /// </summary>
        /// @since 4.0.0
        public int Limit => _limit;

        /// <summary>
        /// Whether there is more page to load.
        /// </summary>
        /// @since 4.0.0
        public bool HasNext => _hasNext;

        /// <summary>
        /// Whether the current query is in communication progress with server.
        /// </summary>
        /// @since 4.0.0
        public bool IsLoading => _isLoading;

        /// <summary>
        /// Requests query result for the previous messages. The queried result is passed to handler as list.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void LoadNextPage(SbMessageListHandler inCompletionHandler)
        {
            LoadNextPageInternal(inCompletionHandler);
        }
    }
}