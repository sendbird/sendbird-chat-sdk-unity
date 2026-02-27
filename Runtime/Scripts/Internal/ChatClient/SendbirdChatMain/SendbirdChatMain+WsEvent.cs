// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal partial class SendbirdChatMain
    {
        private void OnReceiveUserEventWsEvent(UserEventWsReceiveCommand inUserEventReceiveCommand)
        {
            if (_userEventHandlersById == null || _userEventHandlersById.Count <= 0)
                return;

            if (inUserEventReceiveCommand == null || inUserEventReceiveCommand.EventData == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "SendbirdChatMain::OnReceiveUserEventWsEvent invalid params");
                return;
            }

            if (inUserEventReceiveCommand.EventData.CategoryType != UserEventWsReceiveCommand.CategoryType.FriendDiscoveryReady)
                return;


            FriendDiscoveryReadyUserEventData userEventData = inUserEventReceiveCommand.EventData as FriendDiscoveryReadyUserEventData;
            if (userEventData == null)
            {
                Logger.Warning(Logger.CategoryType.Command, "SendbirdChatMain::OnReceiveUserEventWsEvent FriendDiscoveryReadyUserEventData is null.");
                return;
            }

            if (userEventData.FriendDiscoveryUserDtos == null || userEventData.FriendDiscoveryUserDtos.Count <= 0)
                return;

            List<SbUser> users = new List<SbUser>(userEventData.FriendDiscoveryUserDtos.Count);
            foreach (UserDto userDto in userEventData.FriendDiscoveryUserDtos)
            {
                users.Add(new SbUser(userDto, ChatMainContext));
            }

            _userEventHandlersById.ForEachByValue(inEventHandler => { inEventHandler.OnFriendsDiscovered?.Invoke(users); });
        }

        void ICommandRouterEventListener.OnWsError(SbError inError)
        {
            ChatMainContext.ConnectionManager.OnWsError(inError);
        }

        void ICommandRouterEventListener.OnReceiveWsEventCommand(WsReceiveCommandAbstract inWsReceiveCommand)
        {
            if (inWsReceiveCommand == null)
                return;

            if (inWsReceiveCommand.unreadMessageCountDto != null)
            {
                bool changed = ChatMainContext.UnreadMessageCount.UpdateFrom(inWsReceiveCommand.unreadMessageCountDto);
                if (changed)
                {
                    _userEventHandlersById.ForEachByValue(inEventHandler => { inEventHandler.OnTotalUnreadMessageCountChanged?.Invoke(ChatMainContext.UnreadMessageCount); });
                }
            }

            if (inWsReceiveCommand.CommandType == WsCommandType.UserEvent)
            {
                OnReceiveUserEventWsEvent(inWsReceiveCommand as UserEventWsReceiveCommand);
            }

            ChatMainContext.SessionManager.OnReceiveWsEventCommand(inWsReceiveCommand);
            ChatMainContext.ConnectionManager.OnReceiveWsEventCommand(inWsReceiveCommand);
            ChatMainContext.OpenChannelManager.OnReceiveWsEventCommand(inWsReceiveCommand);
            JsonMemoryProfiler.TakeSnapshot("WsEvent:BeforeGroupChannelManager");
            ChatMainContext.GroupChannelManager.OnReceiveWsEventCommand(inWsReceiveCommand);
            JsonMemoryProfiler.TakeSnapshot("WsEvent:AfterGroupChannelManager");
        }
    }
}