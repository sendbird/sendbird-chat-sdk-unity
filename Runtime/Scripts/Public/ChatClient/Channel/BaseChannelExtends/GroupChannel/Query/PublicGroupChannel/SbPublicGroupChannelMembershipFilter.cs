// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Filter my channels or all ones in public group channels.
    /// </summary>
    /// @since 4.0.0
    public enum SbPublicGroupChannelMembershipFilter
    {
        /// @since 4.0.0
        [JsonName("all")] All,

        /// @since 4.0.0
        [JsonName("joined")] Joined
    }
}