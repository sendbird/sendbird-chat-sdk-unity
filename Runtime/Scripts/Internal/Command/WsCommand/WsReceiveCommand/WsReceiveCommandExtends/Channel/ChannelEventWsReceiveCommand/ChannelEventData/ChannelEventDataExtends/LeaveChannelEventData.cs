// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;

namespace Sendbird.Chat
{
    [Serializable]
    internal class LeaveChannelEventData : ChannelEventDataAbstract
    {
        internal MemberDto MemberDto { get; private set; }

        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Leave;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                MemberDto = MemberDto.ReadFromJsonString(base.data.ToString(Newtonsoft.Json.Formatting.None));
            }
        }

        internal static LeaveChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<LeaveChannelEventData>(inJsonString);
        }
    }
}