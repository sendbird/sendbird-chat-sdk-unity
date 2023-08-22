// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// A class representing query to retrieve lists related to User.
    /// </summary>
    /// @since 4.0.0
    public partial class SbApplicationUserListQuery
    {
        /// <summary>
        /// Sets User IDs filter. User list containing the passed User IDs will be returned.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> UserIdsFilter => _userIdsFilter;

        /// <summary>
        /// A filter to return users whose nicknames start with the specified string.
        /// </summary>
        /// @since 4.0.0
        public string NicknameStartsWithFilter => _nicknameStartsWithFilter;

        /// <summary>
        /// The meta data key filter. This query will return users that has the meta data key and values.
        /// </summary>
        /// @since 4.0.0
        public string MetaDataKeyFilter => _metaDataKeyFilter;

        /// <summary>
        /// The meta data values filter. This query will return users that has the meta data key and values.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> MetaDataValuesFilter => _metaDataValuesFilter;

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
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void LoadNextPage(SbUserListHandler inCompletionHandler)
        {
            LoadNextPageInternal(inCompletionHandler);
        }
    }
}