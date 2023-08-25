// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal sealed class OpenChannelDto : BaseChannelDto
    {
#pragma warning disable CS0649
        [JsonProperty("operators")] internal readonly List<UserDto> operatorUserDtos;
        [JsonProperty("participant_count")] internal readonly int participantCount;
        [JsonProperty("max_length_message")] internal readonly long maxLengthMessage;
        [JsonProperty("is_dynamic_partitioned")] internal readonly bool isDynamicPartitioned;
#pragma warning restore CS0649

        internal static OpenChannelDto DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<OpenChannelDto>(inJsonString);
        }
    }
}