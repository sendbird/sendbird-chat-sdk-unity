// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    [Serializable]
    internal class DeletedChannelEventData : ChannelEventDataAbstract
    {
        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Deleted;

        internal static DeletedChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<DeletedChannelEventData>(inJsonString);
        }
    }
}