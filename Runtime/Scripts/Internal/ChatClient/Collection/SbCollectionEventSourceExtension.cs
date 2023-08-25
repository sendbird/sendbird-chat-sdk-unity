// 
//  Copyright (c) 2023 Sendbird, Inc.
// 


namespace Sendbird.Chat
{
    internal static class SbCollectionEventSourceExtension
    {
        internal static bool IsFromEvent(this SbCollectionEventSource inCollectionEventSource)
        {
            if (inCollectionEventSource == SbCollectionEventSource.MessageChangelog || inCollectionEventSource == SbCollectionEventSource.MessageFill ||
                inCollectionEventSource == SbCollectionEventSource.ChannelChangelog || inCollectionEventSource == SbCollectionEventSource.ChannelRefresh ||
                inCollectionEventSource == SbCollectionEventSource.PollChangeLog || inCollectionEventSource == SbCollectionEventSource.LocalMessageCanceled ||
                inCollectionEventSource == SbCollectionEventSource.LocalMessageFailed || inCollectionEventSource == SbCollectionEventSource.LocalMessagePendingCreated ||
                inCollectionEventSource == SbCollectionEventSource.LocalMessageResendStarted)
                return false;

            return true;
        }

        internal static SbGroupChannelContext ToCachedGroupChannelContext(this SbCollectionEventSource inCollectionEventSource)
        {
            switch (inCollectionEventSource)
            {
                case SbCollectionEventSource.ChannelChangelog:               return SbGroupChannelContext.CHANNEL_CHANGELOG;
                case SbCollectionEventSource.EventChannelChanged:            return SbGroupChannelContext.EVENT_CHANNEL_CHANGED;
                case SbCollectionEventSource.EventChannelDeleted:            return SbGroupChannelContext.EVENT_CHANNEL_DELETED;
                case SbCollectionEventSource.EventChannelMemberCountChanged: return SbGroupChannelContext.EVENT_CHANNEL_MEMBER_COUNT_CHANGED;
                case SbCollectionEventSource.EventChannelFrozen:             return SbGroupChannelContext.EVENT_CHANNEL_FROZEN;
                case SbCollectionEventSource.EventChannelUnfrozen:           return SbGroupChannelContext.EVENT_CHANNEL_UNFROZEN;
                case SbCollectionEventSource.EventChannelHidden:             return SbGroupChannelContext.EVENT_CHANNEL_HIDDEN;
                case SbCollectionEventSource.EventOperatorUpdated:           return SbGroupChannelContext.EVENT_OPERATOR_UPDATED;
                case SbCollectionEventSource.EventTypingStatusUpdated:       return SbGroupChannelContext.EVENT_TYPING_STATUS_UPDATED;
                case SbCollectionEventSource.EventDeliveryStatusUpdated:     return SbGroupChannelContext.EVENT_DELIVERY_STATUS_UPDATED;
                case SbCollectionEventSource.EventReadStatusUpdated:         return SbGroupChannelContext.EVENT_READ_STATUS_UPDATED;
                case SbCollectionEventSource.EventUserReceivedInvitation:    return SbGroupChannelContext.EVENT_USER_RECEIVED_INVITATION;
                case SbCollectionEventSource.EventUserDeclinedInvitation:    return SbGroupChannelContext.EVENT_USER_DECLINED_INVITATION;
                case SbCollectionEventSource.EventUserJoined:                return SbGroupChannelContext.EVENT_USER_JOINED;
                case SbCollectionEventSource.EventUserMuted:                 return SbGroupChannelContext.EVENT_USER_MUTED;
                case SbCollectionEventSource.EventUserUnmuted:               return SbGroupChannelContext.EVENT_USER_UNMUTED;
                case SbCollectionEventSource.EventUserLeft:                  return SbGroupChannelContext.EVENT_USER_LEFT;
                case SbCollectionEventSource.EventUserBanned:                return SbGroupChannelContext.EVENT_USER_BANNED;
                case SbCollectionEventSource.EventUserUnbanned:              return SbGroupChannelContext.EVENT_USER_UNBANNED;
                case SbCollectionEventSource.EventMetaDataCreated:           return SbGroupChannelContext.EVENT_META_DATA_CREATED;
                case SbCollectionEventSource.EventMetaDataUpdated:           return SbGroupChannelContext.EVENT_META_DATA_UPDATED;
                case SbCollectionEventSource.EventMetaDataDeleted:           return SbGroupChannelContext.EVENT_META_DATA_DELETED;
                case SbCollectionEventSource.EventMetaCounterCreated:        return SbGroupChannelContext.EVENT_META_COUNTER_CREATED;
                case SbCollectionEventSource.EventMetaCounterUpdated:        return SbGroupChannelContext.EVENT_META_COUNTER_UPDATED;
                case SbCollectionEventSource.EventMetaCounterDeleted:        return SbGroupChannelContext.EVENT_META_COUNTER_DELETED;
                case SbCollectionEventSource.EventPinnedMessageUpdated:      return SbGroupChannelContext.EVENT_PINNED_MESSAGE_UPDATED;
                default:                                                     return new SbGroupChannelContext(inCollectionEventSource);
            }
        }

        internal static SbMessageContext ToCachedMessageContext(this SbCollectionEventSource inCollectionEventSource, SbSendingStatus inSendingStatus = SbSendingStatus.None)
        {
            SbMessageContext resultMessageContext = CollectionEventSourceToMessageContext(inCollectionEventSource);
            resultMessageContext.SetSendingStatus(inSendingStatus);
            return resultMessageContext;
        }

