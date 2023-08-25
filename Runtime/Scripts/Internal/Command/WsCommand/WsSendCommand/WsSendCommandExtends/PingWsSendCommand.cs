// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class PingWsSendCommand : WsSendCommandAbstract
    {
        [JsonProperty("active")] private readonly bool _isActive = false;

        internal PingWsSendCommand(bool inIsActive) : base(WsCommandType.Ping)
        {
            _isActive = inIsActive;
        }
    }
}