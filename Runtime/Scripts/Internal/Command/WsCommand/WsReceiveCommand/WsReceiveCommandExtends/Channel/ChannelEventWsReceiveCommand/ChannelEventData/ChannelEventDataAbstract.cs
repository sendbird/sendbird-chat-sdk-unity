// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal abstract class ChannelEventDataAbstract
    {
        [JsonProperty("channel_url")] internal readonly string channelUrl;
        [JsonProperty("channel_type")] private readonly string _channelType;
        [JsonProperty("is_super")] internal readonly bool isSuper;
        [JsonProperty("is_access_code_required")] internal readonly bool isAccessCodeRequired;
        [JsonProperty("ts")] internal readonly long timestamp;
        [JsonProperty("channel")] private readonly JObject _channelDto;
        [JsonProperty("member_count")] internal readonly int? memberCount;
        [JsonProperty("joined_member_count")] internal readonly int? joinedMemberCount;
        [JsonProperty("data")] protected readonly JObject data;
        internal abstract ChannelReceiveWsReceiveCommand.CategoryType CategoryType { get; }
        internal SbChannelType ChannelType { get; private set; }
        internal BaseChannelDto BaseChannelDto { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (string.IsNullOrEmpty(_channelType) == false)
                ChannelType = SbChannelTypeExtension.JsonNameToType(_channelType);

            BaseChannelDto = BaseChannelDto.ToChannelDtoByChannelType(_channelDto, ChannelType);
        }
    }
}