// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The enum type to filter my group channels.
    /// </summary>
    /// @since 4.0.0
    public enum SbUnreadChannelFilter
    {
        /// <summary>
        /// Shows all my group channels.
        /// </summary>
        /// @since 4.0.0
        [JsonName("all")] All,

        /// <summary>
        /// Shows my group channels that have unread messages.
        /// </summary>
        /// @since 4.0.0
        [JsonName("unread_message")] UnreadMessage
    }
}