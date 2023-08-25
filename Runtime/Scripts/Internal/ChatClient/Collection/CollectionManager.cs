// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal class CollectionManager
    {
        private readonly SendbirdChatMainContext _chatMainContextRef;
        private readonly List<SbBaseCollection> _collectionList = new List<SbBaseCollection>();

        internal CollectionManager(SendbirdChatMainContext inChatMainContext)
        {
            _chatMainContextRef = inChatMainContext;
        }

        internal SbGroupChannelCollection CreateGroupChannelCollection(SbGroupChannelCollectionCreateParams inCreateParams)
        {
            if (inCreateParams == null)
            {
                Logger.Warning(Logger.CategoryType.Collection, "CreateGroupChannelCollection() GroupChannelCollectionCreateParams is null.");
                return null;
            }

            SbGroupChannelCollection channelCollection = new SbGroupChannelCollection(inCreateParams, _chatMainContextRef);
            _collectionList.Add(channelCollection);
            return channelCollection;
        }

        internal SbMessageCollection CreateMessageCollection(SbGroupChannel inGroupChannel, SbMessageCollectionCreateParams inCreateParams)
        {
            if (inCreateParams == null)
            {
                Logger.Warning(Logger.CategoryType.Collection, "CreateMessageCollection() MessageCollectionCreateParams is null.");
                return null;
            }

            SbMessageCollection messageCollection = new SbMessageCollection(inGroupChannel, inCreateParams, _chatMainContextRef);
            _collectionList.Add(messageCollection);
            return messageCollection;
        }
        
        internal void RemoveCollection(SbBaseCollection inBaseCollection)
        {
            if (inBaseCollection == null)
                return;

            _collectionList.Remove(inBaseCollection);
        }

        internal void OnChangeConnectionState(ConnectionStateInternalType inChangedStateType)
        {
            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChangeConnectionState(inChangedStateType));
        }

        internal void OnChannelChanged(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventChannelChanged, inBaseChannel));
        }

        internal void OnChannelDeleted(string inChannelUrl, SbChannelType inChannelType)
        {
            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelDeleted(inChannelUrl, inChannelType));
        }

        internal void OnOperatorUpdated(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventOperatorUpdated, inBaseChannel));
        }

        internal void OnChannelFrozen(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventChannelFrozen, inBaseChannel));
        }

        internal void OnChannelUnfrozen(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventChannelUnfrozen, inBaseChannel));
        }

        internal void OnChannelHidden(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventChannelHidden, inBaseChannel));
        }

        internal void OnMetaDataCreated(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventMetaDataCreated, inBaseChannel));
        }

        internal void OnMetaDataUpdated(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventMetaDataUpdated, inBaseChannel));
        }

        internal void OnMetaDataDeleted(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventMetaDataDeleted, inBaseChannel));
        }

        internal void OnMetaCountersCreated(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventMetaCounterCreated, inBaseChannel));
        }

        internal void OnMetaCountersUpdated(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventMetaCounterUpdated, inBaseChannel));
        }

        internal void OnMetaCountersDeleted(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventMetaCounterDeleted, inBaseChannel));
        }

        internal void OnUserJoined(SbBaseChannel inBaseChannel, SbUser inUser)
        {
            if (inBaseChannel == null || inUser == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventUserJoined, inBaseChannel));
        }

        internal void OnUserReceivedInvitation(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventUserReceivedInvitation, inBaseChannel));
        }

        internal void OnUserDeclinedInvitation(SbBaseChannel inBaseChannel, SbUser inUser)
        {
            if (inBaseChannel == null || inUser == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnLeaveChannel(SbCollectionEventSource.EventUserDeclinedInvitation, inBaseChannel, inUser));
        }

        internal void OnUserLeftChannel(SbBaseChannel inBaseChannel, SbUser inUser)
        {
            if (inBaseChannel == null || inUser == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnLeaveChannel(SbCollectionEventSource.EventUserLeft, inBaseChannel, inUser));
        }

        internal void OnUserMuted(SbBaseChannel inBaseChannel, SbRestrictedUser inRestrictedUser)
        {
            if (inBaseChannel == null || inRestrictedUser == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnUserMuted(inBaseChannel, inRestrictedUser));
        }

        internal void OnUserUnmuted(SbBaseChannel inBaseChannel, SbUser inUser)
        {
            if (inBaseChannel == null || inUser == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnUserUnmuted(inBaseChannel, inUser));
        }

        internal void OnUserBanned(SbBaseChannel inBaseChannel, SbUser inUser)
        {
            if (inBaseChannel == null || inUser == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnLeaveChannel(SbCollectionEventSource.EventUserBanned, inBaseChannel, inUser));
        }

        internal void OnUserUnbanned(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventUserUnbanned, inBaseChannel));
        }

        internal void OnTypingStatusUpdated(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventTypingStatusUpdated, inBaseChannel));
        }

        internal void OnMentionReceived(SbBaseChannel inBaseChannel)
        {
            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventMention, inBaseChannel));
        }

        internal void OnPinnedMessageUpdated(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventPinnedMessageUpdated, inBaseChannel));
        }

        internal void OnReadStatusUpdated(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventReadStatusUpdated, inBaseChannel));
        }

        internal void OnDeliveryStatusUpdated(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelUpdated(SbCollectionEventSource.EventDeliveryStatusUpdated, inBaseChannel));
        }

        internal void OnChannelMemberCountChanged(List<SbGroupChannel> inGroupChannels)
        {
            if (inGroupChannels == null || inGroupChannels.Count <= 0)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnChannelMemberCountChanged(inGroupChannels));
        }

        internal void OnMessageReceived(SbBaseChannel inBaseChannel, SbBaseMessage inMessage)
        {
            if (inBaseChannel == null || inMessage == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnMessageReceived(inBaseChannel, inMessage));
        }

        internal void OnMessageDeleted(SbBaseChannel inBaseChannel, long inMessageId)
        {
            if (inBaseChannel == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnMessageDeleted(inBaseChannel, inMessageId));
        }

        internal void OnMessageUpdated(SbBaseChannel inBaseChannel, SbBaseMessage inMessage)
        {
            if (inBaseChannel == null || inMessage == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnMessageUpdated(inBaseChannel, new List<SbBaseMessage> { inMessage }));
        }

        internal void OnMessageSent(SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage)
        {
            if (inBaseChannel == null || inBaseMessage == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnMessageSent(inBaseChannel, inBaseMessage));
        }

        internal void OnMessagePending(SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage)
        {
            if (inBaseChannel == null || inBaseMessage == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnMessagePending(inBaseChannel, inBaseMessage));
        }

        internal void OnMessageFailed(SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage)
        {
            if (inBaseChannel == null || inBaseMessage == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnMessageFailed(SbCollectionEventSource.LocalMessageFailed, inBaseChannel, inBaseMessage));
        }

        internal void OnMessageCanceled(SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage)
        {
            if (inBaseChannel == null || inBaseMessage == null)
                return;

            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnMessageFailed(SbCollectionEventSource.LocalMessageCanceled, inBaseChannel, inBaseMessage));
        }

        internal void OnReactionUpdated(SbBaseChannel inBaseChannel, SbReactionEvent inReactionEvent)
        {
            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnReactionUpdated(inBaseChannel, inReactionEvent));
        }

        internal void OnThreadInfoUpdated(SbBaseChannel inBaseChannel, SbThreadInfoUpdateEvent inThreadInfoUpdateEvent)
        {
            _collectionList.ForEach(inBaseCollection => inBaseCollection.OnThreadInfoUpdated(inBaseChannel, inThreadInfoUpdateEvent));
        }
    }
}