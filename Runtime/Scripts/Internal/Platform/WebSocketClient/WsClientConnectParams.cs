// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal class WsClientConnectParams
    {
        //wss://ws-APPLICATION_ID.sendbird.com/?p=PLATFORM_NAME&user_id=USER_ID
        internal string Uri { get; }
        internal Dictionary<string, string> CustomHeaders { get; } = new Dictionary<string, string>();
        internal WsClientConnectParams(string inUri)
        {
            Uri = inUri;
        }
    }
}