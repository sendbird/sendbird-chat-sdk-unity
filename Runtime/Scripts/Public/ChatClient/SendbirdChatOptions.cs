// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents SendbirdChat options.
    /// </summary>
    /// @since 4.0.0
    public partial class SendbirdChatOptions
    {
        /// <summary>
        /// If set true, the sender information of sender of UserMessage or FileMessage such as nickname and profile url will be returned as the latest user’s. Otherwise, the information will be the value of the message creation time.
        /// </summary>
        /// <param name="inUse"></param>
        /// @since 4.0.0
        public void SetMemberInfoInMessage(bool inUse)
        {
            SetMemberInfoInMessageInternal(inUse);
        }

        /// <summary>
        /// Sets the timeout for connection. If there is a timeout error frequently, set the longer timeout than default value. The default is 10 seconds.
        /// </summary>
        /// <param name="inTimeout"></param>
        /// @since 4.0.0
        public void SetConnectionTimeout(int inTimeout)
        {
            SetConnectionTimeoutInternal(inTimeout);
        }

        /// <summary>
        /// Sets the websocket response timeout used in sending/receiving commands by websocket.The value should be between 5 seconds and 300 seconds (5 minutes). The default value is 10 seconds.
        /// </summary>
        /// <param name="inTimeout"></param>
        /// @since 4.0.0
        public void SetWebSocketResponseTimeout(int inTimeout)
        {
            SetWebSocketResponseTimeoutInternal(inTimeout);
        }

        /// <summary>
        /// Sets the timeout used in refreshing the session token when SessionHandler.onSessionTokenRequired is called. The value should be between 60 seconds and 1800 seconds (30 minutes). The default value is 60 seconds.
        /// </summary>
        /// <param name="inTimeout"></param>
        /// @since 4.0.0
        public void SetSessionTokenRefreshTimeout(int inTimeout)
        {
            SetSessionTokenRefreshTimeoutInternal(inTimeout);
        }

        /// <summary>
        /// Sets a term of typing indicator throttling in group channel. After this throttling interval from typing indicator started (or ended), You can re-start (or re-end) typing indicator. If you call start (or end) again in this interval, the call will be ignored.
        /// </summary>
        /// <param name="inInterval"></param>
        /// @since 4.0.0
        public void SetTypingIndicatorThrottle(int inInterval)
        {
            SetTypingIndicatorThrottleInternal(inInterval);
        }
    }
}