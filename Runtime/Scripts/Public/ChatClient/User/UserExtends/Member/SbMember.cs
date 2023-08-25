// 
//  Copyright (c) 2022 Sendbird, Inc.
// 


namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a GroupChannel member.
    /// </summary>
    /// @since 4.0.0
    public partial class SbMember : SbUser
    {
        /// <summary>
        /// Whether the Member is blocked by the SendbirdChat.CurrentUser.
        /// </summary>
        /// @since 4.0.0
        public bool IsBlockedByMe { get => _isBlockedByMe; internal set => _isBlockedByMe = value; }

        /// <summary>
        /// Whether the Member is blocking the SendbirdChat.currentUser.
        /// </summary>
        /// @since 4.0.0
        public bool IsBlockingMe => _isBlockingMe;

        /// <summary>
        /// Whether the Member is muted or not.
        /// </summary>
        /// @since 4.0.0
        public bool IsMuted { get => _isMuted; internal set => _isMuted = value; }

        /// <summary>
        /// The role of this member in the channel.
        /// </summary>
        /// @since 4.0.0
        public SbRole Role { get => _role; internal set => _role = value; }

        /// <summary>
        /// The Member's invitation state.
        /// </summary>
        /// @since 4.0.0
        public SbMemberState MemberState { get => _memberState; internal set => _memberState = value; }

        /// <summary>
        /// Restriction information for the current member. Only Nonnull if the member is muted.
        /// </summary>
        /// @since 4.0.0
        public SbRestrictionInfo RestrictionInfo { get => _restrictionInfo; internal set => _restrictionInfo = value; }
    }
}