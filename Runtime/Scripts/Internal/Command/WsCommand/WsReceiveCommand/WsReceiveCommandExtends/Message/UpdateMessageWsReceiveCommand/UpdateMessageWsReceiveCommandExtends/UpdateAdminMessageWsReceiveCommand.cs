// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UpdateAdminMessageWsReceiveCommand : UpdateMessageWsReceiveCommandAbstract
    {
        internal UpdateAdminMessageWsReceiveCommand() : base(WsCommandType.UpdateAdminMessage) { }

        internal static UpdateAdminMessageWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            JObject jObject = NewtonsoftJsonExtension.ParseToJObjectIgnoreException(inJsonString);
            if (jObject == null)
                return null;

            UpdateAdminMessageWsReceiveCommand wsReceiveCommand = jObject.ToObjectIgnoreException<UpdateAdminMessageWsReceiveCommand>();
            if (wsReceiveCommand != null)
            {
                wsReceiveCommand.BaseMessageDto = AdminMessageDto.DeserializeFromJson(jObject);
            }

            return wsReceiveCommand;
        }
    }
}