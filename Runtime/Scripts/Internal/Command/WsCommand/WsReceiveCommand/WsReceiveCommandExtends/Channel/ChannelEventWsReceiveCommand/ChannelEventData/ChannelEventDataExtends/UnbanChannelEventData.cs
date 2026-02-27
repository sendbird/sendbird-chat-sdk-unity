// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UnbanChannelEventData : ChannelEventDataAbstract
    {
        internal UserDto UserDto { get; private set; }

        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Unban;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                UserDto = UserDto.ReadUserDtoFromJsonString(base.data.ToString(Newtonsoft.Json.Formatting.None));
            }
        }

        internal static UnbanChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<UnbanChannelEventData>(inJsonString);
        }
    }
}