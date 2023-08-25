// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UnhiddenChannelEventData : ChannelEventDataAbstract
    {
        internal SbGroupChannelHiddenState HiddenState => SbGroupChannelHiddenState.Unhidden;
        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Unhidden;

        internal static UnhiddenChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<UnhiddenChannelEventData>(inJsonString);
        }
    }
}