// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    public class ReconnectionDto
    {
#pragma warning disable CS0649
        [JsonProperty("interval")] internal readonly float interval;
        [JsonProperty("max_interval")] internal readonly float maxInterval;
        [JsonProperty("mul")] internal readonly int mul;
        [JsonProperty("retry_cnt")] internal readonly int retryCount;
#pragma warning restore CS0649
    }
}