// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UnmuteChannelEventData : ChannelEventDataAbstract
    {
        internal UserDto UserDto { get; private set; }

        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Unmute;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                UserDto = UserDto.ReadUserDtoFromJsonString(base.data.ToString(Newtonsoft.Json.Formatting.None));
            }
        }

        internal static UnmuteChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<UnmuteChannelEventData>(inJsonString);
        }
    }
}