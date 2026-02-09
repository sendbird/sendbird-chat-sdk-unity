// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UserMessageWsReceiveCommand : MessageWsReceiveCommandAbstract
    {
        internal UserMessageWsReceiveCommand() : base(WsCommandType.UserMessage) { }

        internal static UserMessageWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            JObject jObject = NewtonsoftJsonExtension.ParseToJObjectIgnoreException(inJsonString);
            if (jObject == null)
                return null;

            UserMessageWsReceiveCommand wsReceiveCommand = jObject.ToObjectIgnoreException<UserMessageWsReceiveCommand>();
            if (wsReceiveCommand != null)
            {
                wsReceiveCommand.BaseMessageDto = UserMessageDto.DeserializeFromJson(jObject);
            }

            return wsReceiveCommand;
        }
    }
}