// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class SenderDto : UserDto
    {
#pragma warning disable CS0649
        [JsonProperty("is_blocked_by_me")] internal readonly bool? isBlockedByMe;
        [JsonProperty("role")] internal readonly string role;
#pragma warning restore CS0649
    }
}