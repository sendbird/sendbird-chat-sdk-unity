// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbGroupChannelContext
    {
        internal static readonly SbGroupChannelContext CHANNEL_CHANGELOG = new SbGroupChannelContext(SbCollectionEventSource.ChannelChangelog);
        internal static readonly SbGroupChannelContext EVENT_CHANNEL_CHANGED = new SbGroupChannelContext(SbCollectionEventSource.EventChannelChanged);
        internal static readonly SbGroupChannelContext EVENT_CHANNEL_DELETED = new SbGroupChannelContext(SbCollectionEventSource.EventChannelDeleted);
        internal static readonly SbGroupChannelContext EVENT_CHANNEL_MEMBER_COUNT_CHANGED = new SbGroupChannelContext(SbCollectionEventSource.EventChannelMemberCountChanged);
        internal static readonly SbGroupChannelContext EVENT_CHANNEL_FROZEN = new SbGroupChannelContext(SbCollectionEventSource.EventChannelFrozen);
        internal static readonly SbGroupChannelContext EVENT_CHANNEL_UNFROZEN = new SbGroupChannelContext(SbCollectionEventSource.EventChannelUnfrozen);
        internal static readonly SbGroupChannelContext EVENT_CHANNEL_HIDDEN = new SbGroupChannelContext(SbCollectionEventSource.EventChannelHidden);
        internal static readonly SbGroupChannelContext EVENT_OPERATOR_UPDATED = new SbGroupChannelContext(SbCollectionEventSource.EventOperatorUpdated);
        internal static readonly SbGroupChannelContext EVENT_TYPING_STATUS_UPDATED = new SbGroupChannelContext(SbCollectionEventSource.EventTypingStatusUpdated);
        internal static readonly SbGroupChannelContext EVENT_DELIVERY_STATUS_UPDATED = new SbGroupChannelContext(SbCollectionEventSource.EventDeliveryStatusUpdated);
        internal static readonly SbGroupChannelContext EVENT_READ_STATUS_UPDATED = new SbGroupChannelContext(SbCollectionEventSource.EventReadStatusUpdated);
        internal static readonly SbGroupChannelContext EVENT_USER_RECEIVED_INVITATION = new SbGroupChannelContext(SbCollectionEventSource.EventUserReceivedInvitation);
        internal static readonly SbGroupChannelContext EVENT_USER_DECLINED_INVITATION = new SbGroupChannelContext(SbCollectionEventSource.EventUserDeclinedInvitation);
        internal static readonly SbGroupChannelContext EVENT_USER_JOINED = new SbGroupChannelContext(SbCollectionEventSource.EventUserJoined);
        internal static readonly SbGroupChannelContext EVENT_USER_MUTED = new SbGroupChannelContext(SbCollectionEventSource.EventUserMuted);
        internal static readonly SbGroupChannelContext EVENT_USER_UNMUTED = new SbGroupChannelContext(SbCollectionEventSource.EventUserUnmuted);
        internal static readonly SbGroupChannelContext EVENT_USER_LEFT = new SbGroupChannelContext(SbCollectionEventSource.EventUserLeft);
        internal static readonly SbGroupChannelContext EVENT_USER_BANNED = new SbGroupChannelContext(SbCollectionEventSource.EventUserBanned);
        internal static readonly SbGroupChannelContext EVENT_USER_UNBANNED = new SbGroupChannelContext(SbCollectionEventSource.EventUserUnbanned);
        internal static readonly SbGroupChannelContext EVENT_META_DATA_CREATED = new SbGroupChannelContext(SbCollectionEventSource.EventMetaDataCreated);
        internal static readonly SbGroupChannelContext EVENT_META_DATA_UPDATED = new SbGroupChannelContext(SbCollectionEventSource.EventMetaDataUpdated);
        internal static readonly SbGroupChannelContext EVENT_META_DATA_DELETED = new SbGroupChannelContext(SbCollectionEventSource.EventMetaDataDeleted);
        internal static readonly SbGroupChannelContext EVENT_META_COUNTER_CREATED = new SbGroupChannelContext(SbCollectionEventSource.EventMetaCounterCreated);
        internal static readonly SbGroupChannelContext EVENT_META_COUNTER_UPDATED = new SbGroupChannelContext(SbCollectionEventSource.EventMetaCounterUpdated);
        internal static readonly SbGroupChannelContext EVENT_META_COUNTER_DELETED = new SbGroupChannelContext(SbCollectionEventSource.EventMetaCounterDeleted);
        internal static readonly SbGroupChannelContext EVENT_PINNED_MESSAGE_UPDATED = new SbGroupChannelContext(SbCollectionEventSource.EventPinnedMessageUpdated);

        private readonly SbCollectionEventSource _collectionEventSource;

        internal SbGroupChannelContext(SbCollectionEventSource inCollectionEventSource)
        {
            _collectionEventSource = inCollectionEventSource;
        }

        private bool IsFromEventInternal()
        {
            return _collectionEventSource.IsFromEvent();
        }
    }
}