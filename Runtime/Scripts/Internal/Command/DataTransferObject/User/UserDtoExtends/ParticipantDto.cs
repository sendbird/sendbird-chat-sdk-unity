// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class ParticipantDto : UserDto
    {
#pragma warning disable CS0649
        [JsonProperty("is_muted")] internal readonly bool? isMuted;
#pragma warning restore CS0649
    }
}