// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Member state in group channel.
    /// </summary>
    /// @since 4.0.0
    public enum SbMemberState
    {
        /// <summary>
        /// Filter of joined members in a group channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("joined")] Joined,

        /// <summary>
        /// Filter of invited members in a group channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("invited")] Invited,

        /// <summary>
        /// Filter of members neither joined or invited in a group channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("none")] None,

        /// <summary>
        /// Filter of members who have left from a group channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("left")] Left
    }
}