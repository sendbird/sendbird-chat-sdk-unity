// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Filter public group channel or private one in group channels.
    /// </summary>
    /// @since 4.0.0
    public enum SbGroupChannelPublicChannelFilter
    {
        /// @since 4.0.0
        [JsonName("all")] All,

        /// @since 4.0.0
        [JsonName("public")] Public,

        /// @since 4.0.0
        [JsonName("private")] Private
    }
}