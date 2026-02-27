// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;

namespace Sendbird.Chat
{
    [Serializable]
    internal class ExitChannelEventData : ChannelEventDataAbstract
    {
        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Exit;
        internal UserDto UserDto { get; private set; }
        internal int ParticipantCount { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                UserDto = UserDto.ReadUserDtoFromJsonString(base.data.ToString(Newtonsoft.Json.Formatting.None));
                ParticipantCount = base.data.ToPropertyValueIgnoreException<int>("participant_count");
            }
        }

        internal static ExitChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<ExitChannelEventData>(inJsonString);
        }
    }
}