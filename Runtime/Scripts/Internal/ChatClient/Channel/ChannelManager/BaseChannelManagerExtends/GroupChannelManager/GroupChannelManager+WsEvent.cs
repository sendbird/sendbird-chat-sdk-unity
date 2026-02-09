// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Linq;

namespace Sendbird.Chat
{
    internal partial class GroupChannelManager
    {
        internal void OnReceiveWsEventCommand(WsReceiveCommandAbstract inWsReceiveCommand)
        {
            if (inWsReceiveCommand == null)
                return;

            switch (inWsReceiveCommand.CommandType)
            {
                case WsCommandType.DeleteMessage:
                case WsCommandType.Reaction:
                case WsCommandType.VotePoll:
                case WsCommandType.UpdatePoll:
                    ProcessBaseChannelWsEventCommand(inWsReceiveCommand);
                    break;

                case WsCommandType.ChannelEvent:
                    OnReceiveChannelEvent(inWsReceiveCommand);
                    break;

                case WsCommandType.UserEvent:
                    OnReceiveUserEventWsEvent(inWsReceiveCommand);
                    break;

                case WsCommandType.MemberUpdateCount:
                    OnReceiveMemberUpdateCountWsEvent(inWsReceiveCommand);
                    break;

                case WsCommandType.Read:
                    OnReceiveReadWsEvent(inWsReceiveCommand);
                    break;

                case WsCommandType.Delivery:
                    OnReceiveDeliveryWsEvent(inWsReceiveCommand);
                    break;

                case WsCommandType.Threads:
                    OnReceiveThreadsWsEvent(inWsReceiveCommand);
                    break;

                case WsCommandType.AdminMessage:
                case WsCommandType.FileMessage:
                case WsCommandType.UserMessage:
                    OnReceiveNewMessageWsEvent(inWsReceiveCommand as MessageWsReceiveCommandAbstract);
                    break;

                case WsCommandType.UpdateAdminMessage:
                case WsCommandType.UpdateFileMessage:
                case WsCommandType.UpdateUserMessage:
                    OnReceiveUpdateMessageWsEvent(inWsReceiveCommand as UpdateMessageWsReceiveCommandAbstract);
                    break;
            }
        }

        private void OnReceiveNewMessageWsEvent(MessageWsReceiveCommandAbstract inMessageWsReceiveCommand)
        {
            if (inMessageWsReceiveCommand == null || inMessageWsReceiveCommand.BaseMessageDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveNewMessageWsEvent invalid params");
                return;
            }

            if (inMessageWsReceiveCommand.BaseMessageDto.ChannelType != SbChannelType.Group)
                return;

            void OnGetChannelCompletionHandler(SbGroupChannel inGroupChannel, bool inIsFromCache, SbError inError)
            {
                if (inError != null)
                {
                    Logger.Warning(Logger.CategoryType.Channel, $"GroupChannelManager::OnReceiveNewMessageWsEvent GetChannel Error:{inError.ErrorCode} Message:{inError.ErrorMessage}");
                    return;
                }

                BaseMessageDto baseMessageDto = inMessageWsReceiveCommand.BaseMessageDto;
                bool channelWasChanged = !inIsFromCache;
                if (inGroupChannel.HiddenState == SbGroupChannelHiddenState.HiddenPreventAutoUnhide)
                {
                    inGroupChannel.IsHidden = false;
                    inGroupChannel.HiddenState = SbGroupChannelHiddenState.Unhidden;
                }

                if (baseMessageDto.senderDto != null)
                    inGroupChannel.UpdateMember(baseMessageDto.senderDto);

                if (baseMessageDto.mentionedUserDtos != null)
                    inGroupChannel.UpdateMembers(baseMessageDto.mentionedUserDtos);

                SbBaseMessage message = baseMessageDto.CreateMessageInstance(chatMainContextRef);

                if (inGroupChannel.ShouldUpdateLsatMessage(message, baseMessageDto.forceUpdateLastMessage))
                {
                    inGroupChannel.LastMessage = message;
                    channelWasChanged = true;
                }

                if (inIsFromCache && inGroupChannel.UpdateUnreadCount(message))
                    channelWasChanged = true;

                if (channelWasChanged)
                {
                    channelHandlersById.ForEachByValue(inHandler => { inHandler.OnChannelChanged?.Invoke(inGroupChannel); });
                    chatMainContextRef.CollectionManager.OnChannelChanged(inGroupChannel);
                }

                if (inMessageWsReceiveCommand.IsCreatedFromCurrentDevice() == false)
                {
                    channelHandlersById.ForEachByValue(inHandler => { inHandler.OnMessageReceived?.Invoke(inGroupChannel, message); });
                    chatMainContextRef.CollectionManager.OnMessageReceived(inGroupChannel, message);

                    if (inGroupChannel.ShouldDisableMack() == false)
                    {
                        MessageAckWsSendCommand messageAckWsSendCommand = new MessageAckWsSendCommand(inGroupChannel.Url, message.MessageId);
                        chatMainContextRef.CommandRouter.SendWsCommand(messageAckWsSendCommand);
                    }
                }

                if (message.IsMentionedToMe(chatMainContextRef.CurrentUserId))
                {
                    channelHandlersById.ForEachByValue(inHandler => { inHandler.OnMentionReceived?.Invoke(inGroupChannel, message); });
                    chatMainContextRef.CollectionManager.OnMentionReceived(inGroupChannel);
                }
            }

            GetChannel(inMessageWsReceiveCommand.BaseMessageDto.channelUrl, inIsInternal: false, inIsForceRefresh: false, OnGetChannelCompletionHandler);
        }

