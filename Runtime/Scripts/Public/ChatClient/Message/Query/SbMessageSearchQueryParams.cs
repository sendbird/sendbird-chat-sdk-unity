// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Params for creating a MessageSearchQuery object.
    /// </summary>
    /// @since 4.0.0
    public partial class SbMessageSearchQueryParams
    {
        /// <summary>
        /// The keyword to search for.
        /// </summary>
        /// @since 4.0.0
        public string Keyword { get; set; }

        /// <summary>
        /// The channel url to set as the search scope.
        /// </summary>
        /// @since 4.0.0
        public string ChannelUrl { get; set; }

        /// <summary>
        /// The custom type of channel to set as the search scope.
        /// </summary>
        /// @since 4.0.0
        public string ChannelCustomType { get; set; }

        /// <summary>
        /// Whether the search result is set to be reversed or not.
        /// </summary>
        /// @since 4.0.0
        public bool Reverse { get; set; } = false;

        /// <summary>
        /// Whether the search query should be an exact match or not.
        /// </summary>
        /// @since 4.0.0
        public bool ExactMatch { get; set; } = false;

        /// <summary>
        /// The start message timestamp set as the search range.
        /// </summary>
        /// @since 4.0.0
        public long MessageTimestampFrom { get; set; } = 0;

        /// <summary>
        /// The end message timestamp set as the search range.
        /// </summary>
        /// @since 4.0.0
        public long MessageTimestampTo { get; set; } = 0;

        /// <summary>
        /// The SbMessageSearchQueryOrder of the search.
        /// </summary>
        /// @since 4.0.0
        public SbMessageSearchQueryOrder Order { get; set; } = SbMessageSearchQueryOrder.Score;

        /// <summary>
        /// Whether the search query should be an advanced query or not.
        /// </summary>
        /// @since 4.0.0
        public bool AdvancedQuery { get; set; } = false;

        /// <summary>
        /// Target fields of the query to set as the search scope.
        /// </summary>
        /// @since 4.0.0
        public List<string> TargetFields { get; set; }

        /// <summary>
        /// The maximum number of BaseMessages per queried page. Defaults to 20.
        /// </summary>
        /// @since 4.0.0
        public int Limit { get; set; } = SendbirdChatMainContext.QUERY_DEFAULT_LIMIT;
    }
}