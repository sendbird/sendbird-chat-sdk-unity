// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;

namespace Sendbird.Chat
{
    [Serializable]
    internal class MuteChannelEventData : ChannelEventDataAbstract
    {
        internal RestrictedUserDto RestrictedUserDto { get; private set; }

        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Mute;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                RestrictedUserDto = RestrictedUserDto.ReadFromJsonString(base.data.ToString(Newtonsoft.Json.Formatting.None));
            }
        }


        internal static MuteChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<MuteChannelEventData>(inJsonString);
        }
    }
}