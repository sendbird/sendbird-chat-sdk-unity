// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UpdateUserMessageWsReceiveCommand : UpdateMessageWsReceiveCommandAbstract
    {
        internal UpdateUserMessageWsReceiveCommand() : base(WsCommandType.UpdateUserMessage) { }

        internal static UpdateUserMessageWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            JObject jObject = NewtonsoftJsonExtension.ParseToJObjectIgnoreException(inJsonString);
            if (jObject == null)
                return null;

            UpdateUserMessageWsReceiveCommand wsReceiveCommand = jObject.ToObjectIgnoreException<UpdateUserMessageWsReceiveCommand>();
            if (wsReceiveCommand != null)
            {
                wsReceiveCommand.BaseMessageDto = UserMessageDto.DeserializeFromJson(jObject);
            }

            return wsReceiveCommand;
        }
    }
}