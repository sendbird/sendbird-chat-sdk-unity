// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class ReactionDto
    {
#pragma warning disable CS0649
        [JsonProperty("key")] internal readonly string key;
        [JsonProperty("user_ids")] internal readonly List<string> userIds;
        [JsonProperty("updated_at")] internal readonly long latestUpdatedAt;
#pragma warning restore CS0649
    }
}