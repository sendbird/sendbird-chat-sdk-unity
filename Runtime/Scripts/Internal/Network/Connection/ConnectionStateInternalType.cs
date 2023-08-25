// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal enum ConnectionStateInternalType
    {
        Initialized = 0,
        Connecting,
        Connected,
        Reconnecting,
        Disconnected,
        Logout,
        Max,
        None = Max,
    }
}