        private void OnReceiveUpdateMessageWsEvent(UpdateMessageWsReceiveCommandAbstract inUpdateMessageWsReceiveCommand)
        {
            if (inUpdateMessageWsReceiveCommand == null || inUpdateMessageWsReceiveCommand.BaseMessageDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveUpdateMessageWsEvent invalid params");
                return;
            }

            if (inUpdateMessageWsReceiveCommand.BaseMessageDto.ChannelType != SbChannelType.Group)
                return;

            void OnGetChannelCompletionHandler(SbGroupChannel inGroupChannel, bool inIsFromCache, SbError inError)
            {
                if (inError != null)
                {
                    Logger.Warning(Logger.CategoryType.Channel, $"GroupChannelManager::OnReceiveNewMessageWsEvent GetChannel Error:{inError.ErrorCode} Message:{inError.ErrorMessage}");
                    return;
                }

                BaseMessageDto baseMessageDto = inUpdateMessageWsReceiveCommand.BaseMessageDto;
                bool channelWasChanged = false;
                bool mentionReceived = false;
                bool pinnedMessageChanged = false;

                SbBaseMessage updateMessage = baseMessageDto.CreateMessageInstance(chatMainContextRef);

                if (inGroupChannel.ShouldUpdateLsatMessage(updateMessage, baseMessageDto.forceUpdateLastMessage))
                {
                    inGroupChannel.LastMessage = updateMessage;
                    channelWasChanged = true;
                }

                long myReadReceiptTimestamp = inGroupChannel.GetReadReceiptTimestamp(chatMainContextRef.CurrentUserId);
                bool hasUpdatedLaterMyReadReceiptTimestamp = myReadReceiptTimestamp < updateMessage.CreatedAt && myReadReceiptTimestamp < updateMessage.UpdatedAt;
                bool fromCurrentUser = updateMessage.Sender?.UserId == chatMainContextRef.CurrentUserId;

                if (updateMessage.IsSilent == false && fromCurrentUser == false && hasUpdatedLaterMyReadReceiptTimestamp)
                {
                    bool containsCurrentUserInOldMentionedUsers = inUpdateMessageWsReceiveCommand.ContainsUserInOldMentionedUsers(chatMainContextRef.CurrentUserId);
                    bool containsCurrentUserInUpdateMentionedUsers = false;
                    if (inUpdateMessageWsReceiveCommand.BaseMessageDto.mentionedUserDtos != null)
                        containsCurrentUserInUpdateMentionedUsers = inUpdateMessageWsReceiveCommand.BaseMessageDto.mentionedUserDtos.Any(inUserDto => inUserDto.UserId == chatMainContextRef.CurrentUserId);

                    if ((inUpdateMessageWsReceiveCommand.HasChangedMentionTypeTo(SbMentionType.Channel) && containsCurrentUserInOldMentionedUsers) ||
                        (inUpdateMessageWsReceiveCommand.HasOldMentionedUsers() && containsCurrentUserInOldMentionedUsers == false && containsCurrentUserInUpdateMentionedUsers))
                    {
                        channelWasChanged = true;
                        mentionReceived = true;

                        if (inIsFromCache)
                            inGroupChannel.IncreaseUnreadMentionCountIfEnabled();
                    }
                }

                if (baseMessageDto.senderDto != null)
                    inGroupChannel.UpdateMember(baseMessageDto.senderDto);

                if (baseMessageDto.mentionedUserDtos != null)
                    inGroupChannel.UpdateMembers(baseMessageDto.mentionedUserDtos);

                if (updateMessage.MessageId == inGroupChannel.LastPinnedMessage?.MessageId)
                {
                    channelWasChanged = true;
                    pinnedMessageChanged = true;
                    inGroupChannel.LastPinnedMessage = updateMessage;
                }

                channelHandlersById.ForEachByValue(inHandler => { inHandler.OnMessageUpdated?.Invoke(inGroupChannel, updateMessage); });
                chatMainContextRef.CollectionManager.OnMessageUpdated(inGroupChannel, updateMessage);

                if (channelWasChanged)
                {
                    channelHandlersById.ForEachByValue(inHandler => { inHandler.OnChannelChanged?.Invoke(inGroupChannel); });
                    chatMainContextRef.CollectionManager.OnChannelChanged(inGroupChannel);
                }

                if (mentionReceived)
                {
                    channelHandlersById.ForEachByValue(inHandler => { inHandler.OnMentionReceived?.Invoke(inGroupChannel, updateMessage); });
                    chatMainContextRef.CollectionManager.OnMentionReceived(inGroupChannel);
                }

                if (pinnedMessageChanged)
                {
                    channelHandlersById.ForEachByValue(inHandler => { inHandler.OnPinnedMessageUpdated?.Invoke(inGroupChannel); });
                    chatMainContextRef.CollectionManager.OnPinnedMessageUpdated(inGroupChannel);
                }
            }

            GetChannel(inUpdateMessageWsReceiveCommand.BaseMessageDto.channelUrl, inIsInternal: false, inIsForceRefresh: false, OnGetChannelCompletionHandler);
        }

