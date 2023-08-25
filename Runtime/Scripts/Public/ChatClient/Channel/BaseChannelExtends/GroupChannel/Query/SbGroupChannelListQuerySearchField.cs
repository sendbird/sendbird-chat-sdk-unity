// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Filter types to query with SbGroupChannelListQuery.
    /// </summary>
    /// @since 4.0.0
    public enum SbGroupChannelListQuerySearchField
    {
        /// <summary>
        /// filter type to query for channel name
        /// </summary>
        /// @since 4.0.0
        [JsonName("channel_name")] ChannelName,

        /// <summary>
        /// filter type to query for member nickname
        /// </summary>
        /// @since 4.0.0
        [JsonName("member_nickname")] MemberNickname
    }
}