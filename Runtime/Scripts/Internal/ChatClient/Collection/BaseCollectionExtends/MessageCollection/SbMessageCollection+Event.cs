// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Linq;

namespace Sendbird.Chat
{
    public partial class SbMessageCollection
    {
        internal override void OnChangeConnectionState(ConnectionStateInternalType inChangedStateType)
        {
            if (inChangedStateType != ConnectionStateInternalType.Connected || _stateType == StateType.Disposed || _groupChannel == null)
                return;

            void OnMuteInfoHandler(bool inIsMuted, string inDescription, long inStartAt, long inEndAt, long inRemainingDuration, SbError inError)
            {
                if (inError == null && inIsMuted && 0 < inRemainingDuration)
                {
                    _groupChannel.SetMyMutedState(SbMutedState.Muted);
                    StopAndStartAutoUnmuteCoroutine(inRemainingDuration);
                }
            }

            void OnChannelRefreshHandler(SbError inError)
            {
                if (inError != null || _groupChannel.MyMemberState == SbMemberState.None)
                {
                    InvokeOnChannelDeletedHandler(SbCollectionEventSource.ChannelChangelog.ToCachedMessageContext(), _groupChannel.Url);
                    return;
                }

                if (_groupChannel.MyMutedState == SbMutedState.Unmuted)
                {
                    StopAutoUnmuteCoroutineIfStarted();
                }
                else if (_groupChannel.MyMutedState == SbMutedState.Muted)
                {
                    _groupChannel.GetMyMutedInfo(OnMuteInfoHandler);
                }

                InvokeOnChannelUpdatedHandler(SbCollectionEventSource.ChannelChangelog.ToCachedMessageContext(), _groupChannel);

                RequestAllChangeLogs();
                CheckHugeGapAndFill();
            }

            _groupChannel.Refresh(OnChannelRefreshHandler);
        }


        internal override void OnChannelUpdated(SbCollectionEventSource inCollectionEventSource, SbBaseChannel inBaseChannel)
        {
            if (IsValidChannel(inBaseChannel) == false)
                return;

            SbGroupChannel groupChannel = inBaseChannel as SbGroupChannel;
            if (groupChannel == null)
                return;

            if (groupChannel.MessageOffsetTimestamp != _messageOffsetTimestamp && groupChannel.MessageOffsetTimestamp == SbGroupChannel.INVALID_MESSAGE_OFFSET_TIMESTAMP)
            {
                _messageOffsetTimestamp = groupChannel.MessageOffsetTimestamp;
                List<long> deleteUrls = _succeededMessages.Where(inMessage => inMessage.CreatedAt < _messageOffsetTimestamp).Select(inMessage => inMessage.MessageId).ToList();
                if (0 <= deleteUrls.Count)
                {
                    UpsertToSucceededMessagesList(SbCollectionEventSource.EventChannelChanged.ToCachedMessageContext(), inAddOrUpdateMessages: null, deleteUrls);
                }
            }

            InvokeOnChannelUpdatedHandler(inCollectionEventSource.ToCachedMessageContext(), groupChannel);
        }

        internal override void OnUserMuted(SbBaseChannel inBaseChannel, SbRestrictedUser inRestrictedUser)
        {
            if (IsValidChannel(inBaseChannel) == false || inRestrictedUser == null)
                return;

            if (inRestrictedUser.UserId == chatMainContextRef.CurrentUserId && 0 <= inRestrictedUser.RestrictionInfo?.RemainingDuration)
            {
                StopAndStartAutoUnmuteCoroutine(inRestrictedUser.RestrictionInfo.RemainingDuration);
            }

            OnChannelUpdated(SbCollectionEventSource.EventUserMuted, inBaseChannel);
        }

        internal override void OnUserUnmuted(SbBaseChannel inBaseChannel, SbUser inUser)
        {
            if (IsValidChannel(inBaseChannel) == false || inUser == null)
                return;

            if (inUser.UserId == chatMainContextRef.CurrentUserId)
            {
                StopAutoUnmuteCoroutineIfStarted();
            }

            OnChannelUpdated(SbCollectionEventSource.EventUserUnmuted, inBaseChannel);
        }

