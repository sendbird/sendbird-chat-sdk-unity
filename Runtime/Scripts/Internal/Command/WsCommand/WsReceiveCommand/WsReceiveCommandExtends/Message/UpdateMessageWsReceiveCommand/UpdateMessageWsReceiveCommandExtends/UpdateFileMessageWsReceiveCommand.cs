// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UpdateFileMessageWsReceiveCommand : UpdateMessageWsReceiveCommandAbstract
    {
        internal UpdateFileMessageWsReceiveCommand() : base(WsCommandType.UpdateFileMessage) { }

        internal static UpdateFileMessageWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            JObject jObject = NewtonsoftJsonExtension.ParseToJObjectIgnoreException(inJsonString);
            if (jObject == null)
                return null;

            UpdateFileMessageWsReceiveCommand wsReceiveCommand = jObject.ToObjectIgnoreException<UpdateFileMessageWsReceiveCommand>();
            if (wsReceiveCommand != null)
            {
                wsReceiveCommand.BaseMessageDto = FileMessageDto.DeserializeFromJson(jObject);
            }

            return wsReceiveCommand;
        }
    }
}