        private void OnReceiveReadWsEvent(WsReceiveCommandAbstract inWsReceiveCommand)
        {
            ReadWsReceiveCommand readWsEvent = inWsReceiveCommand as ReadWsReceiveCommand;

            if (readWsEvent == null || string.IsNullOrEmpty(readWsEvent.channelUrl) || readWsEvent.userDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveReadWsEvent invalid params");
                return;
            }

            if (readWsEvent.ChannelType != SbChannelType.Group)
                return;

            void OnGetChannelCompletionHandler(SbGroupChannel inGroupChannel, bool inIsFromCache, SbError inError)
            {
                if (inGroupChannel == null)
                    return;

                inGroupChannel.UpdateReadReceipt(readWsEvent.userDto.UserId, readWsEvent.timestamp);

                if (readWsEvent.userDto.UserId == chatMainContextRef.CurrentUserId)
                {
                    inGroupChannel.MyLastRead = readWsEvent.timestamp;
                    bool shouldCallChange = 0 < inGroupChannel.UnreadMessageCount || 0 < inGroupChannel.UnreadMentionCount;

                    if (inIsFromCache)
                        inGroupChannel.ClearAllUnreadCount();

                    if (shouldCallChange)
                    {
                        channelHandlersById.ForEachByValue(inHandler => { inHandler.OnChannelChanged?.Invoke(inGroupChannel); });
                        chatMainContextRef.CollectionManager.OnChannelChanged(inGroupChannel);
                    }
                }
                else
                {
                    channelHandlersById.ForEachByValue(inHandler => { inHandler.OnReadStatusUpdated?.Invoke(inGroupChannel); });
                    chatMainContextRef.CollectionManager.OnReadStatusUpdated(inGroupChannel);
                }
            }

            GetChannel(readWsEvent.channelUrl, inIsInternal: false, inIsForceRefresh: false, OnGetChannelCompletionHandler);
        }

        private void OnReceiveDeliveryWsEvent(WsReceiveCommandAbstract inWsReceiveCommand)
        {
            DeliveryWsReceiveCommand deliveryWsReceive = inWsReceiveCommand as DeliveryWsReceiveCommand;

            if (deliveryWsReceive == null || string.IsNullOrEmpty(deliveryWsReceive.channelUrl) || deliveryWsReceive.updated == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveDeliveryWsEvent invalid params");
                return;
            }

            void OnGetChannelCompletionHandler(SbGroupChannel inGroupChannel, bool inIsFromCache, SbError inError)
            {
                if (inGroupChannel == null)
                    return;

                inGroupChannel.UpdateDeliveryReceipts(deliveryWsReceive.updated);

                if (string.IsNullOrEmpty(chatMainContextRef.CurrentUserId))
                    return;

                bool isOnlyMyReceipt = deliveryWsReceive.updated.ContainsKey(chatMainContextRef.CurrentUserId) && deliveryWsReceive.updated.Count == 1;
                if (isOnlyMyReceipt)
                    return;

                channelHandlersById.ForEachByValue(inHandler => { inHandler.OnDeliveryStatusUpdated?.Invoke(inGroupChannel); });
                chatMainContextRef.CollectionManager.OnDeliveryStatusUpdated(inGroupChannel);
            }

            GetChannel(deliveryWsReceive.channelUrl, inIsInternal: false, inIsForceRefresh: false, OnGetChannelCompletionHandler);
        }

        private void OnReceiveThreadsWsEvent(WsReceiveCommandAbstract inWsReceiveCommand)
        {
            ThreadsWsReceiveCommand threadWsEvent = inWsReceiveCommand as ThreadsWsReceiveCommand;

            if (threadWsEvent == null || string.IsNullOrEmpty(threadWsEvent.channelUrl) || threadWsEvent.threadInfoDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveThreadsWsEvent invalid params");
                return;
            }

            if (threadWsEvent.ChannelType != SbChannelType.Group)
                return;

            void OnGetChannelCompletionHandler(SbGroupChannel inGroupChannel, bool inIsFromCache, SbError inError)
            {
                if (inGroupChannel == null)
                    return;

                SbThreadInfoUpdateEvent threadInfoUpdateEvent = new SbThreadInfoUpdateEvent(
                    threadWsEvent.channelUrl, threadWsEvent.ChannelType, threadWsEvent.rootMsgId, threadWsEvent.threadInfoDto, chatMainContextRef);

                channelHandlersById.ForEachByValue(inHandler => { inHandler.OnThreadInfoUpdated?.Invoke(inGroupChannel, threadInfoUpdateEvent); });
                chatMainContextRef.CollectionManager.OnThreadInfoUpdated(inGroupChannel, threadInfoUpdateEvent);
            }

            GetChannel(threadWsEvent.channelUrl, inIsInternal: false, inIsForceRefresh: false, OnGetChannelCompletionHandler);
        }

        private void OnReceiveMemberUpdateCountWsEvent(WsReceiveCommandAbstract inWsReceiveCommand)
        {
            MemberUpdateCountWsReceiveCommand memberUpdateCountWsEvent = inWsReceiveCommand as MemberUpdateCountWsReceiveCommand;

            if (memberUpdateCountWsEvent == null || memberUpdateCountWsEvent.groupChannelMemberCountObjects == null ||
                memberUpdateCountWsEvent.groupChannelMemberCountObjects.Count <= 0)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveMemberUpdateCountWsEvent invalid params");
                return;
            }


