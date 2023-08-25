// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal enum WsClientErrorType
    {
        SocketClosed,
        CreateReceiveBufferFailed,
        CaughtException,
        ReceivedClose,
    }
}
