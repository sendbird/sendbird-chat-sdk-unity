// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Params for creating a SbFriendListQuery object.
    /// </summary>
    /// @since 4.0.0
    public class SbFriendListQueryParams
    {
        /// <summary>
        /// The maximum number of items per queried page.
        /// </summary>
        /// @since 4.0.0
        public int Limit { get; set; } = SendbirdChatMainContext.QUERY_DEFAULT_LIMIT;
    }
}