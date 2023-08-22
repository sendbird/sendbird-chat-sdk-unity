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
    internal abstract class BaseMessageDto
    {
#pragma warning disable CS0649
        // For more information, see [req_id vs request_id](https://sendbird.atlassian.net/wiki/spaces/SDK/pages/1723140230/req+id+vs+request+id)
        // req_id is the same as the request_id of the message when received via the API.
        // When receiving a req_id over WebSocket, it's the same as the req_id in WsReceiveCommand and is used to verify that the current device generated the message.
        [JsonProperty("req_id")] private readonly string _messageReqId;

        // request_id is only used in WebSocket.
        [JsonProperty("request_id")] private readonly string _messageRequestId;
        [JsonProperty("apple_critical_alert_options")] internal readonly AppleCriticalAlertOptionsDto appleCriticalAlertOptionsDto;
        [JsonProperty("auto_resend_registered")] internal readonly bool autoResendRegistered;
        [JsonProperty("channel_type")] private readonly string _channelType;
        [JsonProperty("channel_url")] internal readonly string channelUrl;
        [JsonProperty("check_reactions")] internal readonly bool checkReactions;
        [JsonProperty("created_at")] private readonly long? _createdAt;
        [JsonProperty("ts")] private readonly long? _timestamp;
        [JsonProperty("custom_type")] internal readonly string customType;
        [JsonProperty("data")] private readonly string _data;
        [JsonProperty("custom")] private readonly string _custom = null;
        [JsonProperty("error_code")] internal readonly int errorCode;
        [JsonProperty("extended_message")] internal readonly Dictionary<string, string> extendedMessage;
        [JsonProperty("force_update_last_message")] internal readonly bool forceUpdateLastMessage;
        [JsonProperty("is_global_block")] internal readonly bool isGlobalBlocked;
        [JsonProperty("is_op_msg")] internal readonly bool isOperatorMessage;
        [JsonProperty("is_reply_to_channel")] internal readonly bool isReplyToChannel;
        [JsonProperty("is_guest_msg")] internal readonly bool isGuestMsg;
        [JsonProperty("is_super")] internal readonly bool isSuper;
        [JsonProperty("mention_type")] private readonly string _mentionType;
        [JsonProperty("mentioned_message_template")] internal readonly string mentionedMessageTemplate;
        [JsonProperty("mentioned_users")] internal readonly List<UserDto> mentionedUserDtos;
        [JsonProperty("message")] internal readonly string message;
        [JsonProperty("message_events")] internal readonly MessageEventsDto messageEventsDto;
        [JsonProperty("message_id")] private readonly long? _messageId;
        [JsonProperty("message_survival_seconds")] internal readonly int messageSurvivalSeconds;
        [JsonProperty("metaarray")] private readonly Dictionary<string, List<string>> _metaArray;
        [JsonProperty("metaarray_key_order")] private readonly List<string> _metaArrayKeyOrder;
        [JsonProperty("msg_id")] private readonly long? _msgId;
        [JsonProperty("og_tag")] internal readonly OgMetaDataDto ogMetaDataDto;
        [JsonProperty("parent_message_id")] internal readonly long? parentMessageId;
        [JsonProperty("parent_message_info")] private readonly JObject _parentMessageInfo;
        [JsonProperty("reactions")] internal readonly List<ReactionDto> reactionDtos;
        [JsonProperty("scheduled_at")] internal readonly long scheduledAt;
        [JsonProperty("scheduled_message_id")] internal readonly long scheduledMessageId;
        [JsonProperty("silent")] internal readonly bool silent;
        [JsonProperty("sorted_metaarray")] private readonly List<MessageMetaArrayDto> _sortedMetaArrayDtos;
        [JsonProperty("thread_info")] internal readonly ThreadInfoDto threadInfoDto;
        [JsonProperty("updated_at")] internal readonly long updatedAt;
        [JsonProperty("user")] internal readonly SenderDto senderDto;
#pragma warning restore CS0649

        internal SbChannelType ChannelType { get; private set; } = SbChannelType.Group;
        internal SbMentionType MentionType { get; private set; } = SbMentionType.Users;
        internal SbSendingStatus RequestState { get; private set; } = SbSendingStatus.None;
        internal string MessageCreatedRequestId { get; private set; }
        internal long MessageId { get; private set; }
        internal long CreatedAt { get; private set; }
        internal string Data { get; private protected set; } = null;
        internal List<MessageMetaArrayDto> MetaArrayDtos { get; private set; }
        internal BaseMessageDto ParentMessageInfoDto { get; private set; }


        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (string.IsNullOrEmpty(_channelType) == false)
                ChannelType = SbChannelTypeExtension.JsonNameToType(_channelType);

            if (string.IsNullOrEmpty(_mentionType) == false)
                MentionType = SbMentionTypeExtension.JsonNameToType(_mentionType);

            // Received from WebSocket
            MessageCreatedRequestId = _messageRequestId;
            if (string.IsNullOrEmpty(MessageCreatedRequestId))
            {
                // Received from API
                MessageCreatedRequestId = _messageReqId;
            }

            CreatedAt = _timestamp ?? _createdAt ?? 0;
            MessageId = _messageId ?? _msgId ?? SbBaseMessage.INVALID_MESSAGE_ID_MIN;
            RequestState = SbSendingStatus.Succeeded;

            if (string.IsNullOrEmpty(Data))
            {
                Data = string.IsNullOrEmpty(_data) == false ? _data : _custom;
            }

            if (_parentMessageInfo != null)
                ParentMessageInfoDto = BaseMessageDto.JObjectToMessageDto(_parentMessageInfo);

            if (_sortedMetaArrayDtos != null && 0 < _sortedMetaArrayDtos.Count)
            {
                MetaArrayDtos = new List<MessageMetaArrayDto>(_sortedMetaArrayDtos);
            }
            else if (_metaArray != null && 0 < _metaArray.Count)
            {
                MetaArrayDtos = new List<MessageMetaArrayDto>(_metaArray.Count);
                if (_metaArrayKeyOrder != null && 0 < _metaArrayKeyOrder.Count)
                {
                    foreach (string key in _metaArrayKeyOrder)
                    {
                        if (_metaArray.ContainsKey(key) == false)
                            continue;

                        MetaArrayDtos.Add(new MessageMetaArrayDto(key, _metaArray[key]));
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, List<string>> metaArrayKeyValuePair in _metaArray)
                    {
                        MetaArrayDtos.Add(new MessageMetaArrayDto(metaArrayKeyValuePair.Key, metaArrayKeyValuePair.Value));
                    }
                }
            }
        }

        internal virtual SbBaseMessage CreateMessageInstance(SendbirdChatMainContext inChatMainContext)
        {
            Logger.Error(Logger.CategoryType.Command, "This function should not be called. Checks the Json deserialization logic.");
            return null;
        }

        internal static BaseMessageDto JsonStringToMessageDto(string inJsonString)
        {
            if (string.IsNullOrEmpty(inJsonString))
                return null;

            try
            {
                return JObjectToMessageDto(JObject.Parse(inJsonString));
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.Command, $"JsonStringToMessageDto invalid format json:{inJsonString} exception:{exception.Message}");
                return null;
            }
        }

        internal static BaseMessageDto JObjectToMessageDto(JObject inJObject)
        {
            if (inJObject == null)
                return null;

            if (inJObject.TryGetValue("type", out JToken typeToken) == false)
                return null;

            string typeString = typeToken.ToString();
            WsCommandType wsCommandType = WsCommandTypeExtension.JsonNameToType(typeString);
            if (wsCommandType == WsCommandType.UserMessage || wsCommandType == WsCommandType.UpdateUserMessage)
            {
                return UserMessageDto.DeserializeFromJson(inJObject);
            }

            if (wsCommandType == WsCommandType.FileMessage || wsCommandType == WsCommandType.UpdateFileMessage)
            {
                return FileMessageDto.DeserializeFromJson(inJObject);
            }

            if (wsCommandType == WsCommandType.AdminMessage || wsCommandType == WsCommandType.UpdateAdminMessage)
            {
                return AdminMessageDto.DeserializeFromJson(inJObject);
            }

            Logger.Warning(Logger.CategoryType.Command, $"Invalid message type:{typeString} json:{inJObject}");
            return null;
        }
    }
}