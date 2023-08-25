// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal enum WsClientCloseResultType
    {
        Succeeded,
        AlreadyClosingOrClosed,
        CaughtException,
        Terminated,
    }
}
