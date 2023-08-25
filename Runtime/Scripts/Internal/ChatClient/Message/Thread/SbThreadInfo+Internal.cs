// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbThreadInfo
    {
        private int _replyCount;
        private long _lastRepliedAt;
        private long _updatedAt;
        private List<SbUser> _mostRepliedUsers = new List<SbUser>();

        internal SbThreadInfo(SbThreadInfo inThreadInfo)
        {
            if (inThreadInfo != null)
            {
                _replyCount = inThreadInfo._replyCount;
                _lastRepliedAt = inThreadInfo._lastRepliedAt;
                _updatedAt = inThreadInfo._updatedAt;

                if (inThreadInfo._mostRepliedUsers != null && 0 < inThreadInfo._mostRepliedUsers.Count)
                {
                    _mostRepliedUsers.AddRange(inThreadInfo._mostRepliedUsers);
                }
            }
        }

        internal SbThreadInfo(ThreadInfoDto inThreadInfoDto, SendbirdChatMainContext inChatMainContext)
        {
            if (inThreadInfoDto != null)
            {
                _replyCount = inThreadInfoDto.replyCount;
                _lastRepliedAt = inThreadInfoDto.lastRepliedAt;
                _updatedAt = inThreadInfoDto.updatedAt;
                if (inThreadInfoDto.mostRepliedUsers != null && 0 < inThreadInfoDto.mostRepliedUsers.Count)
                {
                    foreach (UserDto userDto in inThreadInfoDto.mostRepliedUsers)
                    {
                        SbUser user = new SbUser(userDto, inChatMainContext);
                        _mostRepliedUsers.Add(user);
                    }
                }
            }
        }

        internal bool Merge(SbThreadInfo inThreadInfo)
        {
            if (inThreadInfo == null || inThreadInfo._updatedAt < _updatedAt)
                return false;

            _replyCount = inThreadInfo._replyCount;
            _lastRepliedAt = inThreadInfo._lastRepliedAt;
            _updatedAt = inThreadInfo._updatedAt;
            _mostRepliedUsers.Clear();

            if (inThreadInfo._mostRepliedUsers != null && 0 < inThreadInfo._mostRepliedUsers.Count)
            {
                _mostRepliedUsers.AddRange(inThreadInfo._mostRepliedUsers);
            }

            return true;
        }

        internal SbThreadInfo Clone()
        {
            return new SbThreadInfo(this);
        }
    }
}