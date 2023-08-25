// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class EnterChannelWsReceiveCommand : WsReceiveCommandAbstract
    {
        [JsonProperty("channel_url")] internal readonly string channelUrl = null;
        [JsonProperty("participant_count")] internal readonly int participantCount;
        [JsonProperty("enter_ts")] internal readonly long enterTimestamp;
        [JsonProperty("edge_ts")] internal readonly long edgeTimestamp;
        [JsonProperty("subchannel_id")] internal readonly string subChannelId;

        internal EnterChannelWsReceiveCommand() : base(WsCommandType.EnterChannel) { }

        internal static EnterChannelWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<EnterChannelWsReceiveCommand>(inJsonString);
        }
    }
}