// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal static class PlatformModule
    {
        private static readonly IPlatformProvider _platformProvider = new UnityPlatformProvider();
        public static IPlatformProvider PlatformProvider => _platformProvider;
    }
}