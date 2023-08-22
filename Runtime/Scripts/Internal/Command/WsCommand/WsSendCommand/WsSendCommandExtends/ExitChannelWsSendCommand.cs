// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class ExitChannelWsSendCommand : WsSendCommandAbstract
    {
        [JsonProperty("channel_url")] private readonly string _channelUrl = null;

        internal ExitChannelWsSendCommand(string inChannelUrl, WsSendCommandAbstract.AckHandler inAckHandler) : base(WsCommandType.ExitChannel, inAckHandler)
        {
            _channelUrl = inChannelUrl;
        }
    }
}