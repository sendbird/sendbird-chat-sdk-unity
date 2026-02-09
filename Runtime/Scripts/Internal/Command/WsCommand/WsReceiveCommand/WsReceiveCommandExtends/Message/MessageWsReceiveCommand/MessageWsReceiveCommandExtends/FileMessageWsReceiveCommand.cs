// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal class FileMessageWsReceiveCommand : MessageWsReceiveCommandAbstract
    {
        internal FileMessageWsReceiveCommand() : base(WsCommandType.FileMessage) { }

        internal static FileMessageWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            JObject jObject = NewtonsoftJsonExtension.ParseToJObjectIgnoreException(inJsonString);
            if (jObject == null)
                return null;

            FileMessageWsReceiveCommand wsReceiveCommand = jObject.ToObjectIgnoreException<FileMessageWsReceiveCommand>();
            if (wsReceiveCommand != null)
            {
                wsReceiveCommand.BaseMessageDto = FileMessageDto.DeserializeFromJson(jObject);
            }

            return wsReceiveCommand;
        }
    }
}