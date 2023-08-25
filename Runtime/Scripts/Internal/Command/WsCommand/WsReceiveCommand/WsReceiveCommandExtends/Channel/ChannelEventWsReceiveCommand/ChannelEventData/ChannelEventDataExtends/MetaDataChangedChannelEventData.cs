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
    internal class MetaDataChangedChannelEventData : ChannelEventDataAbstract
    {
        [Serializable]
        private class DataProperties
        {
            [JsonProperty("created")] internal readonly Dictionary<string, string> created;
            [JsonProperty("updated")] internal readonly Dictionary<string, string> updated;
            [JsonProperty("deleted")] internal readonly List<string> deleted;
        }

        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.MetaDataChanged;
        internal Dictionary<string, string> Created { get; private set; }
        internal Dictionary<string, string> Updated { get; private set; }
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

        internal static MetaDataChangedChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<MetaDataChangedChannelEventData>(inJsonString);
        }
    }
}