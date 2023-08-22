// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;

namespace Sendbird.Chat
{
    [Serializable]
    internal class TypingStartChannelEventData : ChannelEventDataAbstract
    {
        internal UserDto UserDto { get; private set; }

        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.TypingStart;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                UserDto = base.data.ToObjectIgnoreException<UserDto>();
            }
        }


        internal static TypingStartChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<TypingStartChannelEventData>(inJsonString);
        }
    }
}