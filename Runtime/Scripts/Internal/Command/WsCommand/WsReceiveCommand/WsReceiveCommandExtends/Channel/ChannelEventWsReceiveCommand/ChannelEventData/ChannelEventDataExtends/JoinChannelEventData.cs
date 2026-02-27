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
    internal class JoinChannelEventData : ChannelEventDataAbstract
    {
        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Join;

        internal List<MemberDto> MemberDtos { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                JToken usersToken = base.data["users"];
                if (usersToken != null)
                {
                    MemberDtos = MemberDto.ReadListFromJsonString(usersToken.ToString(Formatting.None));
                }

                if (MemberDtos == null)
                {
                    MemberDto member = MemberDto.ReadFromJsonString(base.data.ToString(Formatting.None));
                    MemberDtos = new List<MemberDto>(new MemberDto[] { member });
                }
            }
        }

        internal static JoinChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<JoinChannelEventData>(inJsonString);
        }
    }
}