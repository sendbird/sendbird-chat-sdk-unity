// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The enum type to represent various kinds of counts.
    /// </summary>
    /// @since 4.0.0
    public enum SbCountPreference
    {
        /// <summary>
        /// The channel of this preference has all of count.
        /// </summary>
        /// @since 4.0.0
        [JsonName("all")] All,

        /// <summary>
        /// The channel of this preference has only unread message count.
        /// </summary>
        /// @since 4.0.0
        [JsonName("unread_message_count_only")] UnreadMessageCountOnly,

        /// <summary>
        /// The channel of this preference has only unread mention count.
        /// </summary>
        /// @since 4.0.0
        [JsonName("unread_mention_count_only")] UnreadMentionCountOnly,

        /// <summary>
        /// The channel of this preference does not get any count.
        /// </summary>
        /// @since 4.0.0
        [JsonName("off")] Off
    }
}