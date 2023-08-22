// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class LogiWsSendCommand : WsSendCommandAbstract
    {
        [JsonProperty("token")] private readonly string _token = null;
        [JsonProperty("expiring_session")] private readonly bool _expiringSession = false;

        internal LogiWsSendCommand(string inToken, bool inExpiringSession, WsSendCommandAbstract.AckHandler inAckHandler) : base(WsCommandType.Logi, inAckHandler)
        {
            _token = inToken;
            _expiringSession = inExpiringSession;
        }
    }
}