        internal override void OnChannelMemberCountChanged(List<SbGroupChannel> inGroupChannels)
        {
            if (inGroupChannels == null || inGroupChannels.Count <= 0)
                return;

            foreach (SbGroupChannel groupChannel in inGroupChannels)
            {
                if (IsValidChannel(groupChannel) == false)
                    continue;

                InvokeOnChannelUpdatedHandler(SbCollectionEventSource.EventChannelMemberCountChanged.ToCachedMessageContext(), groupChannel);
            }
        }

        internal override void OnLeaveChannel(SbCollectionEventSource inCollectionEventSource, SbBaseChannel inBaseChannel, SbUser inUser)
        {
            if (IsValidChannel(inBaseChannel) == false || inUser == null)
                return;

            if (inUser.UserId == chatMainContextRef.CurrentUserId)
            {
                InvokeOnChannelDeletedHandler(inCollectionEventSource.ToCachedMessageContext(), inBaseChannel?.Url);
            }
            else
            {
                InvokeOnChannelUpdatedHandler(inCollectionEventSource.ToCachedMessageContext(), inBaseChannel as SbGroupChannel);
            }
        }

        internal override void OnChannelDeleted(string inChannelUrl, SbChannelType inChannelType)
        {
            if (inChannelUrl != _groupChannel.Url)
                return;

            InvokeOnChannelDeletedHandler(SbCollectionEventSource.EventChannelDeleted.ToCachedMessageContext(), inChannelUrl);
        }

        internal override void OnMessageReceived(SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage)
        {
            if (IsValidChannel(inBaseChannel) == false || inBaseMessage == null)
                return;

            UpsertToSucceededMessagesList(SbCollectionEventSource.EventMessageReceived.ToCachedMessageContext(), new List<SbBaseMessage> { inBaseMessage });
            _latestSyncedTimestamp = GetLatestCreateAt(_succeededMessages, _latestSyncedTimestamp);
        }

        internal override void OnMessageUpdated(SbBaseChannel inBaseChannel, List<SbBaseMessage> inMessages)
        {
            if (IsValidChannel(inBaseChannel) == false || inMessages == null || inMessages.Count <= 0)
                return;

            SbMessageContext messageContext = SbCollectionEventSource.EventMessageUpdated.ToCachedMessageContext(inMessages[0].SendingStatus);
            UpsertToSucceededMessagesList(messageContext, inMessages);
        }

        internal override void OnMessageDeleted(SbBaseChannel inBaseChannel, long inMessageId)
        {
            if (IsValidChannel(inBaseChannel) == false)
                return;

            UpsertToSucceededMessagesList(SbCollectionEventSource.EventMessageDeleted.ToCachedMessageContext(), inAddOrUpdateMessages: null, new List<long> { inMessageId });
        }

        internal override void OnMessageSent(SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage)
        {
            if (IsValidChannel(inBaseChannel) == false || inBaseMessage == null)
                return;

            RemoveFromPendingMessagesList(inBaseMessage.RequestId);
            RemoveFromFailedMessagesList(inBaseMessage.RequestId);
            SbMessageContext messageContext = SbCollectionEventSource.EventMessageSent.ToCachedMessageContext(inBaseMessage.SendingStatus);
            UpsertToSucceededMessagesList(messageContext, new List<SbBaseMessage> { inBaseMessage });
        }

        internal override void OnReactionUpdated(SbBaseChannel inBaseChannel, SbReactionEvent inReactionEvent)
        {
            if (IsValidChannel(inBaseChannel) == false || inReactionEvent == null)
                return;

            SbBaseMessage foundMessage = _succeededMessages.Find(inMessage => inMessage.MessageId == inReactionEvent.MessageId);
            if (foundMessage == null)
                return;

            foundMessage.ApplyReactionEvent(inReactionEvent);
            InvokeOnMessagesUpdatedHandler(SbCollectionEventSource.EventReactionUpdated.ToCachedMessageContext(), new List<SbBaseMessage> { foundMessage });
        }

