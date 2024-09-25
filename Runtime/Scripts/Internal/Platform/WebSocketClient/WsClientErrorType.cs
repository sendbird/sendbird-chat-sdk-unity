// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal enum WsClientErrorType
    {
        Unknown,
        SocketClosed,
        CreateReceiveBufferFailed,
        CaughtException,
        ReceivedClose
    }
}
