// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// GroupChannel handler. This handler provides callbacks for events related GroupChannel.
    /// </summary>
    /// @since 4.0.0
    public class SbGroupChannelHandler : SbBaseChannelHandler
    {
        /// @since 4.0.0
        public delegate void ReadStatusUpdatedDelegate(SbGroupChannel inGroupChannel);

        /// @since 4.0.0
        public delegate void DeliveryStatusUpdatedDelegate(SbGroupChannel inGroupChannel);

        /// @since 4.0.0
        public delegate void TypingStatusUpdatedDelegate(SbGroupChannel inGroupChannel);

        /// @since 4.0.0
        public delegate void UserReceivedInvitationDelegate(SbGroupChannel inGroupChannel, SbUser inInviter, IReadOnlyList<SbUser> inInvitees);

        /// @since 4.0.0
        public delegate void UserJoinedDelegate(SbGroupChannel inGroupChannel, SbUser inUser);

        /// @since 4.0.0
        public delegate void UserDeclinedInvitationDelegate(SbGroupChannel inGroupChannel, SbUser inInviter, SbUser inInvitee);

        /// @since 4.0.0
        public delegate void UserLeftDelegate(SbGroupChannel inGroupChannel, SbUser inUser);

        /// @since 4.0.0
        public delegate void ChannelHiddenDelegate(SbGroupChannel inGroupChannel);

        /// @since 4.0.0
        public delegate void ChannelMemberCountChangedDelegate(IReadOnlyList<SbGroupChannel> inGroupChannels);

        /// @since 4.0.0
        public delegate void PinnedMessageUpdatedDelegate(SbGroupChannel inGroupChannel);

        /// <summary>
        /// A callback for when read receipts are updated on SbGroupChannel.
        /// </summary>
        /// @since 4.0.0
        public ReadStatusUpdatedDelegate OnReadStatusUpdated { get; set; }

        /// <summary>
        /// A callback for when delivered receipts are updated on SbGroupChannel.
        /// </summary>
        /// @since 4.0.0
        public DeliveryStatusUpdatedDelegate OnDeliveryStatusUpdated { get; set; }

        /// <summary>
        /// A callback for when Users send typing status for SbGroupChannel.
        /// </summary>
        /// @since 4.0.0
        public TypingStatusUpdatedDelegate OnTypingStatusUpdated { get; set; }

        /// <summary>
        /// A callback for when a new member has been invited to GroupChannel.
        /// </summary>
        /// @since 4.0.0
        public UserReceivedInvitationDelegate OnUserReceivedInvitation { get; set; }

        /// <summary>
        /// A callback for when a new member has joined GroupChannel.
        /// </summary>
        /// @since 4.0.0
        public UserJoinedDelegate OnUserJoined { get; set; }

        /// <summary>
        /// A callback for when the newly invited member has declined the invitation for the GroupChannel.
        /// </summary>
        /// @since 4.0.0
        public UserDeclinedInvitationDelegate OnUserDeclinedInvitation { get; set; }

        /// <summary>
        /// A callback for when an existing member has left GroupChannel. 
        /// </summary>
        /// @since 4.0.0
        public UserLeftDelegate OnUserLeft { get; set; }

        /// <summary>
        /// A callback for when GroupChannel is hidden.
        /// </summary>
        /// @since 4.0.0
        public ChannelHiddenDelegate OnChannelHidden { get; set; }

        /// <summary>
        /// Called when one or more broadcast channel's member counts are changed.
        /// </summary>
        /// @since 4.0.0
        public ChannelMemberCountChangedDelegate OnChannelMemberCountChanged { get; set; }

        /// <summary>
        /// A callback for when pinned message is changed.
        /// </summary>
        /// @since 4.0.0
        public PinnedMessageUpdatedDelegate OnPinnedMessageUpdated { get; set; }
    }
}