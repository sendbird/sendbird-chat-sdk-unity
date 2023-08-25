// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal abstract class WsMessageReceiveCommandAbstract : WsReceiveCommandAbstract
    {
        /// For more information, see [req_id vs request_id](https://sendbird.atlassian.net/wiki/spaces/SDK/pages/1723140230/req+id+vs+request+id)
        [JsonProperty("request_id")] private readonly string _requestId = null;

        internal string MessageCreatedRequestId => _requestId;

        protected WsMessageReceiveCommandAbstract(WsCommandType inWsCommandType) : base(inWsCommandType) { }
    }
}