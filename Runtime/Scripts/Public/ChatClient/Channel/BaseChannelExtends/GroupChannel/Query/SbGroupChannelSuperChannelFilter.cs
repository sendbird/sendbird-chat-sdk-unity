// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Channel filter for super mode in group channels.
    /// </summary>
    /// @since 4.0.0
    public enum SbGroupChannelSuperChannelFilter
    {
        /// @since 4.0.0
        [JsonName("all")] All,

        /// @since 4.0.0
        [JsonName("super")] SuperChannelOnly,

        /// @since 4.0.0
        [JsonName("nonsuper")] NonSuperChannelOnly,

        /// @since 4.0.0
        [JsonName("broadcast_only")] BroadcastOnly,

        /// @since 4.0.0
        [JsonName("exclusive_only")] ExclusiveOnly
    }
}