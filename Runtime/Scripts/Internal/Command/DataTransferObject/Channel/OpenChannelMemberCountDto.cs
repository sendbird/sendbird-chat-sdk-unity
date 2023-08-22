// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal sealed class OpenChannelMemberCountDto
    {
#pragma warning disable CS0649
        [JsonProperty("channel_url")] internal readonly string channelUrl;
        [JsonProperty("ts")] internal readonly long timestamp;
        [JsonProperty("participant_count")] internal readonly int participantCount;
#pragma warning restore CS0649
    }
}