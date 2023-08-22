// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class TypingStartWsSendCommand : WsSendCommandAbstract
    {
        [JsonProperty("channel_url")] private readonly string _channelUrl = null;
        [JsonProperty("time")] private readonly long _time;

        internal TypingStartWsSendCommand(string inChannelUrl, long inTime) : base(WsCommandType.TypingStart)
        {
            _channelUrl = inChannelUrl;
            _time = inTime;
        }
    }
}