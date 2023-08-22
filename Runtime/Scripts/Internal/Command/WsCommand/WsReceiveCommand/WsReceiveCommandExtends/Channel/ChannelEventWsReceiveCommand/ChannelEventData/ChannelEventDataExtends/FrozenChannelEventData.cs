// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    [Serializable]
    internal class FrozenChannelEventData : ChannelEventDataAbstract
    {
        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Frozen;

        internal static FrozenChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<FrozenChannelEventData>(inJsonString);
        }
    }
}