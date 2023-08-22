// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Params for creating a SbBlockedUserListQuery object.
    /// </summary>
    /// @since 4.0.0
    public class SbBlockedUserListQueryParams
    {
        /// <summary>
        /// User IDs filter. User list containing the passed User IDs will be returned.
        /// </summary>
        /// @since 4.0.0
        public List<string> UserIdsFilter { get; set; }

        /// <summary>
        /// The maximum number of items per queried page.
        /// </summary>
        /// @since 4.0.0
        public int Limit { get; set; } = SendbirdChatMainContext.QUERY_DEFAULT_LIMIT;
    }
}