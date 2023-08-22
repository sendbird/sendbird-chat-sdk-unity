// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class MemberDto : UserDto
    {
#pragma warning disable CS0649
        [JsonProperty("is_blocked_by_me")] internal readonly bool? isBlockedByMe;
        [JsonProperty("is_blocking_me")] internal readonly bool? isBlockingMe;
        [JsonProperty("is_muted")] internal readonly bool? isMuted;
        [JsonProperty("role")] internal readonly string role;
        [JsonProperty("state")] internal readonly string state;
#pragma warning restore CS0649
    }
}