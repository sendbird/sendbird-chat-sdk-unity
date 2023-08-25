// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;

namespace Sendbird.Chat
{
    [Serializable]
    internal class BanChannelEventData : ChannelEventDataAbstract
    {
        internal RestrictedUserDto RestrictedUserDto { get; private set; }

        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Ban;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                RestrictedUserDto = base.data.ToObjectIgnoreException<RestrictedUserDto>();
            }

            if (RestrictedUserDto != null)
            {
                RestrictedUserDto.RestrictionType = SbRestrictionType.Banned;
            }
        }


        internal static BanChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<BanChannelEventData>(inJsonString);
        }
    }
}