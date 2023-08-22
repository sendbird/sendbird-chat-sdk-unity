// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents channel types.
    /// </summary>
    /// @since 4.0.0
    public enum SbChannelType
    {
        /// @since 4.0.0
        [JsonName("group")] Group,

        /// @since 4.0.0
        [JsonName("open")] Open,

        /// @since 4.0.0
        [JsonName("feed")] Feed,
    }
}