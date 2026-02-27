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
    internal class FriendDiscoveryReadyUserEventData : UserEventDataAbstract
    {
        internal override UserEventWsReceiveCommand.CategoryType CategoryType => UserEventWsReceiveCommand.CategoryType.FriendDiscoveryReady;

        internal List<UserDto> FriendDiscoveryUserDtos { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                JToken friendDiscoveriesToken = base.data["friend_discoveries"];
                if (friendDiscoveriesToken != null)
                {
                    FriendDiscoveryUserDtos = UserDto.ReadUserDtoListFromJsonString(friendDiscoveriesToken.ToString(Formatting.None));
                }
            }
        }

        internal static FriendDiscoveryReadyUserEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<FriendDiscoveryReadyUserEventData>(inJsonString);
        }
    }
}