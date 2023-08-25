// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UpdateFileMessageWsReceiveCommand : UpdateMessageWsReceiveCommandAbstract
    {
        internal UpdateFileMessageWsReceiveCommand() : base(WsCommandType.UpdateFileMessage) { }

        internal static UpdateFileMessageWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            UpdateFileMessageWsReceiveCommand wsReceiveCommand = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<UpdateFileMessageWsReceiveCommand>(inJsonString);
            if (wsReceiveCommand != null)
            {
                wsReceiveCommand.BaseMessageDto = FileMessageDto.DeserializeFromJson(inJsonString);
            }

            return wsReceiveCommand;
        }
    }
}