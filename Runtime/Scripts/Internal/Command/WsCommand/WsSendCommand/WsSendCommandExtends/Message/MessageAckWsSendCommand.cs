// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class MessageAckWsSendCommand : WsSendCommandAbstract
    {
        [JsonProperty("channel_url")] private readonly string _channelUrl = null;
        [JsonProperty("msg_id")] private readonly long _msgId;

        internal MessageAckWsSendCommand(string inChannelUrl, long inMsgId) : base(WsCommandType.MessageAck)
        {
            _channelUrl = inChannelUrl;
            _msgId = inMsgId;
        }
    }
}