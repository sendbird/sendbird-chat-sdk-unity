// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal class AdminMessageWsReceiveCommand : MessageWsReceiveCommandAbstract
    {
        internal AdminMessageWsReceiveCommand() : base(WsCommandType.AdminMessage) { }

        internal static AdminMessageWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            JObject jObject = NewtonsoftJsonExtension.ParseToJObjectIgnoreException(inJsonString);
            if (jObject == null)
                return null;

            AdminMessageWsReceiveCommand wsReceiveCommand = jObject.ToObjectIgnoreException<AdminMessageWsReceiveCommand>();
            if (wsReceiveCommand != null)
            {
                wsReceiveCommand.BaseMessageDto = AdminMessageDto.DeserializeFromJson(jObject);
            }

            return wsReceiveCommand;
        }
    }
}