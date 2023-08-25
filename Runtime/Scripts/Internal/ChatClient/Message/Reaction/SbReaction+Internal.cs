// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbReaction
    {
        private readonly string _key;
        private long _updatedAt;
        private readonly List<string> _userIds = new List<string>();

        internal SbReaction(string inKey, string inUserId, long inUpdatedAt)
        {
            _key = inKey;
            _updatedAt = inUpdatedAt;
            if (string.IsNullOrEmpty(inUserId))
            {
                _userIds = new List<string>();
                _userIds.AddIfNotContains(inUserId);
            }
        }

        internal SbReaction(ReactionDto inReactionDto)
        {
            _key = inReactionDto.key;
            _updatedAt = inReactionDto.latestUpdatedAt;
            if (inReactionDto.userIds != null && 0 < inReactionDto.userIds.Count)
            {
                _userIds.AddRange(inReactionDto.userIds);
            }
        }

        internal bool ApplyReactionEvent(SbReactionEvent inReactionEvent)
        {
            if (inReactionEvent == null || inReactionEvent.Key != _key || string.IsNullOrEmpty(inReactionEvent.UserId))
                return false;

            if (inReactionEvent.Operation == SbReactionEventAction.Add)
            {
                _userIds.AddIfNotContains(inReactionEvent.UserId);
            }
            else if (inReactionEvent.Operation == SbReactionEventAction.Delete)
            {
                _userIds.RemoveIfContains(inReactionEvent.UserId);
            }

            if (_updatedAt < inReactionEvent.UpdatedAt)
                _updatedAt = inReactionEvent.UpdatedAt;

            return true;
        }

        internal int GetUserCount()
        {
            return _userIds.Count;
        }
    }
}