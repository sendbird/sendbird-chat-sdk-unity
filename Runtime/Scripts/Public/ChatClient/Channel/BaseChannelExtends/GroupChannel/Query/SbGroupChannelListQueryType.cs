// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The query type for SbGroupChannelListQuery.
    /// </summary>
    /// @since 4.0.0
    public enum SbGroupChannelListQueryType
    {
        /// @since 4.0.0
        [JsonName("and")] And,

        /// @since 4.0.0
        [JsonName("or")] Or
    }
}