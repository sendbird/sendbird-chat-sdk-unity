// 
//  Copyright (c) 2024 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal class UnityPlatformProvider : IPlatformProvider
    {
        private readonly IPlatformApplication _platformApplication = new UnityPlatformApplication();
        IPlatformApplication IPlatformProvider.PlatformApplication => _platformApplication;

        IPlatformApplication IPlatformProvider.CreateApplication()
        {
            return new UnityPlatformApplication();
        }

        IPlatformHttpClient IPlatformProvider.CreateHttpClient()
        {
            return new UnityPlatformHttpClient();
        }

        public IWebSocket CreateWebSocket()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return JsNativeWebSocketBridge.CreateWebSocketClient();
#else
            return new NetWebSocket();
#endif
        }

        IPlatformLogger IPlatformProvider.CreateLogger()
        {
            return new UnityPlatformLogger();
        }
    }
}