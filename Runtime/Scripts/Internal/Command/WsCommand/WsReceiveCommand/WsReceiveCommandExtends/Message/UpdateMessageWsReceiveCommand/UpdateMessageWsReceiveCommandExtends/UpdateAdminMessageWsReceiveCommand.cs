// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UpdateAdminMessageWsReceiveCommand : UpdateMessageWsReceiveCommandAbstract
    {
        internal UpdateAdminMessageWsReceiveCommand() : base(WsCommandType.UpdateAdminMessage) { }

        internal static UpdateAdminMessageWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            UpdateAdminMessageWsReceiveCommand wsReceiveCommand = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<UpdateAdminMessageWsReceiveCommand>(inJsonString);
            if (wsReceiveCommand != null)
            {
                wsReceiveCommand.BaseMessageDto = AdminMessageDto.DeserializeFromJson(inJsonString);
            }

            return wsReceiveCommand;
        }
    }
}