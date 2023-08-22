// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// A class representing query to retrieve lists related to banned User.
    /// </summary>
    /// @since 4.0.0
    public partial class SbBannedUserListQuery
    {
        /// <summary>
        /// The maximum number of items per queried page.
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
        /// Gets the list of RestrictedUsers. The queried result is passed to handler as List. If this method is repeatedly called after each next is finished, it retrieves the following pages of the SbRestrictedUser list.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void LoadNextPage(SbUserListHandler inCompletionHandler)
        {
            LoadNextPageInternal(inCompletionHandler);
        }
    }
}