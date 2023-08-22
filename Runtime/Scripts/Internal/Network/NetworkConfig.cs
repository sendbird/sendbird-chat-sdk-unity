// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal class NetworkConfig
    {
        internal const int DEFAULT_CONNECTION_TIMEOUT_DURATION = 10;
        internal const int DEFAULT_WEBSOCKET_RESPONSE_TIMEOUT_DURATION = 10;
        internal const int DEFAULT_SESSION_TOKEN_REFRESH_TIMEOUT_DURATION = 60;
        internal const int DEFAULT_CONNECTION_RETRY_MAX_COUNT = 5;
        internal const double DEFAULT_PING_INTERVAL = 15;
        internal const double DEFAULT_PONG_TIMEOUT = 5;
        
        internal int ConnectionTimeout { get; private set; } = NetworkConfig.DEFAULT_CONNECTION_TIMEOUT_DURATION;
        internal int WebsocketResponseTimeout { get; private set; } = NetworkConfig.DEFAULT_WEBSOCKET_RESPONSE_TIMEOUT_DURATION;
        internal int SessionTokenRefreshTimeout { get; private set; } = NetworkConfig.DEFAULT_SESSION_TOKEN_REFRESH_TIMEOUT_DURATION;
        
        internal static string GetDefaultApiHost(string inApplicationId)
        {
            return $"https://api-{inApplicationId}.sendbird.com";
        }
        
        internal static string GetDefaultWsHost(string inApplicationId)
        {
            return $"wss://ws-{inApplicationId}.sendbird.com";
        }
        
        internal void SetConnectionTimeout(int inTimeoutSec)
        {
            ConnectionTimeout = Math.Max(1, inTimeoutSec);
        }

        internal void SetWebSocketResponseTimeout(int inTimeoutSec)
        {
            WebsocketResponseTimeout = Math.Min(inTimeoutSec, 300);
            WebsocketResponseTimeout = Math.Max(5, inTimeoutSec);
        }
        
        internal void SetSessionTokenRefreshTimeout(int inTimeoutSec)
        {
            SessionTokenRefreshTimeout = Math.Min(inTimeoutSec, 1800);
            SessionTokenRefreshTimeout = Math.Max(60, inTimeoutSec);
        }
    }
}