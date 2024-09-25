// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class BlockUserEventData : UserEventDataAbstract
    {
        [Serializable]
        private class DataProperties
        {
#pragma warning disable CS0649
            [JsonProperty("blocker")] internal readonly UserDto blocker;
            [JsonProperty("blockee")] internal readonly UserDto blockee;
#pragma warning restore CS0649
        }

        internal override UserEventWsReceiveCommand.CategoryType CategoryType => UserEventWsReceiveCommand.CategoryType.Block;

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

        internal static BlockUserEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<BlockUserEventData>(inJsonString);
        }
    }
}