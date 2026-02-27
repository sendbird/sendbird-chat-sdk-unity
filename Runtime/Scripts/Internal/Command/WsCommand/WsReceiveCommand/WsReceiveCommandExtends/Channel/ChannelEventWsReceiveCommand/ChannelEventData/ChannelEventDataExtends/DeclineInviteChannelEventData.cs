// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal class DeclineInviteChannelEventData : ChannelEventDataAbstract
    {
        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.DeclineInvite;
        internal MemberDto InviterMemberDto { get; private set; }
        internal MemberDto InviteeMemberDto { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                JToken inviterToken = base.data["inviter"];
                if (inviterToken != null)
                    InviterMemberDto = MemberDto.ReadFromJsonString(inviterToken.ToString(Formatting.None));

                JToken inviteeToken = base.data["invitee"];
                if (inviteeToken != null)
                    InviteeMemberDto = MemberDto.ReadFromJsonString(inviteeToken.ToString(Formatting.None));
            }
        }

        internal static DeclineInviteChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<DeclineInviteChannelEventData>(inJsonString);
        }
    }
}