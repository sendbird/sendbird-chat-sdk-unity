// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UnblockUserEventData : UserEventDataAbstract
    {
        [Serializable]
        private class DataProperties
        {
            [JsonProperty("blocker")] internal readonly UserDto blocker = null;
            [JsonProperty("blockee")] internal readonly UserDto blockee = null;
        }

        internal override UserEventWsReceiveCommand.CategoryType CategoryType => UserEventWsReceiveCommand.CategoryType.Unblock;

        internal UserDto BlockerUserDto { get; private set; }
        internal UserDto BlockeeUserDto { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                DataProperties dataProperties = base.data.ToObjectIgnoreException<DataProperties>();
                if (dataProperties != null)
                {
                    BlockerUserDto = dataProperties.blocker;
                    BlockeeUserDto = dataProperties.blockee;
                }
            }
        }

        internal static UnblockUserEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<UnblockUserEventData>(inJsonString);
        }
    }
}