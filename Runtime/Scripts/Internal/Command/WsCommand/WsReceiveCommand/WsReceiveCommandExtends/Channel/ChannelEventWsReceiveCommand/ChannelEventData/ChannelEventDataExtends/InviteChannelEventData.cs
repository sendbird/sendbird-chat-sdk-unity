// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal class InviteChannelEventData : ChannelEventDataAbstract
    {
        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Invite;

        internal MemberDto InviterMemberDto { get; private set; }
        internal List<MemberDto> InviteeMemberDtos { get; private set; }
        internal long InvitedAt { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                JToken inviterToken = base.data["inviter"];
                if (inviterToken != null)
                    InviterMemberDto = MemberDto.ReadFromJsonString(inviterToken.ToString(Formatting.None));

                JToken inviteesToken = base.data["invitees"];
                if (inviteesToken != null)
                    InviteeMemberDtos = MemberDto.ReadListFromJsonString(inviteesToken.ToString(Formatting.None));

                JToken invitedAtToken = base.data["invitedAt"];
                if (invitedAtToken != null)
                    InvitedAt = invitedAtToken.Value<long>();
            }
        }

        internal static InviteChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<InviteChannelEventData>(inJsonString);
        }
    }
}