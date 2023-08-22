// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class EnterChannelWsSendCommand : WsSendCommandAbstract
    {
        [JsonProperty("channel_url")] private readonly string _channelUrl = null;

        internal EnterChannelWsSendCommand(string inChannelUrl, WsSendCommandAbstract.AckHandler inAckHandler) : base(WsCommandType.EnterChannel, inAckHandler)
        {
            _channelUrl = inChannelUrl;
        }
    }
}