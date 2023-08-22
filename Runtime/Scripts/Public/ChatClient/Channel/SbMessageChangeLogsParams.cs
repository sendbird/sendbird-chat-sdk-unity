// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a message list params.
    /// </summary>
    /// @since 4.0.0
    public class SbMessageChangeLogsParams
    {
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

        /// <summary>
        /// Determines the reply types to include in the results.
        /// </summary>
        /// @since 4.0.0
        public SbReplyType ReplyType { get; set; } = SbReplyType.None;
    }
}