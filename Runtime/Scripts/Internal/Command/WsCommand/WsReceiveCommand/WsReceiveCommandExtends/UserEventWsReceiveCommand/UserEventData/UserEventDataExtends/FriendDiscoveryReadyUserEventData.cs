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
    internal class FriendDiscoveryReadyUserEventData : UserEventDataAbstract
    {
        [Serializable]
        private class DataProperties
        {
            [JsonProperty("friend_discoveries")] internal readonly List<UserDto> friendDiscoveryUserDtos = null;
        }

        internal override UserEventWsReceiveCommand.CategoryType CategoryType => UserEventWsReceiveCommand.CategoryType.FriendDiscoveryReady;

        internal List<UserDto> FriendDiscoveryUserDtos { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                DataProperties dataProperties = base.data.ToObjectIgnoreException<DataProperties>();
                if (dataProperties != null)
                {
                    FriendDiscoveryUserDtos = dataProperties.friendDiscoveryUserDtos;
                }
            }
        }

        internal static FriendDiscoveryReadyUserEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<FriendDiscoveryReadyUserEventData>(inJsonString);
        }
    }
}