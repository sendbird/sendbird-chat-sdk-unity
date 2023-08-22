// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class RoleChangeUserEventData : UserEventDataAbstract
    {
        [Serializable]
        private class DataProperties
        {
            [JsonProperty("channel_url")] internal readonly string channelUrl;
            [JsonProperty("my_role")] internal readonly string myRole;
        }

        internal override UserEventWsReceiveCommand.CategoryType CategoryType => UserEventWsReceiveCommand.CategoryType.RoleChange;

        internal string ChannelUrl { get; private set; }
        internal string MyRole { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                DataProperties dataProperties = base.data.ToObjectIgnoreException<DataProperties>();
                if (dataProperties != null)
                {
                    ChannelUrl = dataProperties.channelUrl;
                    MyRole = dataProperties.myRole;
                }
            }
        }

        internal static RoleChangeUserEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<RoleChangeUserEventData>(inJsonString);
        }
    }
}