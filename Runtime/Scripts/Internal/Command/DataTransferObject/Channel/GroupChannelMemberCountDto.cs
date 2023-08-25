// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal sealed class GroupChannelMemberCountDto
    {
#pragma warning disable CS0649
        [JsonProperty("channel_url")] internal readonly string channelUrl;
        [JsonProperty("ts")] internal readonly long timestamp;
        [JsonProperty("joined_member_count")] internal readonly int joinedMemberCount;
        [JsonProperty("member_count")] internal readonly int memberCount;
#pragma warning restore CS0649
    }
}