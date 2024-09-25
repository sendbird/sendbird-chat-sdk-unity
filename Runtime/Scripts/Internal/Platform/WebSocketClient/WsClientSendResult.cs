// 
//  Copyright (c) 2024 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal class WsClientSendResult
    {
        internal enum ResultType
        {
            Succeeded,
            CaughtException,
            Terminated,
        }
        
        internal const int UNKNOWN_NATIVE_ERROR_CODE = 0;
        internal WsClientSendResult(ResultType inResultType, string inResultMessage = "", int inNativeErrorCode = UNKNOWN_NATIVE_ERROR_CODE)
        {
            resultType = inResultType;
            resultMessage = inResultMessage;
            nativeErrorCode = inNativeErrorCode;
        }

        public readonly ResultType resultType;
        public readonly string resultMessage;
        public readonly int nativeErrorCode;
    }
}