// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal abstract class UserEventDataAbstract
    {
        [JsonProperty("data")] protected readonly JObject data;
        [JsonProperty("ts")] protected readonly long timestamp;
        internal abstract UserEventWsReceiveCommand.CategoryType CategoryType { get; }
    }
}