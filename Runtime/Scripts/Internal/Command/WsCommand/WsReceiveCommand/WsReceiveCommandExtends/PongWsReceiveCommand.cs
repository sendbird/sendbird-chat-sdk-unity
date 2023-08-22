// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class PongWsReceiveCommand : WsReceiveCommandAbstract
    {
        [JsonProperty("ts")] internal readonly long ts;
        [JsonProperty("sts")] internal readonly long sts;

        internal PongWsReceiveCommand() : base(WsCommandType.Pong) { }

        internal static PongWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<PongWsReceiveCommand>(inJsonString);
        }
    }
}