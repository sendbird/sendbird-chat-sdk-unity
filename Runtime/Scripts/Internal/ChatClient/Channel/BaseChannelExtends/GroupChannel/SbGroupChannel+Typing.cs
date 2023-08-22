// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbGroupChannel
    {
        private void StartTypingInternal()
        {
            long currentTimeMs = TimeUtil.GetCurrentUnixTimeMilliseconds();
            long typingIndicatorThrottleMs = TimeUtil.SecondsToMilliseconds(chatMainContextRef.TypingIndicatorThrottle);
            if (currentTimeMs < _startTypingLastSentAtMs + typingIndicatorThrottleMs)
                return;

            _startTypingLastSentAtMs = currentTimeMs;
            _endTypingLastSentAtMs = 0;
            TypingStartWsSendCommand wsSendCommand = new TypingStartWsSendCommand(Url, _startTypingLastSentAtMs);
            chatMainContextRef.CommandRouter.SendWsCommand(wsSendCommand);
        }

        private void EndTypingInternal()
        {
            long currentTimeMs = TimeUtil.GetCurrentUnixTimeMilliseconds();
            long typingIndicatorThrottleMs = TimeUtil.SecondsToMilliseconds(chatMainContextRef.TypingIndicatorThrottle);
            if (currentTimeMs < _endTypingLastSentAtMs + typingIndicatorThrottleMs)
                return;

            _startTypingLastSentAtMs = 0;
            _endTypingLastSentAtMs = currentTimeMs;
            TypingEndWsSendCommand wsSendCommand = new TypingEndWsSendCommand(Url, _endTypingLastSentAtMs);
            chatMainContextRef.CommandRouter.SendWsCommand(wsSendCommand);
        }

        private bool IsTypingInternal()
        {
            return 0 < _typingUsers.Count;
        }

        internal bool UpdateTypingStatus(SbUser inUser, bool inDidStart)
        {
            if (inUser == null)
                return false;

            bool isChanged = false;
            if (inDidStart)
            {
                if (_typingUserTimestampsByUserId.ContainsKey(inUser.UserId))
                {
                    _typingUserTimestampsByUserId[inUser.UserId] = TimeUtil.GetCurrentUnixTimeMilliseconds();
                }
                else
                {
                    _typingUserTimestampsByUserId.Add(inUser.UserId, TimeUtil.GetCurrentUnixTimeMilliseconds());
                    _typingUsers.Add(inUser);
                    isChanged = true;
                }
            }
            else
            {
                _typingUserTimestampsByUserId.RemoveIfContains(inUser.UserId);
                
                SbUser foundUser = _typingUsers.Find(inTypingUser => inTypingUser.UserId == inUser.UserId);
                if (foundUser != null)
                {
                    _typingUsers.Remove(foundUser);
                    isChanged = true;
                }
            }

            return isChanged;
        }

        internal bool RemoveInvalidateTypingStatus()
        {
            List<string> deleteUserIds = new List<string>();
            long currentTimeMilliseconds = TimeUtil.GetCurrentUnixTimeMilliseconds();
            long typingInvalidateTimeMs = TimeUtil.SecondsToMilliseconds(TYPING_INVALIDATE_TIME);

            foreach (KeyValuePair<string, long> keyValuePair in _typingUserTimestampsByUserId)
            {
                long typingStartTimeMilliseconds = keyValuePair.Value;
                if (typingStartTimeMilliseconds + typingInvalidateTimeMs <= currentTimeMilliseconds)
                    deleteUserIds.Add(keyValuePair.Key);
            }

            foreach (string userId in deleteUserIds)
            {
                _typingUserTimestampsByUserId.Remove(userId);
                SbUser foundUser = _typingUsers.Find(inUser => inUser.UserId == userId);
                if (foundUser != null)
                    _typingUsers.Remove(foundUser);
            }

            bool removed = 0 < deleteUserIds.Count;
            return removed;
        }
    }
}