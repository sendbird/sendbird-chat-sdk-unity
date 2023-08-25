// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal interface IWsClientEventListener
    {
        void OnErrorInOpenState(WsClientErrorType inErrorType, WsClientError inWsClientErrorNullable = null);
        void OnReceive(string inReceivedMessage);
    }
}