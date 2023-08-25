// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class TimeUtil
    {
        internal static long GetCurrentUnixTime()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
        
        internal static long GetCurrentUnixTimeMilliseconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
        
        internal static long SecondsToMilliseconds(float inSeconds)
        {
            return (long)(inSeconds * 1000);
        }
        
        internal static float MillisecondsToSeconds(long inMilliseconds)
        {
            return (float)(inMilliseconds * 0.001);
        }
    }
}