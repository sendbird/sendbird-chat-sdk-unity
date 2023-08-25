// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class DeclineInviteChannelEventData : ChannelEventDataAbstract
    {
        [Serializable]
        private class DataProperties
        {
            [JsonProperty("inviter")] internal readonly MemberDto inviter;
            [JsonProperty("invitee")] internal readonly MemberDto invitee;
        }

        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.DeclineInvite;
        internal MemberDto InviterMemberDto { get; private set; }
        internal MemberDto InviteeMemberDto { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                DataProperties dataProperties = base.data.ToObjectIgnoreException<DataProperties>();
                if (dataProperties != null)
                {
                    InviterMemberDto = dataProperties.inviter;
                    InviteeMemberDto = dataProperties.invitee;
                }
            }
        }

        internal static DeclineInviteChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<DeclineInviteChannelEventData>(inJsonString);
        }
    }
}