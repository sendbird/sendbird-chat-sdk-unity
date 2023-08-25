// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UserMessageWsReceiveCommand : MessageWsReceiveCommandAbstract
    {
        internal UserMessageWsReceiveCommand() : base(WsCommandType.UserMessage) { }

        internal static UserMessageWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            UserMessageWsReceiveCommand wsReceiveCommand = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<UserMessageWsReceiveCommand>(inJsonString);
            if (wsReceiveCommand != null)
            {
                wsReceiveCommand.BaseMessageDto = UserMessageDto.DeserializeFromJson(inJsonString);
            }

            return wsReceiveCommand;
        }
    }
}