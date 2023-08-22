// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Params for creating a SbMemberListQuery object.
    /// </summary>
    /// @since 4.0.0
    public class SbMemberListQueryParams
    {
        /// <summary>
        /// A filter to return members whose nicknames start with the specified string.
        /// </summary>
        /// @since 4.0.0
        public string NicknameStartsWithFilter { get; set; } = null;

        /// <summary>
        /// Operator filter.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelOperatorFilter OperatorFilter { get; set; } = SbGroupChannelOperatorFilter.All;

        /// <summary>
        /// Muted member filter.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelMutedMemberFilter MutedMemberFilter { get; set; } = SbGroupChannelMutedMemberFilter.All;

        /// <summary>
        /// A filter to return members with the member state matching to SbMemberStateFilter.
        /// </summary>
        /// @since 4.0.0
        public SbMemberStateFilter MemberStateFilter { get; set; } = SbMemberStateFilter.All;

        /// <summary>
        /// Indicates how the query result should be ordered.
        /// </summary>
        /// @since 4.0.0
        public SbMemberListOrder Order { get; set; } = SbMemberListOrder.NicknameAlphabetical;

        /// <summary>
        /// The maximum number of items per queried page.
        /// </summary>
        /// @since 4.0.0
        public int Limit { get; set; } = SendbirdChatMainContext.QUERY_DEFAULT_LIMIT;
    }
}