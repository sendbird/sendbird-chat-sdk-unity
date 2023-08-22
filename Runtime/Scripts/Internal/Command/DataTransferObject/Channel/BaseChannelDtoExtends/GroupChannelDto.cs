// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal sealed class GroupChannelDto : BaseChannelDto
    {
#pragma warning disable CS0649
        [JsonProperty("count_preference")] private readonly string _countPreference;
        [JsonProperty("delivery_receipt")] internal readonly Dictionary<string, long> deliveryReceipt;
        [JsonProperty("hidden_state")] private readonly string _hiddenState;
        [JsonProperty("invited_at")] internal readonly long invitedAt;
        [JsonProperty("inviter")] internal readonly UserDto inviter;
        [JsonProperty("is_access_code_required")] internal readonly bool isAccessCodeRequired;
        [JsonProperty("is_broadcast")] internal readonly bool isBroadcast;
        [JsonProperty("is_chat_notification")] internal readonly bool isChatNotification;
        [JsonProperty("is_conversation")] internal readonly bool isConversation;
        [JsonProperty("is_created")] internal readonly bool isCreated;
        [JsonProperty("is_discoverable")] internal readonly bool isDiscoverable;
        [JsonProperty("is_distinct")] internal readonly bool isDistinct;
        [JsonProperty("is_exclusive")] internal readonly bool isExclusive;
        [JsonProperty("is_hidden")] internal readonly bool isHidden;
        [JsonProperty("is_muted")] private readonly bool _isMuted;
        [JsonProperty("is_public")] internal readonly bool isPublic;
        [JsonProperty("is_super")] internal readonly bool isSuper;
        [JsonProperty("joined_member_count")] internal readonly int joinedMemberCount;
        [JsonProperty("joined_ts")] internal readonly long joinedTs;
        [JsonProperty("last_message")] private readonly JObject _lastMessage;
        [JsonProperty("latest_pinned_message")] private readonly JObject _latestPinnedMessage;
        [JsonProperty("member_count")] internal readonly int memberCount;
        [JsonProperty("member_state")] private readonly string _memberState;
        [JsonProperty("members")] internal readonly List<MemberDto> members;
        [JsonProperty("message_survival_seconds")] internal readonly int messageSurvivalSeconds;
        [JsonProperty("my_role")] private readonly string _myRole;
        [JsonProperty("pinned_message_ids")] internal readonly List<long> pinnedMessageIds;
        [JsonProperty("push_trigger_option")] private readonly string _pushTriggerOption;
        [JsonProperty("read_receipt")] internal readonly Dictionary<string, long> readReceipt;
        [JsonProperty("ts_message_offset")] internal readonly long tsMessageOffset;
        [JsonProperty("unread_mention_count")] internal readonly int unreadMentionCount;
        [JsonProperty("unread_message_count")] internal readonly int unreadMessageCount;
        [JsonProperty("user_last_read")] internal readonly long userLastRead;
#pragma warning restore CS0649

        internal SbCountPreference CountPreference { get; private set; } = SbCountPreference.All;
        internal SbGroupChannelHiddenState HiddenState { get; private set; } = SbGroupChannelHiddenState.Unhidden;
        internal SbRole MyRole { get; private set; } = SbRole.None;
        internal SbMemberState MemberState { get; private set; } = SbMemberState.None;

        internal SbGroupChannelPushTriggerOption GroupChannelPushTriggerOption { get; private set; } = SbGroupChannelPushTriggerOption.All;
        internal SbMutedState MutedState { get; private set; } = SbMutedState.Unmuted;
        internal BaseMessageDto LastMessage { get; private set; }
        internal BaseMessageDto LatestPinnedMessage { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
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

            if (_lastMessage != null)
                LastMessage = BaseMessageDto.JObjectToMessageDto(_lastMessage);

            if (_latestPinnedMessage != null)
                LatestPinnedMessage = BaseMessageDto.JObjectToMessageDto(_latestPinnedMessage);


            MutedState = _isMuted ? SbMutedState.Muted : SbMutedState.Unmuted;
        }

        internal static GroupChannelDto DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<GroupChannelDto>(inJsonString);
        }
    }
}