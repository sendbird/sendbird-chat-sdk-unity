// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The unread item key.
    /// </summary>
    /// @since 4.0.0
    public enum SbUnreadItemKey
    {
        /// <summary>
        /// The key for invitation count of group channel(super and non-super).
        /// </summary>
        /// @since 4.0.0
        [JsonName("group_channel_invitation_count")] GroupChannelInvitationCount,

        /// <summary>
        /// The key for unread mention count of group channel(super and non-super).
        /// </summary>
        /// @since 4.0.0
        [JsonName("group_channel_unread_mention_count")] GroupChannelUnreadMentionCount,

        /// <summary>
        /// The key for unread message count of group channel(super and non-super).
        /// </summary>
        /// @since 4.0.0
        [JsonName("group_channel_unread_message_count")] GroupChannelUnreadMessageCount,

        /// <summary>
        /// The key for invitation count of non super channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("non_super_group_channel_invitation_count")] NonSuperGroupChannelInvitationCount,

        /// <summary>
        /// The key for unread mention count of non super channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("non_super_group_channel_unread_mention_count")] NonSuperGroupChannelUnreadMentionCount,

        /// <summary>
        /// The key for unread message count of non super channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("non_super_group_channel_unread_message_count")] NonSuperGroupChannelUnreadMessageCount,

        /// <summary>
        /// The key for invitation count of super channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("super_group_channel_invitation_count")] SuperGroupChannelInvitationCount,

        /// <summary>
        /// The key for unread mention count of non super channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("super_group_channel_unread_mention_count")] SuperGroupChannelUnreadMentionCount,

        /// <summary>
        /// The key for unread message count of super channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("super_group_channel_unread_message_count")] SuperGroupChannelUnreadMessageCount
    }
}