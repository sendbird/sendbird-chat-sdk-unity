// 
//  Copyright (c) 2023 Sendbird, Inc.
// 


namespace Sendbird.Chat
{
    /// <summary>
    /// Channel sources that represents where the channel object comes from.
    /// </summary>
    /// @since 4.0.0
    public enum SbCollectionEventSource
    {
        /// @since 4.0.0
        ChannelChangelog,

        /// @since 4.0.0
        ChannelRefresh,

        /// @since 4.0.0
        EventChannelChanged,

        /// @since 4.0.0
        EventChannelDeleted,

        /// @since 4.0.0
        EventChannelFrozen,

        /// @since 4.0.0
        EventChannelHidden,

        /// @since 4.0.0
        EventChannelMemberCountChanged,

        /// @since 4.0.0
        EventChannelUnfrozen,

        /// @since 4.0.0
        EventDeliveryStatusUpdated,

        /// @since 4.0.0
        EventMention,

        /// @since 4.0.0
        EventMessageDeleted,

        /// @since 4.0.0
        EventMessageReceived,

        /// @since 4.0.0
        EventMessageSent,

        /// @since 4.0.0
        EventMessageUpdated,

        /// @since 4.0.0
        EventMetaCounterCreated,

        /// @since 4.0.0
        EventMetaCounterDeleted,

        /// @since 4.0.0
        EventMetaCounterUpdated,

        /// @since 4.0.0
        EventMetaDataCreated,

        /// @since 4.0.0
        EventMetaDataDeleted,

        /// @since 4.0.0
        EventMetaDataUpdated,

        /// @since 4.0.0
        EventOperatorUpdated,

        /// @since 4.0.0
        EventPinnedMessageUpdated,

        /// @since 4.0.0
        EventReactionUpdated,

        /// @since 4.0.0
        EventReadStatusUpdated,

        /// @since 4.0.0
        EventThreadInfoUpdated,

        /// @since 4.0.0
        EventTypingStatusUpdated,

        /// @since 4.0.0
        EventUserBanned,

        /// @since 4.0.0
        EventUserDeclinedInvitation,

        /// @since 4.0.0
        EventUserJoined,

        /// @since 4.0.0
        EventUserLeft,

        /// @since 4.0.0
        EventUserMuted,

        /// @since 4.0.0
        EventUserReceivedInvitation,

        /// @since 4.0.0
        EventUserUnbanned,

        /// @since 4.0.0
        EventUserUnmuted,

        /// @since 4.0.0
        LocalMessageCanceled,

        /// @since 4.0.0
        LocalMessageFailed,

        /// @since 4.0.0
        LocalMessagePendingCreated,

        /// @since 4.0.0
        LocalMessageResendStarted,

        /// @since 4.0.0
        MessageChangelog,

        /// @since 4.0.0
        MessageFill,

        /// @since 4.0.0
        PollChangeLog
    }
}