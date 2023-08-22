// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// A class representing query to retrieve list of SbBaseMessages that matches a given query with given filters.
    /// </summary>
    /// @since 4.0.0
    public partial class SbMessageSearchQuery
    {
        /// <summary>
        /// The current search keyword.
        /// </summary>
        /// @since 4.0.0
        public string Keyword => _queryParams.Keyword;

        /// <summary>
        /// Whether the current search result is set to be reversed or not.
        /// </summary>
        /// @since 4.0.0
        public bool Reverse => _queryParams.Reverse;

        /// <summary>
        /// The current custom type of channel set as the search scope.
        /// </summary>
        /// @since 4.0.0
        public string ChannelCustomType => _queryParams.ChannelCustomType;

        /// <summary>
        /// Whether the current search query should be an exact match or not.
        /// </summary>
        /// @since 4.0.0
        public bool ExactMatch => _queryParams.ExactMatch;

        /// <summary>
        /// The start message timestamp set as the search range.
        /// </summary>
        /// @since 4.0.0
        public long MessageTimestampFrom => _queryParams.MessageTimestampFrom;

        /// <summary>
        /// The end message timestamp set as the search range.
        /// </summary>
        /// @since 4.0.0
        public long MessageTimestampTo => _queryParams.MessageTimestampTo;

        /// <summary>
        /// The current order method.
        /// </summary>
        /// @since 4.0.0
        public SbMessageSearchQueryOrder Order => _queryParams.Order;

        /// <summary>
        /// Whether the current search query should be an advanced query or not.
        /// </summary>
        /// @since 4.0.0
        public bool IsAdvancedQuery => _queryParams.AdvancedQuery;

        /// <summary>
        /// The target fields of the current query as the search scope.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> TargetFields => _queryParams.TargetFields;

        /// <summary>
        /// The total count of results that matches the given search.
        /// </summary>
        /// @since 4.0.0
        public int TotalCount => _totalCount;

        /// <summary>
        /// The maximum number of BaseMessages per single query.
        /// </summary>
        /// @since 4.0.0
        public int Limit => _queryParams.Limit;

        /// <summary>
        /// Whether there is a next page.
        /// </summary>
        /// @since 4.0.0
        public bool HasNext => _hasNext;

        /// <summary>
        /// Whether the current query is in communication progress with server.
        /// </summary>
        /// @since 4.0.0
        public bool IsLoading => _isLoading;

        /// <summary>
        /// Gets the list of SbBaseMessages that matches the given search. The queried result is passed to handler as list. If this method is repeatedly called after each LoadNextPage() is finished, it retrieves the following search results as SbBaseMessage list.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        public void LoadNextPage(SbMessageListHandler inCompletionHandler)
        {
            LoadNextPageInternal(inCompletionHandler);
        }
    }
}