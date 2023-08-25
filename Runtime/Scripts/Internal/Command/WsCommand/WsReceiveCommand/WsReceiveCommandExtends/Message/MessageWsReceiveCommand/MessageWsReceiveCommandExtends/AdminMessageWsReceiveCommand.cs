// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    [Serializable]
    internal class AdminMessageWsReceiveCommand : MessageWsReceiveCommandAbstract
    {
        internal AdminMessageWsReceiveCommand() : base(WsCommandType.AdminMessage) { }

        internal static AdminMessageWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            AdminMessageWsReceiveCommand wsReceiveCommand = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<AdminMessageWsReceiveCommand>(inJsonString);
            if (wsReceiveCommand != null)
            {
                wsReceiveCommand.BaseMessageDto = AdminMessageDto.DeserializeFromJson(inJsonString);
            }

            return wsReceiveCommand;
        }
    }
}