// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal class PinnedMessageUpdatedChannelEventData : ChannelEventDataAbstract
    {
        [Serializable]
        private class DataProperties
        {
#pragma warning disable CS0649
            [JsonProperty("pinned_message_ids")] internal readonly List<long> pinnedMessageIds;
            [JsonProperty("latest_pinned_message")] internal readonly JObject lastPinnedMessageJObject;
#pragma warning restore CS0649
        }

        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.PinnedMessageUpdated;
        internal List<long> PinnedMessageIds { get; private set; }
        internal BaseMessageDto LastPinnedMessageDto { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                DataProperties dataProperties = base.data.ToObjectIgnoreException<DataProperties>();
                if (dataProperties != null)
                {
                    PinnedMessageIds = dataProperties.pinnedMessageIds;
                    if (dataProperties.lastPinnedMessageJObject != null)
                        LastPinnedMessageDto = BaseMessageDto.JObjectToMessageDto(dataProperties.lastPinnedMessageJObject);
                }
            }
        }

        internal static PinnedMessageUpdatedChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<PinnedMessageUpdatedChannelEventData>(inJsonString);
        }
    }
}