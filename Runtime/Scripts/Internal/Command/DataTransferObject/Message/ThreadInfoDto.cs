// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class ThreadInfoDto
    {
#pragma warning disable CS0649
        [JsonProperty("most_replies")] internal readonly List<UserDto> mostRepliedUsers;
        [JsonProperty("last_replied_at")] internal readonly long lastRepliedAt;
        [JsonProperty("updated_at")] internal readonly long updatedAt;
        [JsonProperty("reply_count")] internal readonly int replyCount;
#pragma warning restore CS0649
    }
}