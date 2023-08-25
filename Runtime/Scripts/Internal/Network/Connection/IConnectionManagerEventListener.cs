// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal interface IConnectionManagerEventListener
    {
        void OnChangeConnectionState(ConnectionStateInternalType inChangedStateType, ConnectionStateAbstract.ParamsAbstract inChangedStateParams);
    }
}