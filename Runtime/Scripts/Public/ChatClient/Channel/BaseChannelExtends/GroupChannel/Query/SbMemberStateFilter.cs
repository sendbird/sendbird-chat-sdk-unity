// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Member state filter for group channel list query and group channel count.
    /// </summary>
    /// @since 4.0.0
    public enum SbMemberStateFilter
    {
        /// <summary>
        /// Filter of all member states.
        /// </summary>
        /// @since 4.0.0
        [JsonName("all")] All,

        /// <summary>
        /// Filter of joined state only.
        /// </summary>
        /// @since 4.0.0
        [JsonName("joined_only")] JoinedOnly,

        /// <summary>
        /// Filter of invited state only. This contains InvitedByFriend InvitedByNonFriend.
        /// </summary>
        /// @since 4.0.0
        [JsonName("invited_only")] InvitedOnly,

        /// <summary>
        /// Filter of invited by friend state only.
        /// </summary>
        /// @since 4.0.0
        [JsonName("invited_by_friend")] InvitedByFriend,

        /// <summary>
        /// Filter of invited by non-friend state only.
        /// </summary>
        /// @since 4.0.0
        [JsonName("invited_by_non_friend")] InvitedByNonFriend
    }
}