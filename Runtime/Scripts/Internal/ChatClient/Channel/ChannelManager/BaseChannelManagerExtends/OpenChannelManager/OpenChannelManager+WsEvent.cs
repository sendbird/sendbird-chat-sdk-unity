// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal partial class OpenChannelManager
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

                case WsCommandType.MemberUpdateCount:
                    OnReceiveMemberUpdateCountWsEvent(inWsReceiveCommand);
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
                Logger.Warning(Logger.CategoryType.Channel, "OpenChannelManager::OnReceiveNewMessageWsEvent invalid params");
                return;
            }

            if (inMessageWsReceiveCommand.BaseMessageDto.ChannelType != SbChannelType.Open)
                return;

            void OnGetChannelCompletionHandler(SbOpenChannel inOpenChannel, bool inIsCached, SbError inError)
            {
                if (inError != null)
                {
                    Logger.Warning(Logger.CategoryType.Channel, $"OpenChannelManager::OnReceiveNewMessageWsEvent GetChannel Error:{inError.ErrorCode} Message:{inError.ErrorMessage}");
                    return;
                }

                SbBaseMessage message = inMessageWsReceiveCommand.BaseMessageDto.CreateMessageInstance(chatMainContextRef);

                if (inMessageWsReceiveCommand.IsCreatedFromCurrentDevice() == false)
                    channelHandlersById.ForEachByValue(inHandler => { inHandler.OnMessageReceived?.Invoke(inOpenChannel, message); });

                if (message.IsMentionedToMe(chatMainContextRef.CurrentUserId))
                    channelHandlersById.ForEachByValue(inHandler => { inHandler.OnMentionReceived?.Invoke(inOpenChannel, message); });
            }

            GetChannel(inMessageWsReceiveCommand.BaseMessageDto.channelUrl, inIsInternal: false, inIsForceRefresh: false, OnGetChannelCompletionHandler);
        }

        private void OnReceiveUpdateMessageWsEvent(UpdateMessageWsReceiveCommandAbstract inUpdateMessageWsReceiveCommand)
        {
            if (inUpdateMessageWsReceiveCommand == null || inUpdateMessageWsReceiveCommand.BaseMessageDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "OpenChannelManager::OnReceiveUpdateMessageWsEvent invalid params");
                return;
            }

            if (inUpdateMessageWsReceiveCommand.BaseMessageDto.ChannelType != SbChannelType.Open)
                return;

            void OnGetChannelCompletionHandler(SbOpenChannel inOpenChannel, bool inIsCached, SbError inError)
            {
                if (inError != null)
                {
                    Logger.Warning(Logger.CategoryType.Channel, $"OpenChannelManager::OnReceiveNewMessageWsEvent GetChannel Error:{inError.ErrorCode} Message:{inError.ErrorMessage}");
                    return;
                }

                SbBaseMessage message = inUpdateMessageWsReceiveCommand.BaseMessageDto.CreateMessageInstance(chatMainContextRef);

                channelHandlersById.ForEachByValue(inHandler => { inHandler.OnMessageUpdated?.Invoke(inOpenChannel, message); });
            }

            GetChannel(inUpdateMessageWsReceiveCommand.BaseMessageDto.channelUrl, inIsInternal: false, inIsForceRefresh: false, OnGetChannelCompletionHandler);
        }

        private void OnReceiveMemberUpdateCountWsEvent(WsReceiveCommandAbstract inWsReceiveCommand)
        {
            MemberUpdateCountWsReceiveCommand memberUpdateCountWsEvent = inWsReceiveCommand as MemberUpdateCountWsReceiveCommand;

            if (memberUpdateCountWsEvent == null || memberUpdateCountWsEvent.openChannelMemberCountObjects == null ||
                memberUpdateCountWsEvent.openChannelMemberCountObjects.Count <= 0)
                return;

            List<SbOpenChannel> changedOpenChannels = new List<SbOpenChannel>(memberUpdateCountWsEvent.openChannelMemberCountObjects.Count);

            foreach (OpenChannelMemberCountDto openChannelMemberCountCObject in memberUpdateCountWsEvent.openChannelMemberCountObjects)
            {
                SbOpenChannel cachedOpenChannel = FindCachedChannel(openChannelMemberCountCObject.channelUrl);
                if (cachedOpenChannel == null)
                    continue;

                cachedOpenChannel.ParticipantCount = openChannelMemberCountCObject.participantCount;
                changedOpenChannels.Add(cachedOpenChannel);
            }

            if (changedOpenChannels.Count <= 0)
                return;

            channelHandlersById.ForEachByValue((inHandler) => { inHandler.OnChannelParticipantCountChanged?.Invoke(changedOpenChannels); });
        }

        private void OnReceiveChannelEvent(WsReceiveCommandAbstract inWsReceiveCommand)
        {
            ChannelReceiveWsReceiveCommand channelReceiveWsCommand = inWsReceiveCommand as ChannelReceiveWsReceiveCommand;
            if (channelReceiveWsCommand == null || channelReceiveWsCommand.EventData == null || channelReceiveWsCommand.EventData.ChannelType != SbChannelType.Open)
                return;

            if (channelReceiveWsCommand.EventData.CategoryType == ChannelReceiveWsReceiveCommand.CategoryType.Deleted)
            {
                OnReceiveDeleteChannelEvent(channelReceiveWsCommand.EventData);
                return;
            }

            void OnGetChannelCompletionHandler(SbOpenChannel inOpenChannel, bool inIsCached, SbError inError)
            {
                if (inOpenChannel != null)
                {
                    ProcessChannelEvent(inOpenChannel, channelReceiveWsCommand.EventData);
                }
                else if (inError != null)
                {
                    Logger.Warning(Logger.CategoryType.Channel, $"OpenChannelManager::OnReceiveChannelEvent GetChannel Error:{inError.ErrorCode} Message:{inError.ErrorMessage}");
                }
            }

            bool isForceRefresh = channelReceiveWsCommand.EventData.CategoryType == ChannelReceiveWsReceiveCommand.CategoryType.PropChanged;
            GetChannel(channelReceiveWsCommand.EventData.channelUrl, inIsInternal: false, isForceRefresh, OnGetChannelCompletionHandler);
        }

        private void ProcessChannelEvent(SbOpenChannel inOpenChannel, ChannelEventDataAbstract inChannelEventData)
        {
            if (inOpenChannel == null || inChannelEventData == null || inChannelEventData.ChannelType != SbChannelType.Open)
                return;

            switch (inChannelEventData.CategoryType)
            {
                case ChannelReceiveWsReceiveCommand.CategoryType.Frozen:
                case ChannelReceiveWsReceiveCommand.CategoryType.Unfrozen:
                case ChannelReceiveWsReceiveCommand.CategoryType.MetaDataChanged:
                case ChannelReceiveWsReceiveCommand.CategoryType.MetaCounterChanged:
                    ProcessBaseChannelEvent(inOpenChannel, inChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Enter:
                    OnReceiveEnterChannelEvent(inOpenChannel, inChannelEventData as EnterChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Exit:
                    OnReceiveExitChannelEvent(inOpenChannel, inChannelEventData as ExitChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Mute:
                    OnReceiveMuteChannelEvent(inOpenChannel, inChannelEventData as MuteChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Unmute:
                    OnReceiveUnmuteChannelEvent(inOpenChannel, inChannelEventData as UnmuteChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Ban:
                    OnReceiveBanChannelEvent(inOpenChannel, inChannelEventData as BanChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Unban:
                    OnReceiveUnbanChannelEvent(inOpenChannel, inChannelEventData as UnbanChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.UpdateOperator:
                    OnReceiveUpdateOperatorChannelEvent(inOpenChannel, inChannelEventData as UpdateOperatorChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.PropChanged:
                    OnReceivePropChangeChannelEvent(inOpenChannel);
                    return;
            }
        }

        private void OnReceiveDeleteChannelEvent(ChannelEventDataAbstract inChannelEventData)
        {
            if (inChannelEventData == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "OpenChannelManager::OnReceiveDeleteChannelEvent invalid params");
                return;
            }

            RemoveCachedChannelIfContains(inChannelEventData.channelUrl);
            RemoveEnteredChannelIfContains(inChannelEventData.channelUrl);
            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnChannelDeleted?.Invoke(inChannelEventData.channelUrl, inChannelEventData.ChannelType); });
        }

        private void OnReceiveEnterChannelEvent(SbOpenChannel inOpenChannel, EnterChannelEventData inEnterEventData)
        {
            if (inOpenChannel == null || inEnterEventData == null || inEnterEventData.UserDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "OpenChannelManager::OnReceiveEnterChannelEvent invalid params");
                return;
            }

            inOpenChannel.ParticipantCount = inEnterEventData.ParticipantCount;
            SbUser user = new SbUser(inEnterEventData.UserDto, chatMainContextRef);
            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnUserEntered?.Invoke(inOpenChannel, user); });
            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnChannelParticipantCountChanged?.Invoke(new List<SbOpenChannel> { inOpenChannel }); });
        }

        private void OnReceiveExitChannelEvent(SbOpenChannel inOpenChannel, ExitChannelEventData inExitEventData)
        {
            if (inOpenChannel == null || inExitEventData == null || inExitEventData.UserDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "OpenChannelManager::OnReceiveExitChannelEvent invalid params");
                return;
            }

            inOpenChannel.ParticipantCount = inExitEventData.ParticipantCount;
            SbUser user = new SbUser(inExitEventData.UserDto, chatMainContextRef);
            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnUserExited?.Invoke(inOpenChannel, user); });
            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnChannelParticipantCountChanged?.Invoke(new List<SbOpenChannel> { inOpenChannel }); });
        }

        private void OnReceiveMuteChannelEvent(SbBaseChannel inBaseChannel, MuteChannelEventData inMuteEventData)
        {
            if (inBaseChannel == null || inMuteEventData == null || inMuteEventData.RestrictedUserDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "OpenChannelManager::OnReceiveMuteChannelEvent invalid params");
                return;
            }

            SbRestrictedUser restrictedUser = new SbRestrictedUser(inMuteEventData.RestrictedUserDto, chatMainContextRef);
            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnUserMuted?.Invoke(inBaseChannel, restrictedUser); });
        }

        private void OnReceiveUnmuteChannelEvent(SbBaseChannel inBaseChannel, UnmuteChannelEventData inUnmuteEventData)
        {
            if (inBaseChannel == null || inUnmuteEventData == null || inUnmuteEventData.UserDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "OpenChannelManager::OnReceiveMuteChannelEvent invalid params");
                return;
            }

            SbUser user = new SbUser(inUnmuteEventData.UserDto, chatMainContextRef);
            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnUserUnmuted?.Invoke(inBaseChannel, user); });
        }

        private void OnReceiveBanChannelEvent(SbBaseChannel inBaseChannel, BanChannelEventData inBanEventData)
        {
            if (inBaseChannel == null || inBanEventData == null || inBanEventData.RestrictedUserDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "OpenChannelManager::OnReceiveBanChannelEvent invalid params");
                return;
            }

            if (inBanEventData.RestrictedUserDto.UserId == chatMainContextRef.CurrentUserId)
            {
                RemoveCachedChannelIfContains(inBanEventData.channelUrl);
                RemoveEnteredChannelIfContains(inBanEventData.channelUrl);
            }

            SbRestrictedUser restrictedUser = new SbRestrictedUser(inBanEventData.RestrictedUserDto, chatMainContextRef);
            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnUserBanned?.Invoke(inBaseChannel, restrictedUser); });
        }

        private void OnReceiveUnbanChannelEvent(SbBaseChannel inBaseChannel, UnbanChannelEventData inUnbanEventData)
        {
            if (inBaseChannel == null || inUnbanEventData == null || inUnbanEventData.UserDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "OpenChannelManager::OnReceiveUnbanChannelEvent invalid params");
                return;
            }

            SbUser user = new SbUser(inUnbanEventData.UserDto, chatMainContextRef);
            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnUserUnbanned?.Invoke(inBaseChannel, user); });
        }

        private void OnReceivePropChangeChannelEvent(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "OpenChannelManager::OnReceivePropChangeChannelEvent invalid params");
                return;
            }

            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnChannelChanged?.Invoke(inBaseChannel); });
        }

        private void OnReceiveUpdateOperatorChannelEvent(SbOpenChannel inOpenChannel, UpdateOperatorChannelEventData inUpdateOperatorEventData)
        {
            if (inOpenChannel == null || inUpdateOperatorEventData == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "OpenChannelManager::OnReceiveUpdateOperatorChannelEvent invalid params");
                return;
            }

            List<SbUser> operatorUsers = null;
            if (inUpdateOperatorEventData.OperatorMemberDtos != null && 0 < inUpdateOperatorEventData.OperatorMemberDtos.Count)
            {
                operatorUsers = new List<SbUser>(inUpdateOperatorEventData.OperatorMemberDtos.Count);
                foreach (MemberDto memberDto in inUpdateOperatorEventData.OperatorMemberDtos)
                {
                    operatorUsers.Add(new SbUser(memberDto, chatMainContextRef));
                }
            }

            inOpenChannel.SetOperators(operatorUsers);

            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnOperatorUpdated?.Invoke(inOpenChannel); });
        }
    }
}