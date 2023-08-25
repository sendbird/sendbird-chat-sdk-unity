// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal enum WsClientConnectResultType
    {
        Succeeded,
        CaughtException,
        Terminated,
        InvalidParams,
        Canceled
    }
}
