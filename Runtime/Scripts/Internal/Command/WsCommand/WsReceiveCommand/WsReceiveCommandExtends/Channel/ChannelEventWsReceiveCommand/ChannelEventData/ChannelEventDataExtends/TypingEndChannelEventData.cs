// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;

namespace Sendbird.Chat
{
    [Serializable]
    internal class TypingEndChannelEventData : ChannelEventDataAbstract
    {
        internal UserDto UserDto { get; private set; }

        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.TypingEnd;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                UserDto = base.data.ToObjectIgnoreException<UserDto>();
            }
        }


        internal static TypingEndChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<TypingEndChannelEventData>(inJsonString);
        }
    }
}