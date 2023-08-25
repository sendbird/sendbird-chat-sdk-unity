// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UpdateUserMessageWsReceiveCommand : UpdateMessageWsReceiveCommandAbstract
    {
        internal UpdateUserMessageWsReceiveCommand() : base(WsCommandType.UpdateUserMessage) { }

        internal static UpdateUserMessageWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            UpdateUserMessageWsReceiveCommand wsReceiveCommand = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<UpdateUserMessageWsReceiveCommand>(inJsonString);
            if (wsReceiveCommand != null)
            {
                wsReceiveCommand.BaseMessageDto = UserMessageDto.DeserializeFromJson(inJsonString);
            }

            return wsReceiveCommand;
        }
    }
}