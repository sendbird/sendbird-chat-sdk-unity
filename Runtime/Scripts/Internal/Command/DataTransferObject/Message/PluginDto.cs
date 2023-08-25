// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class PluginDto
    {
#pragma warning disable CS0649
        [JsonProperty("type")] internal readonly string type;
        [JsonProperty("vendor")] internal readonly string vendor;
        [JsonProperty("detail")] internal readonly Dictionary<string, string> detail;
#pragma warning restore CS0649
    }
}