//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal abstract class UpdateMessageWsReceiveCommandAbstract : WsMessageReceiveCommandAbstract
    {
        private string _oldValuesMentionType;
        private List<string> _oldValuesMentionedUserIds;

        protected internal BaseMessageDto BaseMessageDto { get; protected set; }
        protected internal SbMentionType OldMentionType { get; protected set; }
        protected internal List<string> OldMentionedUserIds { get; protected set; }

        protected UpdateMessageWsReceiveCommandAbstract(WsCommandType inWsCommandType) : base(inWsCommandType) { }

        internal bool HasOldMentionType()
        {
            return !string.IsNullOrEmpty(_oldValuesMentionType);
        }

        internal bool HasChangedMentionTypeTo(SbMentionType inMentionType)
        {
            if (HasOldMentionType() && OldMentionType != BaseMessageDto?.MentionType && BaseMessageDto?.MentionType == inMentionType)
                return true;

            return false;
        }

        internal bool HasOldMentionedUsers()
        {
            return _oldValuesMentionedUserIds != null && 0 < _oldValuesMentionedUserIds.Count;
        }

        internal bool ContainsUserInOldMentionedUsers(string inUserId)
        {
            if (string.IsNullOrEmpty(inUserId) || _oldValuesMentionedUserIds == null)
                return false;

            return _oldValuesMentionedUserIds.Contains(inUserId);
        }

        internal void SetOldValues(string inMentionType, List<string> inMentionedUserIds)
        {
            _oldValuesMentionType = inMentionType;
            _oldValuesMentionedUserIds = inMentionedUserIds;

            if (string.IsNullOrEmpty(_oldValuesMentionType) == false)
                OldMentionType = SbMentionTypeExtension.JsonNameToType(_oldValuesMentionType);

            OldMentionedUserIds = _oldValuesMentionedUserIds;
        }
    }
}
