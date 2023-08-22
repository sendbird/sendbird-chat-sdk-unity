// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SendbirdChatOptions
    {
        private readonly SendbirdChatMain _chatMainRef = null;

        internal SendbirdChatOptions(SendbirdChatMain inSendbirdChatMain)
        {
            _chatMainRef = inSendbirdChatMain;
        }

        private void SetMemberInfoInMessageInternal(bool inUse)
        {
            _chatMainRef.ChatMainContext.SetMemberInfoInMessage(inUse);
        }

        private void SetConnectionTimeoutInternal(int inTimeoutSec)
        {
            _chatMainRef.ChatMainContext.NetworkConfig.SetConnectionTimeout(inTimeoutSec);
        }

        private void SetWebSocketResponseTimeoutInternal(int inTimeoutSec)
        {
            _chatMainRef.ChatMainContext.NetworkConfig.SetWebSocketResponseTimeout(inTimeoutSec);
        }

        private void SetSessionTokenRefreshTimeoutInternal(int inTimeoutSec)
        {
            _chatMainRef.ChatMainContext.NetworkConfig.SetSessionTokenRefreshTimeout(inTimeoutSec);
        }

        private void SetTypingIndicatorThrottleInternal(int inIntervalSec)
        {
            _chatMainRef.ChatMainContext.SetTypingIndicatorThrottle(inIntervalSec);
        }
    }
}