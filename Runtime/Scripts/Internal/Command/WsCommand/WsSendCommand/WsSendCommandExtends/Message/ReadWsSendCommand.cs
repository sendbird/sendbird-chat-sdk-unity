// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class ReadWsSendCommand : WsSendCommandAbstract
    {
        [JsonProperty("channel_url")] private readonly string _channelUrl = null;

        internal ReadWsSendCommand(string inReqId, string inChannelUrl, AckHandler inAckHandler) : base(WsCommandType.Read, inAckHandler, inReqId)
        {
            _channelUrl = inChannelUrl;
        }
    }
}