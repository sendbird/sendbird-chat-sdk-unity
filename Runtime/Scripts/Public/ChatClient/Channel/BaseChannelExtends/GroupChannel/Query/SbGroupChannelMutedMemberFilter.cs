// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Filter operators in group channels.
    /// </summary>
    /// @since 4.0.0
    public enum SbGroupChannelMutedMemberFilter
    {
        /// @since 4.0.0
        [JsonName("all")] All,

        /// @since 4.0.0
        [JsonName("muted")] Muted,

        /// @since 4.0.0
        [JsonName("unmuted")] Unmuted
    }
}