            List<SbGroupChannel> changedGroupChannels = new List<SbGroupChannel>(memberUpdateCountWsEvent.groupChannelMemberCountObjects.Count);

            foreach (GroupChannelMemberCountDto groupChannelMemberCountCObject in memberUpdateCountWsEvent.groupChannelMemberCountObjects)
            {
                SbGroupChannel cachedGroupChannel = FindCachedChannel(groupChannelMemberCountCObject.channelUrl);
                if (cachedGroupChannel == null)
                    continue;

                if (cachedGroupChannel.UpdateMemberCount(groupChannelMemberCountCObject))
                    changedGroupChannels.Add(cachedGroupChannel);
            }

            if (changedGroupChannels.Count <= 0)
                return;

            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnChannelMemberCountChanged?.Invoke(changedGroupChannels); });
            chatMainContextRef.CollectionManager.OnChannelMemberCountChanged(changedGroupChannels);
        }

        private void OnReceiveUserEventWsEvent(WsReceiveCommandAbstract inWsReceiveCommand)
        {
            UserEventWsReceiveCommand userEventWsCommand = inWsReceiveCommand as UserEventWsReceiveCommand;
            if (userEventWsCommand == null || userEventWsCommand.EventData == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveUserEventWsEvent invalid params");
                return;
            }

            switch (userEventWsCommand.EventData.CategoryType)
            {
                case UserEventWsReceiveCommand.CategoryType.Block:
                    OnReceiveBlockUserEvent(userEventWsCommand.EventData as BlockUserEventData);
                    return;

                case UserEventWsReceiveCommand.CategoryType.Unblock:
                    OnReceiveUnblockUserEvent(userEventWsCommand.EventData as UnblockUserEventData);
                    return;
            }
        }

        private void OnReceiveBlockUserEvent(BlockUserEventData inBlockUserEventData)
        {
            if (inBlockUserEventData == null || inBlockUserEventData.BlockerUserDto == null || inBlockUserEventData.BlockeeUserDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveBlockUserEvent invalid params");
                return;
            }

            if (inBlockUserEventData.BlockerUserDto.UserId != chatMainContextRef.CurrentUserId)
                return;

            foreach (SbGroupChannel cachedGroupChannel in cachedChannelsByUrl.Values)
            {
                foreach (SbMember member in cachedGroupChannel.Members)
                {
                    if (member.UserId == inBlockUserEventData.BlockeeUserDto.UserId)
                    {
                        member.IsBlockedByMe = true;
                    }
                }
            }
        }

        private void OnReceiveUnblockUserEvent(UnblockUserEventData inUnblockUserEventData)
        {
            if (inUnblockUserEventData == null || inUnblockUserEventData.BlockerUserDto == null || inUnblockUserEventData.BlockeeUserDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveUnblockUserEvent invalid params");
                return;
            }

            if (inUnblockUserEventData.BlockerUserDto.UserId != chatMainContextRef.CurrentUserId)
                return;

            foreach (SbGroupChannel cachedGroupChannel in cachedChannelsByUrl.Values)
            {
                foreach (SbMember member in cachedGroupChannel.Members)
                {
                    if (member.UserId == inUnblockUserEventData.BlockeeUserDto.UserId)
                    {
                        member.IsBlockedByMe = false;
                    }
                }
            }
        }

        private void OnReceiveChannelEvent(WsReceiveCommandAbstract inWsReceiveCommand)
        {
            ChannelReceiveWsReceiveCommand channelReceiveWsCommand = inWsReceiveCommand as ChannelReceiveWsReceiveCommand;
            if (channelReceiveWsCommand == null || channelReceiveWsCommand.EventData == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveChannelEvent invalid params");
                return;
            }

            if (channelReceiveWsCommand.EventData.ChannelType != SbChannelType.Group)
                return;

            if (channelReceiveWsCommand.EventData.CategoryType == ChannelReceiveWsReceiveCommand.CategoryType.Deleted)
            {
                OnReceiveDeleteChannelEvent(channelReceiveWsCommand.EventData);
                return;
            }

            void OnGetChannelCompletionHandler(SbGroupChannel inGroupChannel, bool inIsCached, SbError inError)
            {
                if (inGroupChannel != null)
                {
                    ProcessChannelEvent(inGroupChannel, channelReceiveWsCommand.EventData);
                }
                else if (inError != null)
                {
                    Logger.Warning(Logger.CategoryType.Channel, $"GroupChannelManager::OnReceiveChannelEvent GetChannel Error:{inError.ErrorCode} Message:{inError.ErrorMessage}");
                }
            }

            bool isForceRefresh = channelReceiveWsCommand.EventData.CategoryType == ChannelReceiveWsReceiveCommand.CategoryType.PropChanged;
            GetChannel(channelReceiveWsCommand.EventData.channelUrl, inIsInternal: false, isForceRefresh, OnGetChannelCompletionHandler);
        }

        private void ProcessChannelEvent(SbGroupChannel inGroupChannel, ChannelEventDataAbstract inChannelEventData)
        {
            if (inGroupChannel == null || inChannelEventData == null || inChannelEventData.ChannelType != SbChannelType.Group)
            {
                Logger.Warning(Logger.CategoryType.Channel, "ProcessChannelEvent invalid params");
                return;
            }

            switch (inChannelEventData.CategoryType)
            {
                case ChannelReceiveWsReceiveCommand.CategoryType.Frozen:
                case ChannelReceiveWsReceiveCommand.CategoryType.Unfrozen:
                case ChannelReceiveWsReceiveCommand.CategoryType.MetaDataChanged:
                case ChannelReceiveWsReceiveCommand.CategoryType.MetaCounterChanged:
                    ProcessBaseChannelEvent(inGroupChannel, inChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Join:
                    OnReceiveJoinChannelEvent(inGroupChannel, inChannelEventData as JoinChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Leave:
                    OnReceiveLeaveChannelEvent(inGroupChannel, inChannelEventData as LeaveChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.TypingStart:
                case ChannelReceiveWsReceiveCommand.CategoryType.TypingEnd:
                    OnReceiveTypingStartOrEndChannelEvent(inGroupChannel, inChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Mute:
                    OnReceiveMuteChannelEvent(inGroupChannel, inChannelEventData as MuteChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Unmute:
                    OnReceiveUnmuteChannelEvent(inGroupChannel, inChannelEventData as UnmuteChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Ban:
                    OnReceiveBanChannelEvent(inGroupChannel, inChannelEventData as BanChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Unban:
                    OnReceiveUnbanChannelEvent(inGroupChannel, inChannelEventData as UnbanChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Hidden:
                    OnReceiveHiddenChannelEvent(inGroupChannel, inChannelEventData as HiddenChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Unhidden:
                    OnReceiveUnhiddenChannelEvent(inGroupChannel, inChannelEventData as UnhiddenChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.UpdateOperator:
                    OnReceiveUpdateOperatorChannelEvent(inGroupChannel, inChannelEventData as UpdateOperatorChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.PropChanged:
                    OnReceivePropChangeChannelEvent(inGroupChannel);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Invite:
                    OnReceiveInviteChannelEvent(inGroupChannel, inChannelEventData as InviteChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.DeclineInvite:
                    OnReceiveDeclineInviteChannelEventData(inGroupChannel, inChannelEventData as DeclineInviteChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.PinnedMessageUpdated:
                    OnReceivePinnedMessageUpdatedChannelEvent(inGroupChannel, inChannelEventData as PinnedMessageUpdatedChannelEventData);
                    return;
            }
        }

        private void OnReceiveDeleteChannelEvent(ChannelEventDataAbstract inChannelEventData)
        {
            if (inChannelEventData == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveDeleteChannelEvent invalid params");
                return;
            }

            channelHandlersById.ForEachByValue(inGroupHandler => { inGroupHandler.OnChannelDeleted?.Invoke(inChannelEventData.channelUrl, inChannelEventData.ChannelType); });
            chatMainContextRef.CollectionManager.OnChannelDeleted(inChannelEventData.channelUrl, inChannelEventData.ChannelType);
            RemoveCachedChannelIfContains(inChannelEventData.channelUrl);
        }

        private void OnReceiveJoinChannelEvent(SbGroupChannel inGroupChannel, JoinChannelEventData inJoinEventData)
        {
            if (inGroupChannel == null || inJoinEventData == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveJoinChannelEvent invalid params");
                return;
            }

            if (inGroupChannel.IsSuper)
                inGroupChannel.UpdateMemberCount(inJoinEventData);

            if (inJoinEventData.MemberDtos == null || inJoinEventData.MemberDtos.Count <= 0)
                return;

            foreach (MemberDto memberDto in inJoinEventData.MemberDtos)
            {
                SbMember member = null;
                if (!inGroupChannel.IsExclusive && !inGroupChannel.IsSuper && !inGroupChannel.IsBroadcast)
                {
                    member = inGroupChannel.AddMember(memberDto, inJoinEventData.timestamp);
                    inGroupChannel.UpdateJoinedMemberCount();
                }
                else
                {
                    member = new SbMember(memberDto, chatMainContextRef);
                }

                if (member != null)
                {
                    if (member.UserId == chatMainContextRef.CurrentUserId)
                    {
                        inGroupChannel.MyMemberState = SbMemberState.Joined;
                    }

                    channelHandlersById.ForEachByValue(inGroupHandler => { inGroupHandler.OnUserJoined?.Invoke(inGroupChannel, member); });
                    chatMainContextRef.CollectionManager.OnUserJoined(inGroupChannel, member);
                }
            }
        }

        private void OnReceiveLeaveChannelEvent(SbGroupChannel inGroupChannel, LeaveChannelEventData inLeaveEventData)
        {
            if (inGroupChannel == null || inLeaveEventData == null || inLeaveEventData.MemberDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveLeaveChannelEvent invalid params");
                return;
            }

            SbMember member = inGroupChannel.FindMember(inLeaveEventData.MemberDto.UserId);
            if (member == null)
            {
                if (inGroupChannel.IsExclusive || inGroupChannel.IsSuper || inGroupChannel.IsBroadcast)
                    member = new SbMember(inLeaveEventData.MemberDto, chatMainContextRef);
                else
                    return;
            }

            if (inLeaveEventData.BaseChannelDto is GroupChannelDto groupChannelDto)
            {
                inGroupChannel.ResetMembersFromChannelDto(groupChannelDto);
            }

            if (inGroupChannel.IsExclusive || inGroupChannel.IsSuper || inGroupChannel.IsBroadcast)
            {
                inGroupChannel.UpdateMemberCount(inLeaveEventData);
            }
            else
            {
                inGroupChannel.UpdateJoinedMemberCount();
            }

            bool isChangedTypingStatus = inGroupChannel.UpdateTypingStatus(member, inDidStart: false);

            if (member.UserId == chatMainContextRef.CurrentUserId)
            {
                inGroupChannel.MyMemberState = SbMemberState.None;
                inGroupChannel.ClearAllUnreadCount();
                inGroupChannel.InvitedAt = 0;
                inGroupChannel.JoinedAt = 0;
                RemoveCachedChannelIfContains(inGroupChannel.Url);
            }
            else if (!inGroupChannel.IsExclusive && !inGroupChannel.IsSuper && !inGroupChannel.IsBroadcast
                     && inLeaveEventData.BaseChannelDto != null)
            {
                inGroupChannel.ResetFromChannelDto(inLeaveEventData.BaseChannelDto);
            }

            channelHandlersById.ForEachByValue(inGroupHandler => { inGroupHandler.OnUserLeft?.Invoke(inGroupChannel, member); });
            chatMainContextRef.CollectionManager.OnUserLeftChannel(inGroupChannel, member);
            if (isChangedTypingStatus)
            {
                channelHandlersById.ForEachByValue(inGroupHandler => { inGroupHandler.OnTypingStatusUpdated?.Invoke(inGroupChannel); });
                chatMainContextRef.CollectionManager.OnTypingStatusUpdated(inGroupChannel);
            }
        }

        private void OnReceiveInviteChannelEvent(SbGroupChannel inGroupChannel, InviteChannelEventData inInviteEventData)
        {
            if (inGroupChannel == null || inInviteEventData == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveInviteChannelEvent invalid params");
                return;
            }

            if (inGroupChannel.IsSuper)
            {
                inGroupChannel.UpdateMemberCount(inInviteEventData);
            }
            else if (inInviteEventData.InviterMemberDto != null)
            {
                inGroupChannel.AddMember(inInviteEventData.InviterMemberDto, inInviteEventData.timestamp);
                inGroupChannel.UpdateJoinedMemberCount();
            }

            List<SbUser> invitees = null;
            if (inInviteEventData.InviteeMemberDtos != null && 0 < inInviteEventData.InviteeMemberDtos.Count)
            {
                List<SbUser> tempInvitees = new List<SbUser>(inInviteEventData.InviteeMemberDtos.Count);
                foreach (MemberDto inviteeMemberDto in inInviteEventData.InviteeMemberDtos)
                {
                    SbMember inviteeMember = null;
                    inGroupChannel.InvitedAt = inInviteEventData.timestamp;

                    if (inGroupChannel.IsSuper == false)
                    {
                        inviteeMember = inGroupChannel.AddMember(inviteeMemberDto, inInviteEventData.timestamp);
                        inGroupChannel.UpdateJoinedMemberCount();
                    }

                    if (inviteeMemberDto.UserId == chatMainContextRef.CurrentUserId)
                    {
                        if (inGroupChannel.MyMemberState != SbMemberState.Joined)
                            inGroupChannel.MyMemberState = SbMemberState.Invited;

                        if (inGroupChannel.HiddenState == SbGroupChannelHiddenState.HiddenAllowAutoUnhide)
                        {
                            inGroupChannel.IsHidden = false;
                            inGroupChannel.HiddenState = SbGroupChannelHiddenState.Unhidden;
                        }
                    }

                    if (inviteeMember == null)
                    {
                        inviteeMember = new SbMember(inviteeMemberDto, chatMainContextRef);
                    }

                    tempInvitees.Add(inviteeMember);
                }

                invitees = new List<SbUser>(tempInvitees);
            }

            SbMember inviter = null;
            if (inInviteEventData.InviterMemberDto != null)
            {
                inviter = inGroupChannel.FindMember(inInviteEventData.InviterMemberDto.UserId);
                if (inviter == null)
                {
                    inviter = new SbMember(inInviteEventData.InviterMemberDto, chatMainContextRef);
                }
            }

            channelHandlersById.ForEachByValue(inGroupHandler => { inGroupHandler.OnUserReceivedInvitation?.Invoke(inGroupChannel, inviter, invitees); });
            chatMainContextRef.CollectionManager.OnUserReceivedInvitation(inGroupChannel);
        }

        private void OnReceiveDeclineInviteChannelEventData(SbGroupChannel inGroupChannel, DeclineInviteChannelEventData inDeclineInviteEventData)
        {
            if (inGroupChannel == null || inDeclineInviteEventData == null || inDeclineInviteEventData.InviteeMemberDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveDeclineInviteChannelEventData invalid params");
                return;
            }

            SbMember invitee = inGroupChannel.FindMember(inDeclineInviteEventData.InviteeMemberDto.UserId);
            if (invitee == null)
                invitee = new SbMember(inDeclineInviteEventData.InviteeMemberDto, chatMainContextRef);

            if (inGroupChannel.IsSuper)
            {
                inGroupChannel.UpdateMemberCount(inDeclineInviteEventData);
            }

            if (inDeclineInviteEventData.InviteeMemberDto.UserId == chatMainContextRef.CurrentUserId)
            {
                inGroupChannel.MyMemberState = SbMemberState.None;
                inGroupChannel.InvitedAt = 0;
                RemoveCachedChannelIfContains(inDeclineInviteEventData.channelUrl);
            }
            else
            {
                if (inDeclineInviteEventData.BaseChannelDto != null)
                {
                    inGroupChannel.ResetFromChannelDto(inDeclineInviteEventData.BaseChannelDto);
                }

                if (inGroupChannel.IsSuper == false)
                {
                    inGroupChannel.RemoveMember(invitee.UserId);
                    inGroupChannel.UpdateJoinedMemberCount();
                }
            }

            SbMember inviter = null;
            if (inDeclineInviteEventData.InviterMemberDto != null)
            {
                inviter = inGroupChannel.FindMember(inDeclineInviteEventData.InviterMemberDto.UserId);

                if (inviter == null)
                    inviter = new SbMember(inDeclineInviteEventData.InviterMemberDto, chatMainContextRef);
            }


            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnUserDeclinedInvitation?.Invoke(inGroupChannel, inviter, invitee); });
            chatMainContextRef.CollectionManager.OnUserDeclinedInvitation(inGroupChannel, invitee);
        }

        private void OnReceiveTypingStartOrEndChannelEvent(SbGroupChannel inGroupChannel, ChannelEventDataAbstract inEventData)
        {
            if (inGroupChannel == null || inEventData == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveTypingChannelEvent invalid params");
                return;
            }

            UserDto userDto = null;
            bool didStart = false;
            if (inEventData is TypingStartChannelEventData typingStartChannelEventData)
            {
                userDto = typingStartChannelEventData.UserDto;
                didStart = true;
            }
            else if (inEventData is TypingEndChannelEventData typingEndChannelEventData)
            {
                userDto = typingEndChannelEventData.UserDto;
                didStart = false;
            }

            if (userDto == null)
                return;

            SbUser user = inGroupChannel.FindMember(userDto.UserId);
            if (user == null)
            {
                user = new SbUser(userDto, chatMainContextRef);
            }

            bool changed = inGroupChannel.UpdateTypingStatus(user, didStart);
            if (changed)
            {
                channelHandlersById.ForEachByValue(inHandler => { inHandler.OnTypingStatusUpdated?.Invoke(inGroupChannel); });
                chatMainContextRef.CollectionManager.OnTypingStatusUpdated(inGroupChannel);
            }
        }

        private void OnReceivePropChangeChannelEvent(SbGroupChannel inGroupChannel)
        {
            if (inGroupChannel == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceivePropChangeChannelEvent invalid params");
                return;
            }

            if (inGroupChannel.IsMyUnreadMessageCountEnabled() == false)
                inGroupChannel.UnreadMessageCount = 0;

            if (inGroupChannel.IsMyUnreadMentionCountEnabled() == false)
                inGroupChannel.UnreadMentionCount = 0;

            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnChannelChanged?.Invoke(inGroupChannel); });
            chatMainContextRef.CollectionManager.OnChannelChanged(inGroupChannel);
        }

        private void OnReceiveUpdateOperatorChannelEvent(SbGroupChannel inGroupChannel, UpdateOperatorChannelEventData inUpdateOperatorEventData)
        {
            if (inGroupChannel == null || inUpdateOperatorEventData == null || inUpdateOperatorEventData.OperatorMemberDtos == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveUpdateOperatorChannelEvent invalid params");
                return;
            }

            foreach (SbMember groupChannelMember in inGroupChannel.Members)
            {
                SbRole role = SbRole.None;
                if (inUpdateOperatorEventData.OperatorMemberDtos.Any(inMemberDto => inMemberDto.UserId == groupChannelMember.UserId))
                {
                    role = SbRole.Operator;
                }

                groupChannelMember.Role = role;
                if (groupChannelMember.UserId == chatMainContextRef.CurrentUserId)
                    inGroupChannel.MyRole = role;
            }

            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnOperatorUpdated?.Invoke(inGroupChannel); });
            chatMainContextRef.CollectionManager.OnOperatorUpdated(inGroupChannel);
        }

        private void OnReceiveHiddenChannelEvent(SbGroupChannel inGroupChannel, HiddenChannelEventData inHiddenEventData)
        {
            if (inGroupChannel == null || inHiddenEventData == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveHiddenChannelEvent invalid params");
                return;
            }

            inGroupChannel.SetMessageOffsetTimestamp(inHiddenEventData.messageOffset);

            if (inHiddenEventData.HidePreviousMessages)
                inGroupChannel.ClearAllUnreadCount();

            inGroupChannel.IsHidden = true;
            inGroupChannel.HiddenState = inHiddenEventData.HiddenState;

            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnChannelHidden?.Invoke(inGroupChannel); });
            chatMainContextRef.CollectionManager.OnChannelHidden(inGroupChannel);
        }

        private void OnReceiveUnhiddenChannelEvent(SbGroupChannel inGroupChannel, UnhiddenChannelEventData inUnhiddenEventData)
        {
            if (inGroupChannel == null || inUnhiddenEventData == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveUnhiddenChannelEvent invalid params");
                return;
            }


            inGroupChannel.IsHidden = false;
            inGroupChannel.HiddenState = inUnhiddenEventData.HiddenState;

            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnChannelChanged?.Invoke(inGroupChannel); });
            chatMainContextRef.CollectionManager.OnChannelChanged(inGroupChannel);
        }

        private void OnReceiveMuteChannelEvent(SbGroupChannel inGroupChannel, MuteChannelEventData inMuteEventData)
        {
            if (inGroupChannel == null || inMuteEventData == null || inMuteEventData.RestrictedUserDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveMuteChannelEvent invalid params");
                return;
            }

            if (chatMainContextRef.CurrentUserId == inMuteEventData.RestrictedUserDto.UserId)
            {
                inGroupChannel.SetMyMutedState(SbMutedState.Muted);
            }

            SbMember member = inGroupChannel.FindMember(inMuteEventData.RestrictedUserDto.UserId);
            if (member != null)
            {
                member.IsMuted = true;
                if (inMuteEventData.RestrictedUserDto != null)
                    member.RestrictionInfo = new SbRestrictionInfo(inMuteEventData.RestrictedUserDto);
            }

            SbRestrictedUser restrictedUser = new SbRestrictedUser(inMuteEventData.RestrictedUserDto, chatMainContextRef);
            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnUserMuted?.Invoke(inGroupChannel, restrictedUser); });
            chatMainContextRef.CollectionManager.OnUserMuted(inGroupChannel, restrictedUser);
        }

        private void OnReceiveUnmuteChannelEvent(SbGroupChannel inGroupChannel, UnmuteChannelEventData inUnmuteEventData)
        {
            if (inGroupChannel == null || inUnmuteEventData == null || inUnmuteEventData.UserDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveUnmuteChannelEvent invalid params");
                return;
            }

            if (chatMainContextRef.CurrentUserId == inUnmuteEventData.UserDto.UserId)
            {
                inGroupChannel.SetMyMutedState(SbMutedState.Unmuted);
            }

            SbMember member = inGroupChannel.FindMember(inUnmuteEventData.UserDto.UserId);
            if (member != null)
            {
                member.IsMuted = false;
                member.RestrictionInfo = null;
            }

            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnUserUnmuted?.Invoke(inGroupChannel, member); });
            chatMainContextRef.CollectionManager.OnUserUnmuted(inGroupChannel, member);
        }

        private void OnReceiveBanChannelEvent(SbGroupChannel inGroupChannel, BanChannelEventData inBanEventData)
        {
            if (inGroupChannel == null || inBanEventData == null || inBanEventData.RestrictedUserDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveBanChannelEvent invalid params");
                return;
            }

            if (inGroupChannel.IsSuper)
            {
                inGroupChannel.UpdateMemberCount(inBanEventData);
            }
            else
            {
                inGroupChannel.RemoveMember(inBanEventData.RestrictedUserDto.UserId);
                inGroupChannel.UpdateJoinedMemberCount();
            }

            if (chatMainContextRef.CurrentUserId == inBanEventData.RestrictedUserDto.UserId)
            {
                inGroupChannel.MyMemberState = SbMemberState.None;
                inGroupChannel.InvitedAt = 0;
                inGroupChannel.ClearAllUnreadCount();
                RemoveCachedChannelIfContains(inGroupChannel.Url);
            }

            SbRestrictedUser restrictedUser = new SbRestrictedUser(inBanEventData.RestrictedUserDto, chatMainContextRef);
            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnUserBanned?.Invoke(inGroupChannel, restrictedUser); });
            chatMainContextRef.CollectionManager.OnUserBanned(inGroupChannel, restrictedUser);
        }

        private void OnReceiveUnbanChannelEvent(SbBaseChannel inBaseChannel, UnbanChannelEventData inUnbanEventData)
        {
            if (inBaseChannel == null || inUnbanEventData == null || inUnbanEventData.UserDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceiveUnbanChannelEvent invalid params");
                return;
            }

            SbUser unbannedUser = new SbUser(inUnbanEventData.UserDto, chatMainContextRef);
            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnUserUnbanned?.Invoke(inBaseChannel, unbannedUser); });
            chatMainContextRef.CollectionManager.OnUserUnbanned(inBaseChannel);
        }

        private void OnReceivePinnedMessageUpdatedChannelEvent(SbGroupChannel inGroupChannel, PinnedMessageUpdatedChannelEventData inPinnedMessageUpdatedEventData)
        {
            if (inGroupChannel == null || inPinnedMessageUpdatedEventData == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "GroupChannelManager::OnReceivePinnedMessageUpdatedChannelEvent invalid params");
                return;
            }

            if (inPinnedMessageUpdatedEventData.timestamp < inGroupChannel.PinnedMessageUpdatedAt)
                return;

            inGroupChannel.SetPinnedMessageIds(inPinnedMessageUpdatedEventData.PinnedMessageIds);
            if (inPinnedMessageUpdatedEventData.LastPinnedMessageDto != null)
            {
                inGroupChannel.LastPinnedMessage = inPinnedMessageUpdatedEventData.LastPinnedMessageDto.CreateMessageInstance(chatMainContextRef);
            }
            else
            {
                inGroupChannel.LastPinnedMessage = null;
            }

            inGroupChannel.PinnedMessageUpdatedAt = inPinnedMessageUpdatedEventData.timestamp;

            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnPinnedMessageUpdated?.Invoke(inGroupChannel); });
            chatMainContextRef.CollectionManager.OnPinnedMessageUpdated(inGroupChannel);

            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnChannelChanged?.Invoke(inGroupChannel); });
            chatMainContextRef.CollectionManager.OnChannelChanged(inGroupChannel);
        }
    }
}