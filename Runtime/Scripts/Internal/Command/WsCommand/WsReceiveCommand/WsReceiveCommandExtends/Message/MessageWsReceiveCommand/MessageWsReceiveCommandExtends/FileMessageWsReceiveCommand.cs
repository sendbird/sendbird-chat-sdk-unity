// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    [Serializable]
    internal class FileMessageWsReceiveCommand : MessageWsReceiveCommandAbstract
    {
        internal FileMessageWsReceiveCommand() : base(WsCommandType.FileMessage) { }

        internal static FileMessageWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            FileMessageWsReceiveCommand wsReceiveCommand = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<FileMessageWsReceiveCommand>(inJsonString);
            if (wsReceiveCommand != null)
            {
                wsReceiveCommand.BaseMessageDto = FileMessageDto.DeserializeFromJson(inJsonString);
            }
            
            return wsReceiveCommand;
        }
    }
}