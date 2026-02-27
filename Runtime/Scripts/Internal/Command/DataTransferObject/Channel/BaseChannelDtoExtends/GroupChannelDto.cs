//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GroupChannelDto : BaseChannelDto
    {
        private string _countPreference;
        internal Dictionary<string, long> deliveryReceipt;
        private string _hiddenState;
        internal long invitedAt;
        internal UserDto inviter;
        internal bool isAccessCodeRequired;
        internal bool isBroadcast;
        internal bool isChatNotification;
        internal bool isConversation;
        internal bool isCreated;
        internal bool isDiscoverable;
        internal bool isDistinct;
        internal bool isExclusive;
        internal bool isHidden;
        private bool _isMuted;
        internal bool isPublic;
        internal bool isSuper;
        internal int joinedMemberCount;
        internal long joinedTs;
        private string _lastMessageJson;
        private string _latestPinnedMessageJson;
        internal int memberCount;
        private string _memberState;
        internal List<MemberDto> members;
        internal int messageSurvivalSeconds;
        private string _myRole;
        internal List<long> pinnedMessageIds;
        private string _pushTriggerOption;
        internal Dictionary<string, long> readReceipt;
        internal long tsMessageOffset;
        internal int unreadMentionCount;
        internal int unreadMessageCount;
        internal long userLastRead;

        internal SbCountPreference CountPreference { get; private set; } = SbCountPreference.All;
        internal SbGroupChannelHiddenState HiddenState { get; private set; } = SbGroupChannelHiddenState.Unhidden;
        internal SbRole MyRole { get; private set; } = SbRole.None;
        internal SbMemberState MemberState { get; private set; } = SbMemberState.None;
        internal SbGroupChannelPushTriggerOption GroupChannelPushTriggerOption { get; private set; } = SbGroupChannelPushTriggerOption.All;
        internal SbMutedState MutedState { get; private set; } = SbMutedState.Unmuted;
        internal BaseMessageDto LastMessage { get; private set; }
        internal BaseMessageDto LatestPinnedMessage { get; private set; }

        internal override bool TryReadSubclassField(JsonTextReader inReader, string inPropName)
        {
            switch (inPropName)
            {
                case "count_preference": _countPreference = JsonStreamingHelper.ReadString(inReader); return true;
                case "delivery_receipt": deliveryReceipt = JsonStreamingHelper.ReadStringLongDictionary(inReader); return true;
                case "hidden_state": _hiddenState = JsonStreamingHelper.ReadString(inReader); return true;
                case "invited_at": invitedAt = JsonStreamingHelper.ReadLong(inReader); return true;
                case "inviter": inviter = UserDto.ReadUserDtoFromJson(inReader); return true;
                case "is_access_code_required": isAccessCodeRequired = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_broadcast": isBroadcast = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_chat_notification": isChatNotification = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_conversation": isConversation = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_created": isCreated = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_discoverable": isDiscoverable = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_distinct": isDistinct = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_exclusive": isExclusive = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_hidden": isHidden = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_muted": _isMuted = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_public": isPublic = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_super": isSuper = JsonStreamingHelper.ReadBool(inReader); return true;
                case "joined_member_count": joinedMemberCount = JsonStreamingHelper.ReadInt(inReader); return true;
                case "joined_ts": joinedTs = JsonStreamingHelper.ReadLong(inReader); return true;
                case "last_message": _lastMessageJson = JsonStreamingHelper.ReadRawJsonString(inReader); return true;
                case "latest_pinned_message": _latestPinnedMessageJson = JsonStreamingHelper.ReadRawJsonString(inReader); return true;
                case "member_count": memberCount = JsonStreamingHelper.ReadInt(inReader); return true;
                case "member_state": _memberState = JsonStreamingHelper.ReadString(inReader); return true;
                case "members": members = MemberDto.ReadListFromJson(inReader); return true;
                case "message_survival_seconds": messageSurvivalSeconds = JsonStreamingHelper.ReadInt(inReader); return true;
                case "my_role": _myRole = JsonStreamingHelper.ReadString(inReader); return true;
                case "pinned_message_ids": pinnedMessageIds = JsonStreamingHelper.ReadLongList(inReader); return true;
                case "push_trigger_option": _pushTriggerOption = JsonStreamingHelper.ReadString(inReader); return true;
                case "read_receipt": readReceipt = JsonStreamingHelper.ReadStringLongDictionary(inReader); return true;
                case "ts_message_offset": tsMessageOffset = JsonStreamingHelper.ReadLong(inReader); return true;
                case "unread_mention_count": unreadMentionCount = JsonStreamingHelper.ReadInt(inReader); return true;
                case "unread_message_count": unreadMessageCount = JsonStreamingHelper.ReadInt(inReader); return true;
                case "user_last_read": userLastRead = JsonStreamingHelper.ReadLong(inReader); return true;
                default: return false;
            }
        }

        private void PostDeserialize()
        {
            if (string.IsNullOrEmpty(_countPreference) == false)
                CountPreference = SbCountPreferenceExtension.JsonNameToType(_countPreference);

            if (string.IsNullOrEmpty(_hiddenState) == false)
                HiddenState = SbGroupChannelHiddenStateExtension.JsonNameToType(_hiddenState);

            if (string.IsNullOrEmpty(_myRole) == false)
                MyRole = SbRoleExtension.JsonNameToType(_myRole);

            if (string.IsNullOrEmpty(_memberState) == false)
                MemberState = SbMemberStateExtension.JsonNameToType(_memberState);

            if (string.IsNullOrEmpty(_pushTriggerOption) == false)
                GroupChannelPushTriggerOption = SbGroupChannelPushTriggerOptionExtension.JsonNameToType(_pushTriggerOption);

            if (!string.IsNullOrEmpty(_lastMessageJson))
                LastMessage = BaseMessageDto.JsonStringToMessageDto(_lastMessageJson);

            if (!string.IsNullOrEmpty(_latestPinnedMessageJson))
                LatestPinnedMessage = BaseMessageDto.JsonStringToMessageDto(_latestPinnedMessageJson);

            MutedState = _isMuted ? SbMutedState.Muted : SbMutedState.Unmuted;
        }

        internal static List<GroupChannelDto> ReadListFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<GroupChannelDto> list = new List<GroupChannelDto>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                GroupChannelDto dto = ReadFromJson(inReader);
                if (dto != null)
                    list.Add(dto);
            }

            return list;
        }

        internal static GroupChannelDto DeserializeFromJson(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadFromJson);
        }

        internal static GroupChannelDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            GroupChannelDto dto = new GroupChannelDto();
            ReadBaseFields(inReader, dto);
            dto.PostDeserialize();
            return dto;
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            WriteBaseFields(inWriter);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "count_preference", _countPreference);
            JsonStreamingHelper.WriteStringLongDictionary(inWriter, "delivery_receipt", deliveryReceipt);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "hidden_state", _hiddenState);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "invited_at", invitedAt);
            if (inviter != null)
            {
                inWriter.WritePropertyName("inviter");
                inviter.WriteToJson(inWriter);
            }
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_access_code_required", isAccessCodeRequired);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_broadcast", isBroadcast);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_chat_notification", isChatNotification);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_conversation", isConversation);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_created", isCreated);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_discoverable", isDiscoverable);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_distinct", isDistinct);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_exclusive", isExclusive);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_hidden", isHidden);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_muted", _isMuted);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_public", isPublic);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_super", isSuper);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "joined_member_count", joinedMemberCount);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "joined_ts", joinedTs);
            JsonStreamingHelper.WriteRawJsonProperty(inWriter, "last_message", _lastMessageJson);
            JsonStreamingHelper.WriteRawJsonProperty(inWriter, "latest_pinned_message", _latestPinnedMessageJson);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "member_count", memberCount);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "member_state", _memberState);
            if (members != null)
            {
                inWriter.WritePropertyName("members");
                inWriter.WriteStartArray();
                foreach (MemberDto member in members)
                {
                    member.WriteToJson(inWriter);
                }
                inWriter.WriteEndArray();
            }
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "message_survival_seconds", messageSurvivalSeconds);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "my_role", _myRole);
            JsonStreamingHelper.WriteLongList(inWriter, "pinned_message_ids", pinnedMessageIds);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "push_trigger_option", _pushTriggerOption);
            JsonStreamingHelper.WriteStringLongDictionary(inWriter, "read_receipt", readReceipt);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "ts_message_offset", tsMessageOffset);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "unread_mention_count", unreadMentionCount);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "unread_message_count", unreadMessageCount);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "user_last_read", userLastRead);
            inWriter.WriteEndObject();
        }
    }
}
