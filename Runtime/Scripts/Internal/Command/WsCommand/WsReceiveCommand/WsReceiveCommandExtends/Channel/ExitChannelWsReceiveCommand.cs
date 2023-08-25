// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class ExitChannelWsReceiveCommand : WsReceiveCommandAbstract
    {
        [JsonProperty("channel_url")] internal readonly string channelUrl = null;
        [JsonProperty("participant_count")] internal readonly int participantCount;
        [JsonProperty("edge_ts")] internal readonly long edgeTimestamp;
        [JsonProperty("subchannel_id")] internal readonly string subChannelId;

        internal ExitChannelWsReceiveCommand() : base(WsCommandType.ExitChannel) { }

        internal static ExitChannelWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<ExitChannelWsReceiveCommand>(inJsonString);
        }
    }
}