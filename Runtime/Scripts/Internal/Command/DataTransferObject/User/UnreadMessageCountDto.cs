// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class UnreadMessageCountDto
    {
#pragma warning disable CS0649
        [JsonProperty("ts")] internal readonly long timeStamp;
        [JsonProperty("all")] internal readonly int groupChannelCount;
        [JsonProperty("feed")] internal readonly int feedChannelCount;
        [JsonProperty("custom_types")] internal readonly Dictionary<string, int> customTypes;
#pragma warning restore CS0649
    }
}