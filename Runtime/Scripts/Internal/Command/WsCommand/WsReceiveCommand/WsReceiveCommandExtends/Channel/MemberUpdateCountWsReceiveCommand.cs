// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class MemberUpdateCountWsReceiveCommand : WsReceiveCommandAbstract
    {
        [JsonProperty("group_channels")] internal readonly List<GroupChannelMemberCountDto> groupChannelMemberCountObjects = null;
        [JsonProperty("open_channels")] internal readonly List<OpenChannelMemberCountDto> openChannelMemberCountObjects = null;

        internal MemberUpdateCountWsReceiveCommand() : base(WsCommandType.MemberUpdateCount) { }

        internal static MemberUpdateCountWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<MemberUpdateCountWsReceiveCommand>(inJsonString);
        }
    }
}