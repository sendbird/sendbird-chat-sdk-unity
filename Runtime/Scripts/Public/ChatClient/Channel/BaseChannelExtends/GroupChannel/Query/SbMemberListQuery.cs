// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// A class representing query to retrieve lists related to group channel member.
    /// </summary>
    /// @since 4.0.0
    public partial class SbMemberListQuery
    {
        /// <summary>
        /// A filter to return members whose nicknames start with the specified string.
        /// </summary>
        /// @since 4.0.0
        public string NicknameStartsWithFilter => _nicknameStartsWithFilter;

        /// <summary>
        /// Operator filter.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelOperatorFilter OperatorFilter => _operatorFilter;

        /// <summary>
        /// Muted member filter.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelMutedMemberFilter MutedMemberFilter => _mutedMemberFilter;

        /// <summary>
        /// A filter to return members with the member state matching to MemberStateFilter.
        /// </summary>
        /// @since 4.0.0
        public SbMemberStateFilter MemberStateFilter => _memberStateFilter;

        /// <summary>
        /// Indicates how the query result should be ordered.
        /// </summary>
        /// @since 4.0.0
        public SbMemberListOrder Order => _order;

        /// <summary>
        /// The maximum number of group channel members per queried page.
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
        /// Gets the list of group channel members. The queried result is passed to handler as List. If this method is repeatedly called after each LoadNextPage() is finished, it retrieves the following pages of the group channel members list.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void LoadNextPage(SbMemberListHandler inCompletionHandler)
        {
            LoadNextPageInternal(inCompletionHandler);
        }
    }
}