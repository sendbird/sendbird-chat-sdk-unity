// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal class ReconnectionContext
    {
        internal const float DEFAULT_RETRY_BASE_INTERVAL = 3;
        internal const float DEFAULT_RETRY_MAX_INTERVAL = 24;
        internal const float DEFAULT_RETRY_INTERVAL_MULTIPLIER = 2;
        internal const int DEFAULT_RETRY_MAX_COUNT = 5;

        internal string WsHostUrl { get; private set; } = null;
        internal string UserId { get; private set; } = null;
        internal string AuthToken { get; private set; } = null;
        internal int ConnectionTimeoutDuration { get; private set; } = NetworkConfig.DEFAULT_CONNECTION_TIMEOUT_DURATION;
        internal string SessionKey { get; private set; } = null;

        internal float RetryBaseInterval { get; private set; } = DEFAULT_RETRY_BASE_INTERVAL;
        internal float RetryMaxInterval { get; private set; } = DEFAULT_RETRY_MAX_INTERVAL;
        internal float RetryIntervalMultiplier { get; private set; } = DEFAULT_RETRY_INTERVAL_MULTIPLIER;
        internal int RetryMaxCount { get; private set; } = DEFAULT_RETRY_MAX_COUNT;

        internal void SetConnectionTimeoutDuration(int inTimeoutDuration)
        {
            ConnectionTimeoutDuration = inTimeoutDuration;
        }

        internal void SetSessionKey(string inSessionKey)
        {
            SessionKey = inSessionKey;
        }

        internal void OnChangeReconnectionCommand(ReconnectionDto inReconnectionDto)
        {
            RetryMaxCount = inReconnectionDto.retryCount;
            RetryBaseInterval = inReconnectionDto.interval;
            RetryMaxInterval = inReconnectionDto.maxInterval;
            RetryIntervalMultiplier = inReconnectionDto.mul;
        }

        internal void SetWsHostUrl(string inWsUri)
        {
            WsHostUrl = inWsUri;
        }

        internal void SetUserId(string inUserId)
        {
            UserId = inUserId;
        }
        
        internal void SetAuthToken(string inAuthToken)
        {
            AuthToken = inAuthToken;
        }

        internal bool IsValidAuthTokenAndSessionKey()
        {
            return string.IsNullOrEmpty(SessionKey) == false && string.IsNullOrEmpty(AuthToken) == false;
        }
    }
}