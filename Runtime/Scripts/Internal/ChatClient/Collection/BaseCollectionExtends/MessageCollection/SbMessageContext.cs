// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbMessageContext
    {
        internal static readonly SbMessageContext CHANNEL_CHANGELOG = new SbMessageContext(SbCollectionEventSource.ChannelChangelog);
        internal static readonly SbMessageContext CHANNEL_REFRESH = new SbMessageContext(SbCollectionEventSource.ChannelRefresh);
        internal static readonly SbMessageContext EVENT_CHANNEL_CHANGED = new SbMessageContext(SbCollectionEventSource.EventChannelChanged);
        internal static readonly SbMessageContext EVENT_CHANNEL_DELETED = new SbMessageContext(SbCollectionEventSource.EventChannelDeleted);
        internal static readonly SbMessageContext EVENT_CHANNEL_FROZEN = new SbMessageContext(SbCollectionEventSource.EventChannelFrozen);
        internal static readonly SbMessageContext EVENT_CHANNEL_HIDDEN = new SbMessageContext(SbCollectionEventSource.EventChannelHidden);
        internal static readonly SbMessageContext EVENT_CHANNEL_MEMBER_COUNT_CHANGED = new SbMessageContext(SbCollectionEventSource.EventChannelMemberCountChanged);
        internal static readonly SbMessageContext EVENT_CHANNEL_UNFROZEN = new SbMessageContext(SbCollectionEventSource.EventChannelUnfrozen);
        internal static readonly SbMessageContext EVENT_DELIVERY_STATUS_UPDATED = new SbMessageContext(SbCollectionEventSource.EventDeliveryStatusUpdated);
        internal static readonly SbMessageContext EVENT_MENTION = new SbMessageContext(SbCollectionEventSource.EventMention);
        internal static readonly SbMessageContext EVENT_MESSAGE_DELETED = new SbMessageContext(SbCollectionEventSource.EventMessageDeleted);
        internal static readonly SbMessageContext EVENT_MESSAGE_RECEIVED = new SbMessageContext(SbCollectionEventSource.EventMessageReceived);
        internal static readonly SbMessageContext EVENT_MESSAGE_SENT = new SbMessageContext(SbCollectionEventSource.EventMessageSent);
        internal static readonly SbMessageContext EVENT_MESSAGE_UPDATED = new SbMessageContext(SbCollectionEventSource.EventMessageUpdated);
        internal static readonly SbMessageContext EVENT_META_COUNTER_CREATED = new SbMessageContext(SbCollectionEventSource.EventMetaCounterCreated);
        internal static readonly SbMessageContext EVENT_META_COUNTER_DELETED = new SbMessageContext(SbCollectionEventSource.EventMetaCounterDeleted);
        internal static readonly SbMessageContext EVENT_META_COUNTER_UPDATED = new SbMessageContext(SbCollectionEventSource.EventMetaCounterUpdated);
        internal static readonly SbMessageContext EVENT_META_DATA_CREATED = new SbMessageContext(SbCollectionEventSource.EventMetaDataCreated);
        internal static readonly SbMessageContext EVENT_META_DATA_DELETED = new SbMessageContext(SbCollectionEventSource.EventMetaDataDeleted);
        internal static readonly SbMessageContext EVENT_META_DATA_UPDATED = new SbMessageContext(SbCollectionEventSource.EventMetaDataUpdated);
        internal static readonly SbMessageContext EVENT_OPERATOR_UPDATED = new SbMessageContext(SbCollectionEventSource.EventOperatorUpdated);
        internal static readonly SbMessageContext EVENT_PINNED_MESSAGE_UPDATED = new SbMessageContext(SbCollectionEventSource.EventPinnedMessageUpdated);
        internal static readonly SbMessageContext EVENT_REACTION_UPDATED = new SbMessageContext(SbCollectionEventSource.EventReactionUpdated);
        internal static readonly SbMessageContext EVENT_READ_STATUS_UPDATED = new SbMessageContext(SbCollectionEventSource.EventReadStatusUpdated);
        internal static readonly SbMessageContext EVENT_THREAD_INFO_UPDATED = new SbMessageContext(SbCollectionEventSource.EventThreadInfoUpdated);
        internal static readonly SbMessageContext EVENT_TYPING_STATUS_UPDATED = new SbMessageContext(SbCollectionEventSource.EventTypingStatusUpdated);
        internal static readonly SbMessageContext EVENT_USER_BANNED = new SbMessageContext(SbCollectionEventSource.EventUserBanned);
        internal static readonly SbMessageContext EVENT_USER_DECLINED_INVITATION = new SbMessageContext(SbCollectionEventSource.EventUserDeclinedInvitation);
        internal static readonly SbMessageContext EVENT_USER_JOINED = new SbMessageContext(SbCollectionEventSource.EventUserJoined);
        internal static readonly SbMessageContext EVENT_USER_LEFT = new SbMessageContext(SbCollectionEventSource.EventUserLeft);
        internal static readonly SbMessageContext EVENT_USER_MUTED = new SbMessageContext(SbCollectionEventSource.EventUserMuted);
        internal static readonly SbMessageContext EVENT_USER_RECEIVED_INVITATION = new SbMessageContext(SbCollectionEventSource.EventUserReceivedInvitation);
        internal static readonly SbMessageContext EVENT_USER_UNBANNED = new SbMessageContext(SbCollectionEventSource.EventUserUnbanned);
        internal static readonly SbMessageContext EVENT_USER_UNMUTED = new SbMessageContext(SbCollectionEventSource.EventUserUnmuted);
        internal static readonly SbMessageContext LOCAL_MESSAGE_CANCELED = new SbMessageContext(SbCollectionEventSource.LocalMessageCanceled);
        internal static readonly SbMessageContext LOCAL_MESSAGE_FAILED = new SbMessageContext(SbCollectionEventSource.LocalMessageFailed);
        internal static readonly SbMessageContext LOCAL_MESSAGE_PENDING_CREATED = new SbMessageContext(SbCollectionEventSource.LocalMessagePendingCreated);
        internal static readonly SbMessageContext LOCAL_MESSAGE_RESEND_STARTED = new SbMessageContext(SbCollectionEventSource.LocalMessageResendStarted);
        internal static readonly SbMessageContext MESSAGE_CHANGELOG = new SbMessageContext(SbCollectionEventSource.MessageChangelog);
        internal static readonly SbMessageContext MESSAGE_FILL = new SbMessageContext(SbCollectionEventSource.MessageFill);
        internal static readonly SbMessageContext POLL_CHANGE_LOG = new SbMessageContext(SbCollectionEventSource.PollChangeLog);

        private readonly SbCollectionEventSource _collectionEventSource;
        private SbSendingStatus _messagesSendingStatus;

        internal SbMessageContext(SbCollectionEventSource inCollectionEventSource, SbSendingStatus inSendingStatus = SbSendingStatus.None)
        {
            _collectionEventSource = inCollectionEventSource;
            _messagesSendingStatus = inSendingStatus;
        }

        internal void SetSendingStatus(SbSendingStatus inSendingStatus)
        {
            _messagesSendingStatus = inSendingStatus;
        }

        private bool IsFromEventInternal()
        {
            return _collectionEventSource.IsFromEvent();
        }
    }
}