// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class DeliveryWsReceiveCommand : WsReceiveCommandAbstract
    {
        [JsonProperty("channel_url")] internal readonly string channelUrl = null;
        [JsonProperty("updated")] internal readonly Dictionary<string, long> updated = null;

        internal DeliveryWsReceiveCommand() : base(WsCommandType.Delivery) { }


        internal static DeliveryWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<DeliveryWsReceiveCommand>(inJsonString);
        }
    }
}