        internal override void OnThreadInfoUpdated(SbBaseChannel inBaseChannel, SbThreadInfoUpdateEvent inThreadInfoUpdateEvent)
        {
            if (IsValidChannel(inBaseChannel) == false || inThreadInfoUpdateEvent == null)
                return;

            SbBaseMessage foundMessage = _succeededMessages.Find(inMessage => inMessage.MessageId == inThreadInfoUpdateEvent.TargetMessageId);
            if (foundMessage == null)
                return;

            foundMessage.ApplyThreadInfoUpdateEvent(inThreadInfoUpdateEvent);
            InvokeOnMessagesUpdatedHandler(SbCollectionEventSource.EventThreadInfoUpdated.ToCachedMessageContext(), new List<SbBaseMessage> { foundMessage });
        }

        internal override void OnMessagePending(SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage)
        {
            if (IsValidChannel(inBaseChannel) == false || inBaseMessage == null)
                return;

            RemoveFromFailedMessagesList(inBaseMessage.RequestId);

            int foundIndex = _pendingMessages.FindIndex(inPendingMessage => inPendingMessage.RequestId == inBaseMessage.RequestId);
            if (0 <= foundIndex)
            {
                _pendingMessages[foundIndex] = inBaseMessage;
                SbMessageContext messageContext = SbCollectionEventSource.LocalMessageResendStarted.ToCachedMessageContext(inBaseMessage.SendingStatus);
                InvokeOnMessagesUpdatedHandler(messageContext, new List<SbBaseMessage> { inBaseMessage });
            }
            else
            {
                _pendingMessages.Add(inBaseMessage);
                SbMessageContext messageContext = SbCollectionEventSource.LocalMessagePendingCreated.ToCachedMessageContext(inBaseMessage.SendingStatus);
                InvokeOnMessagesAddedHandler(messageContext, new List<SbBaseMessage> { inBaseMessage });
            }
        }

        internal override void OnMessageFailed(SbCollectionEventSource inEventSource, SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage)
        {
            if (IsValidChannel(inBaseChannel) == false || inBaseMessage == null)
                return;

            RemoveFromPendingMessagesList(inBaseMessage.RequestId);

            int foundIndex = _failedMessages.FindIndex(inFailedMessage => inFailedMessage.RequestId == inBaseMessage.RequestId);
            if (0 <= foundIndex)
            {
                _failedMessages[foundIndex] = inBaseMessage;
            }
            else
            {
                _failedMessages.Add(inBaseMessage);
            }

            SbMessageContext messageContext = inEventSource.ToCachedMessageContext(inBaseMessage.SendingStatus);
            InvokeOnMessagesUpdatedHandler(messageContext, new List<SbBaseMessage> { inBaseMessage });
        }

        private bool IsValidChannel(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null || inBaseChannel.ChannelType != _groupChannel.ChannelType || inBaseChannel.Url != _groupChannel.Url)
                return false;

            return true;
        }

        
        private void InvokeOnChannelUpdatedHandler(SbMessageContext inMessageContext, SbGroupChannel inChannel)
        {
            _messageCollectionHandler?.OnChannelUpdated?.Invoke(inMessageContext, inChannel);
        }
        
        private void InvokeOnChannelDeletedHandler(SbMessageContext inMessageContext, string inDeletedChannelUrl)
        {
            _messageCollectionHandler?.OnChannelDeleted?.Invoke(inMessageContext, inDeletedChannelUrl);
        }
        
        private void InvokeOnMessagesAddedHandler(SbMessageContext inMessageContext, IReadOnlyList<SbBaseMessage> inMessages)
        {
            _messageCollectionHandler?.OnMessagesAdded?.Invoke(inMessageContext, inMessages);
        }
        
        private void InvokeOnMessagesUpdatedHandler(SbMessageContext inMessageContext, IReadOnlyList<SbBaseMessage> inMessages)
        {
            _messageCollectionHandler?.OnMessagesUpdated?.Invoke(inMessageContext, inMessages);
        }
        
        private void InvokeOnMessagesDeletedHandler(SbMessageContext inMessageContext, IReadOnlyList<SbBaseMessage> inMessages)
        {
            _messageCollectionHandler?.OnMessagesDeleted?.Invoke(inMessageContext, inMessages);
        }
        
        private void InvokeOnHugeGapDetectedHandler()
        {
            _messageCollectionHandler?.OnHugeGapDetected?.Invoke();
        }
    }
}