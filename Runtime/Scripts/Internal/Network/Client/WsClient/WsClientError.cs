// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal sealed class WsClientError
    {
        private const int UNKNOWN_ERROR = 0;
        internal int SocketErrorCode { get; }
        internal string ErrorMessage { get; }

        internal WsClientError(string inErrorMessage, int inSocketErrorCode = UNKNOWN_ERROR)
        {
            SocketErrorCode = inSocketErrorCode;
            ErrorMessage = inErrorMessage;
        }
    }
}
