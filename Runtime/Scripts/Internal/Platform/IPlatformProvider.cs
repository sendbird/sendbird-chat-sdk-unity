// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal interface IPlatformProvider
    {
        IPlatformApplication PlatformApplication { get; }
        IPlatformApplication CreateApplication();
        IPlatformHttpClient CreateHttpClient();
        IWebSocket CreateWebSocket();
        IPlatformLogger CreateLogger();
    }
}