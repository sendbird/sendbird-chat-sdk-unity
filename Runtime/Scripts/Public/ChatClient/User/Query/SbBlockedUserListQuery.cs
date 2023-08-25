// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// The SbBlockedUserListQuery class is a query class for getting the list of blocked users by the current user.
    /// </summary>
    /// @since 4.0.0
    public partial class SbBlockedUserListQuery
    {
        /// <summary>
        /// User IDs filter. User list containing the passed User IDs will be returned.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> UserIdsFilter => _userIdsFilter;

        /// <summary>
        /// The maximum number of Users per queried page.
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
        /// Gets the list of Users. The queried result is passed to handler as List. If this method is repeatedly called after each next is finished, it retrieves the following pages of the User list. If there is no more pages to be read, an empty List is returned to handler.
        /// </summary>
        /// @since 4.0.0
        public void LoadNextPage(SbUserListHandler inCompletionHandler)
        {
            LoadNextPageInternal(inCompletionHandler);
        }
    }
}