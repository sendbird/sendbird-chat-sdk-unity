// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal class WsClientReceiveResult
    {
        internal enum ResultType
        {
            Succeeded,
            CaughtException,
            Terminated,
            InvalidParams,
            Canceled
        }

        internal ResultType resultType;
        internal string receivedMessage;
        internal WsClientError clientError;
    }
}