// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class MetaCounterChangedChannelEventData : ChannelEventDataAbstract
    {
        [Serializable]
        private class DataProperties
        {
#pragma warning disable CS0649
            [JsonProperty("created")] internal readonly Dictionary<string, int> created;
            [JsonProperty("updated")] internal readonly Dictionary<string, int> updated;
            [JsonProperty("deleted")] internal readonly List<string> deleted;
#pragma warning restore CS0649
        }

        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.MetaCounterChanged;
        internal Dictionary<string, int> Created { get; private set; }
        internal Dictionary<string, int> Updated { get; private set; }
        internal List<string> Deleted { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                DataProperties dataProperties = base.data.ToObjectIgnoreException<DataProperties>();
                if (dataProperties != null)
                {
                    Created = dataProperties.created;
                    Updated = dataProperties.updated;
                    Deleted = dataProperties.deleted;
                }
            }
        }

        internal static MetaCounterChangedChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<MetaCounterChangedChannelEventData>(inJsonString);
        }
    }
}