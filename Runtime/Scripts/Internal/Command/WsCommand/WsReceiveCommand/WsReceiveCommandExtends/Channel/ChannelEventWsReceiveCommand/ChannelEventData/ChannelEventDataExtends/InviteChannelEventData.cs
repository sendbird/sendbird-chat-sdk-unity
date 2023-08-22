// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class InviteChannelEventData : ChannelEventDataAbstract
    {
        [Serializable]
        private class DataProperties
        {
            [JsonProperty("inviter")] internal readonly MemberDto inviter;
            [JsonProperty("invitees")] internal readonly List<MemberDto> invitees;
            [JsonProperty("invitedAt")] internal readonly long invitedAt;
        }

        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Invite;

        internal MemberDto InviterMemberDto { get; private set; }
        internal List<MemberDto> InviteeMemberDtos { get; private set; }
        internal long InvitedAt { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                DataProperties dataProperties = base.data.ToObjectIgnoreException<DataProperties>();
                if (dataProperties != null)
                {
                    InviterMemberDto = dataProperties.inviter;
                    InviteeMemberDtos = dataProperties.invitees;
                    InvitedAt = dataProperties.invitedAt;
                }
            }
        }

        internal static InviteChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<InviteChannelEventData>(inJsonString);
        }
    }
}