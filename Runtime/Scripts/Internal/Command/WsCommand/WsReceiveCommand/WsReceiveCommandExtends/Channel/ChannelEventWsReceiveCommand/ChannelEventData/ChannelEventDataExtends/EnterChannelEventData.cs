// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;

namespace Sendbird.Chat
{
    [Serializable]
    internal class EnterChannelEventData : ChannelEventDataAbstract
    {
        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Enter;
        internal UserDto UserDto { get; private set; }
        internal int ParticipantCount { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                UserDto = base.data.ToObjectIgnoreException<UserDto>();
                ParticipantCount = base.data.ToPropertyValueIgnoreException<int>("participant_count");
            }
        }

        internal static EnterChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<EnterChannelEventData>(inJsonString);
        }
    }
}