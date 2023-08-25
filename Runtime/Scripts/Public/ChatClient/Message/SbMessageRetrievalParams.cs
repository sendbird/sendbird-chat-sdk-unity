// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a params for retrieving a single message.
    /// </summary>
    /// @since 4.0.0
    public class SbMessageRetrievalParams
    {
        /// <summary>
        /// The channel url.
        /// </summary>
        /// @since 4.0.0
        public string ChannelUrl { get; set; }

        /// <summary>
        /// The SbChannelType.
        /// </summary>
        /// @since 4.0.0
        public SbChannelType ChannelType { get; set; }

        /// <summary>
        /// The message ID.
        /// </summary>
        /// @since 4.0.0
        public long MessageId { get; set; }

        /// <summary>
        /// Whether the meta arrays should be included in the results.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeMetaArray { get; set; } = false;

        /// <summary>
        /// Whether the reaction data should be included in the results.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeReactions { get; set; } = false;

        /// <summary>
        /// Whether the thread information should be included in the results.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeThreadInfo { get; set; } = false;

        /// <summary>
        /// Whether the information of a parent message should be included in the reply messages included in the results.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeParentMessageInfo { get; set; } = false;
        
        /// @since 4.0.0
        public SbMessageRetrievalParams(string inChannelUrl, SbChannelType inChannelType, long inMessageId, bool inIncludeMetaArray = false,
                                        bool inIncludeReactions = false, bool inIncludeThreadInfo = false, bool inIncludeParentMessageInfo = false)
        {
            ChannelUrl = inChannelUrl;
            ChannelType = inChannelType;
            MessageId = inMessageId;
            IncludeMetaArray = inIncludeMetaArray;
            IncludeReactions = inIncludeReactions;
            IncludeThreadInfo = inIncludeThreadInfo;
            IncludeParentMessageInfo = inIncludeParentMessageInfo;
        }
    }
}