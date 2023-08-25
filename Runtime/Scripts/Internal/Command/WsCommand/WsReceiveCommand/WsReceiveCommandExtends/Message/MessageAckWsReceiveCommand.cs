// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class MessageAckWsReceiveCommand : WsReceiveCommandAbstract
    {
        [JsonProperty("channel_url")] internal readonly string channelUrl = null;
        [JsonProperty("msg_id")] internal readonly long msgId;

        internal MessageAckWsReceiveCommand() : base(WsCommandType.MessageAck) { }

        internal static MessageAckWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<MessageAckWsReceiveCommand>(inJsonString);
        }
    }
}