        private static SbMessageContext CollectionEventSourceToMessageContext(SbCollectionEventSource inCollectionEventSource)
        {
            switch (inCollectionEventSource)
            {
                case SbCollectionEventSource.ChannelChangelog:               return SbMessageContext.CHANNEL_CHANGELOG;
                case SbCollectionEventSource.ChannelRefresh:                 return SbMessageContext.CHANNEL_REFRESH;
                case SbCollectionEventSource.EventChannelChanged:            return SbMessageContext.EVENT_CHANNEL_CHANGED;
                case SbCollectionEventSource.EventChannelDeleted:            return SbMessageContext.EVENT_CHANNEL_DELETED;
                case SbCollectionEventSource.EventChannelFrozen:             return SbMessageContext.EVENT_CHANNEL_FROZEN;
                case SbCollectionEventSource.EventChannelHidden:             return SbMessageContext.EVENT_CHANNEL_HIDDEN;
                case SbCollectionEventSource.EventChannelMemberCountChanged: return SbMessageContext.EVENT_CHANNEL_MEMBER_COUNT_CHANGED;
                case SbCollectionEventSource.EventChannelUnfrozen:           return SbMessageContext.EVENT_CHANNEL_UNFROZEN;
                case SbCollectionEventSource.EventDeliveryStatusUpdated:     return SbMessageContext.EVENT_DELIVERY_STATUS_UPDATED;
                case SbCollectionEventSource.EventMention:                   return SbMessageContext.EVENT_MENTION;
                case SbCollectionEventSource.EventMessageDeleted:            return SbMessageContext.EVENT_MESSAGE_DELETED;
                case SbCollectionEventSource.EventMessageReceived:           return SbMessageContext.EVENT_MESSAGE_RECEIVED;
                case SbCollectionEventSource.EventMessageSent:               return SbMessageContext.EVENT_MESSAGE_SENT;
                case SbCollectionEventSource.EventMessageUpdated:            return SbMessageContext.EVENT_MESSAGE_UPDATED;
                case SbCollectionEventSource.EventMetaCounterCreated:        return SbMessageContext.EVENT_META_COUNTER_CREATED;
                case SbCollectionEventSource.EventMetaCounterDeleted:        return SbMessageContext.EVENT_META_COUNTER_DELETED;
                case SbCollectionEventSource.EventMetaCounterUpdated:        return SbMessageContext.EVENT_META_COUNTER_UPDATED;
                case SbCollectionEventSource.EventMetaDataCreated:           return SbMessageContext.EVENT_META_DATA_CREATED;
                case SbCollectionEventSource.EventMetaDataDeleted:           return SbMessageContext.EVENT_META_DATA_DELETED;
                case SbCollectionEventSource.EventMetaDataUpdated:           return SbMessageContext.EVENT_META_DATA_UPDATED;
                case SbCollectionEventSource.EventOperatorUpdated:           return SbMessageContext.EVENT_OPERATOR_UPDATED;
                case SbCollectionEventSource.EventPinnedMessageUpdated:      return SbMessageContext.EVENT_PINNED_MESSAGE_UPDATED;
                case SbCollectionEventSource.EventReactionUpdated:           return SbMessageContext.EVENT_REACTION_UPDATED;
                case SbCollectionEventSource.EventReadStatusUpdated:         return SbMessageContext.EVENT_READ_STATUS_UPDATED;
                case SbCollectionEventSource.EventThreadInfoUpdated:         return SbMessageContext.EVENT_THREAD_INFO_UPDATED;
                case SbCollectionEventSource.EventTypingStatusUpdated:       return SbMessageContext.EVENT_TYPING_STATUS_UPDATED;
                case SbCollectionEventSource.EventUserBanned:                return SbMessageContext.EVENT_USER_BANNED;
                case SbCollectionEventSource.EventUserDeclinedInvitation:    return SbMessageContext.EVENT_USER_DECLINED_INVITATION;
                case SbCollectionEventSource.EventUserJoined:                return SbMessageContext.EVENT_USER_JOINED;
                case SbCollectionEventSource.EventUserLeft:                  return SbMessageContext.EVENT_USER_LEFT;
                case SbCollectionEventSource.EventUserMuted:                 return SbMessageContext.EVENT_USER_MUTED;
                case SbCollectionEventSource.EventUserReceivedInvitation:    return SbMessageContext.EVENT_USER_RECEIVED_INVITATION;
                case SbCollectionEventSource.EventUserUnbanned:              return SbMessageContext.EVENT_USER_UNBANNED;
                case SbCollectionEventSource.EventUserUnmuted:               return SbMessageContext.EVENT_USER_UNMUTED;
                case SbCollectionEventSource.LocalMessageCanceled:           return SbMessageContext.LOCAL_MESSAGE_CANCELED;
                case SbCollectionEventSource.LocalMessageFailed:             return SbMessageContext.LOCAL_MESSAGE_FAILED;
                case SbCollectionEventSource.LocalMessagePendingCreated:     return SbMessageContext.LOCAL_MESSAGE_PENDING_CREATED;
                case SbCollectionEventSource.LocalMessageResendStarted:      return SbMessageContext.LOCAL_MESSAGE_RESEND_STARTED;
                case SbCollectionEventSource.MessageChangelog:               return SbMessageContext.MESSAGE_CHANGELOG;
                case SbCollectionEventSource.MessageFill:                    return SbMessageContext.MESSAGE_FILL;
                case SbCollectionEventSource.PollChangeLog:                  return SbMessageContext.POLL_CHANGE_LOG;
                default:                                                     return new SbMessageContext(inCollectionEventSource);
            }
        }
    }
}