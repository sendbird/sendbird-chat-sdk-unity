// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// A class representing query to retrieve operator lists related to SbUser.
    /// </summary>
    /// @since 4.0.0
    public partial class SbOperatorListQuery
    {
        /// <summary>
        /// The maximum number of operators per queried page.
        /// </summary>
        /// @since 4.0.0
        public int Limit => _limit;

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
        /// Gets the list of operators. The queried result is passed to handler as List. If this method is repeatedly called after each LoadNextPage() is finished, it retrieves the following pages of the operator list.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void LoadNextPage(SbUserListHandler inCompletionHandler)
        {
            LoadNextPageInternal(inCompletionHandler);
        }
    }
}