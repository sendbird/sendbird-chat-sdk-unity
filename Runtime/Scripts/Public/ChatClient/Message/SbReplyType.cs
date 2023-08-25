// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// ReplyType to be used in loading messages.
    /// </summary>
    /// @since 4.0.0
    public enum SbReplyType
    {
        /// <summary>
        /// Do not include any replies.
        /// </summary>
        /// @since 4.0.0
        None = 0,

        /// <summary>
        /// Include all replies.
        /// </summary>
        /// @since 4.0.0
        All,

        /// <summary>
        /// Only include replies which has SbBaseMessage.isReplyToChannel set to true.
        /// </summary>
        /// @since 4.0.0
        OnlyReplyToChannel
    }
}