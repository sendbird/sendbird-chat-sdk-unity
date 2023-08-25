// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Params for creating a SbApplicationUserListQuery object.
    /// </summary>
    /// @since 4.0.0
    public class SbApplicationUserListQueryParams
    {
        /// <summary>
        /// User IDs filter. User list containing the passed User IDs will be returned.
        /// </summary>
        /// @since 4.0.0
        public List<string> UserIdsFilter { get; set; }

        /// <summary>
        /// A filter to return users whose nicknames start with the specified string.
        /// </summary>
        /// @since 4.0.0
        public string NicknameStartsWithFilter { get; set; }

        /// <summary>
        /// The maximum number of items per queried page.
        /// </summary>
        /// @since 4.0.0
        public int Limit { get; set; } = SendbirdChatMainContext.QUERY_DEFAULT_LIMIT;

        /// <summary>
        /// The meta data key filter. This query will return users that has the meta data key and values. 
        /// </summary>
        /// @since 4.0.0
        public string MetaDataKeyFilter { get; private set; }

        /// <summary>
        /// The meta data values filter. This query will return users that has the meta data key and values.
        /// </summary>
        /// @since 4.0.0
        public List<string> MetaDataValuesFilter { get; private set; }

        /// <summary>
        /// Sets meta data filter.
        /// </summary>
        /// <param name="inKey">The key of the meta data to use for filter.</param>
        /// <param name="inValues">The values of the meta data to use for filter.</param>
        /// @since 4.0.0
        public void SetMetaDataFilter(string inKey, List<string> inValues)
        {
            MetaDataKeyFilter = inKey;
            MetaDataValuesFilter = inValues;
        }
    }
}