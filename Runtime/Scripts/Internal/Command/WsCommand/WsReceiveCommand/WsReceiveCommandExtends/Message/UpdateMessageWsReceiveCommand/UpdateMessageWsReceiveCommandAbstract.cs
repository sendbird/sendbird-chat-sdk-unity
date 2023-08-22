// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal abstract class UpdateMessageWsReceiveCommandAbstract : WsMessageReceiveCommandAbstract
    {
        [Serializable]
        internal class OldValues
        {
            [JsonProperty("mention_type")] internal readonly string mentionType = null;
            [JsonProperty("mentioned_user_ids")] internal readonly List<string> mentionedUserIds = null;
        }

        [JsonProperty("old_values")] private readonly OldValues _oldValues = null;

        protected internal BaseMessageDto BaseMessageDto { get; protected set; }
        protected internal SbMentionType OldMentionType { get; protected set; }
        protected internal List<string> OldMentionedUserIds { get; protected set; }

        protected UpdateMessageWsReceiveCommandAbstract(WsCommandType inWsCommandType) : base(inWsCommandType) { }

        internal bool HasOldMentionType()
        {
            return !string.IsNullOrEmpty(_oldValues?.mentionType);
        }

        internal bool HasChangedMentionTypeTo(SbMentionType inMentionType)
        {
            if (HasOldMentionType() && OldMentionType != BaseMessageDto?.MentionType && BaseMessageDto?.MentionType == inMentionType)
                return true;

            return false;
        }

        internal bool HasOldMentionedUsers()
        {
            return _oldValues?.mentionedUserIds != null && 0 < _oldValues?.mentionedUserIds.Count;
        }

        internal bool ContainsUserInOldMentionedUsers(string inUserId)
        {
            if (string.IsNullOrEmpty(inUserId) || _oldValues == null || _oldValues.mentionedUserIds == null)
                return false;

            return _oldValues.mentionedUserIds.Contains(inUserId);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (_oldValues != null)
            {
                if (string.IsNullOrEmpty(_oldValues.mentionType) == false)
                    OldMentionType = SbMentionTypeExtension.JsonNameToType(_oldValues.mentionType);

                OldMentionedUserIds = _oldValues.mentionedUserIds;
            }
        }
    }
}