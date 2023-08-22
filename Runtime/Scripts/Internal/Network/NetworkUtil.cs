// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class NetworkUtil
    {
        private static long _requestIdSeed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        internal static string GenerateReqId()
        {
            return (++_requestIdSeed).ToString();
        }
    }
}