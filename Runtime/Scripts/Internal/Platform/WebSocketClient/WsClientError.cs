// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal sealed class WsClientError
    {
        internal const int UNKNOWN_NATIVE_SOCKET_ERROR = 0;
        internal int NativeErrorCode { get; }
        internal string ErrorMessage { get; }
        internal WsClientErrorType ClientErrorType { get; }

        internal WsClientError(WsClientErrorType inClientErrorType, string inErrorMessage = "", int inNativeErrorCode = UNKNOWN_NATIVE_SOCKET_ERROR)
        {
            ClientErrorType = inClientErrorType;
            NativeErrorCode = inNativeErrorCode;
            ErrorMessage = inErrorMessage;
        }
    }
}
