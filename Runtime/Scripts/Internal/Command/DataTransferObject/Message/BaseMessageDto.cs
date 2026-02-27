//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    internal abstract class BaseMessageDto
    {
        // For more information, see [req_id vs request_id](https://sendbird.atlassian.net/wiki/spaces/SDK/pages/1723140230/req+id+vs+request+id)
        // req_id is the same as the request_id of the message when received via the API.
        // When receiving a req_id over WebSocket, it's the same as the req_id in WsReceiveCommand and is used to verify that the current device generated the message.
        private string _messageReqId;

        // request_id is only used in WebSocket.
        private string _messageRequestId;
        internal AppleCriticalAlertOptionsDto appleCriticalAlertOptionsDto;
        internal bool autoResendRegistered;
        private string _channelType;
        internal string channelUrl;
        internal bool checkReactions;
        private long? _createdAt;
        private long? _timestamp;
        internal string customType;
        private string _data;
        private string _custom;
        internal int errorCode;
        internal Dictionary<string, string> extendedMessage;
        internal bool forceUpdateLastMessage;
        internal bool isGlobalBlocked;
        internal bool isOperatorMessage;
        internal bool isReplyToChannel;
        internal bool isGuestMsg;
        internal bool isSuper;
        private string _mentionType;
        internal string mentionedMessageTemplate;
        internal List<UserDto> mentionedUserDtos;
        internal string message;
        internal MessageEventsDto messageEventsDto;
        private long? _messageId;
        internal int messageSurvivalSeconds;
        private Dictionary<string, List<string>> _metaArray;
        private List<string> _metaArrayKeyOrder;
        private long? _msgId;
        private string _ogTagJson;
        internal long? parentMessageId;
        private string _parentMessageInfoJson;
        private string _reactionsJson;
        internal long scheduledAt;
        internal long scheduledMessageId;
        internal bool silent;
        private List<MessageMetaArrayDto> _sortedMetaArrayDtos;
        private string _threadInfoJson;
        internal long updatedAt;
        internal SenderDto senderDto;

        internal SbChannelType ChannelType { get; private set; } = SbChannelType.Group;
        internal SbMentionType MentionType { get; private set; } = SbMentionType.Users;
        internal SbSendingStatus RequestState { get; private set; } = SbSendingStatus.None;
        internal string MessageCreatedRequestId { get; private set; }
        internal long MessageId { get; private set; }
        internal long CreatedAt { get; private set; }
        internal string Data { get; private protected set; }
        internal List<MessageMetaArrayDto> MetaArrayDtos { get; private set; }
        private OgMetaDataDto _cachedOgTag;
        private BaseMessageDto _cachedParentMessageInfo;
        private List<ReactionDto> _cachedReactions;
        private ThreadInfoDto _cachedThreadInfo;

        internal OgMetaDataDto OgMetaDataDto
        {
            get
            {
                if (_cachedOgTag == null && !string.IsNullOrEmpty(_ogTagJson))
                    _cachedOgTag = OgMetaDataDto.ReadFromJsonString(_ogTagJson);
                return _cachedOgTag;
            }
        }

        internal BaseMessageDto ParentMessageInfoDto
        {
            get
            {
                if (_cachedParentMessageInfo == null && !string.IsNullOrEmpty(_parentMessageInfoJson))
                    _cachedParentMessageInfo = BaseMessageDto.JsonStringToMessageDto(_parentMessageInfoJson);
                return _cachedParentMessageInfo;
            }
        }

        internal List<ReactionDto> ReactionDtos
        {
            get
            {
                if (_cachedReactions == null && !string.IsNullOrEmpty(_reactionsJson))
                    _cachedReactions = ReactionDto.ReadListFromJsonString(_reactionsJson);
                return _cachedReactions;
            }
        }

        internal ThreadInfoDto ThreadInfoDto
        {
            get
            {
                if (_cachedThreadInfo == null && !string.IsNullOrEmpty(_threadInfoJson))
                    _cachedThreadInfo = ThreadInfoDto.ReadFromJsonString(_threadInfoJson);
                return _cachedThreadInfo;
            }
        }

        internal virtual void PostDeserialize()
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

        internal virtual bool TryReadSubclassField(JsonTextReader inReader, string inPropName)
        {
            return false;
        }

        // Internal setters for StreamingMessageDtoReader single-pass access
        internal void SetMessageReqId(string inValue) { _messageReqId = inValue; }
        internal void SetMessageRequestId(string inValue) { _messageRequestId = inValue; }

        /// <summary>
        /// Tries to read a single base field from the reader. Returns true if the field was handled.
        /// Used by StreamingMessageDtoReader for single-pass command+DTO deserialization.
        /// </summary>
        internal bool TryReadBaseField(JsonTextReader inReader, string inPropName)
        {
            switch (inPropName)
            {
                case "req_id": _messageReqId = JsonStreamingHelper.ReadString(inReader); return true;
                case "request_id": _messageRequestId = JsonStreamingHelper.ReadString(inReader); return true;
                case "apple_critical_alert_options": appleCriticalAlertOptionsDto = AppleCriticalAlertOptionsDto.ReadFromJson(inReader); return true;
                case "auto_resend_registered": autoResendRegistered = JsonStreamingHelper.ReadBool(inReader); return true;
                case "channel_type": _channelType = JsonStreamingHelper.ReadString(inReader); return true;
                case "channel_url": channelUrl = JsonStreamingHelper.ReadString(inReader); return true;
                case "check_reactions": checkReactions = JsonStreamingHelper.ReadBool(inReader); return true;
                case "created_at": _createdAt = JsonStreamingHelper.ReadNullableLong(inReader); return true;
                case "ts": _timestamp = JsonStreamingHelper.ReadNullableLong(inReader); return true;
                case "custom_type": customType = JsonStreamingHelper.ReadString(inReader); return true;
                case "data": _data = JsonStreamingHelper.ReadString(inReader); return true;
                case "custom": _custom = JsonStreamingHelper.ReadString(inReader); return true;
                case "error_code": errorCode = JsonStreamingHelper.ReadInt(inReader); return true;
                case "extended_message": extendedMessage = JsonStreamingHelper.ReadStringDictionary(inReader); return true;
                case "force_update_last_message": forceUpdateLastMessage = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_global_block": isGlobalBlocked = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_op_msg": isOperatorMessage = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_reply_to_channel": isReplyToChannel = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_guest_msg": isGuestMsg = JsonStreamingHelper.ReadBool(inReader); return true;
                case "is_super": isSuper = JsonStreamingHelper.ReadBool(inReader); return true;
                case "mention_type": _mentionType = JsonStreamingHelper.ReadString(inReader); return true;
                case "mentioned_message_template": mentionedMessageTemplate = JsonStreamingHelper.ReadString(inReader); return true;
                case "mentioned_users": mentionedUserDtos = UserDto.ReadUserDtoListFromJson(inReader); return true;
                case "message": message = JsonStreamingHelper.ReadString(inReader); return true;
                case "message_events": messageEventsDto = MessageEventsDto.ReadFromJson(inReader); return true;
                case "message_id": _messageId = JsonStreamingHelper.ReadNullableLong(inReader); return true;
                case "message_survival_seconds": messageSurvivalSeconds = JsonStreamingHelper.ReadInt(inReader); return true;
                case "metaarray": _metaArray = JsonStreamingHelper.ReadStringListDictionary(inReader); return true;
                case "metaarray_key_order": _metaArrayKeyOrder = JsonStreamingHelper.ReadStringList(inReader); return true;
                case "msg_id": _msgId = JsonStreamingHelper.ReadNullableLong(inReader); return true;
                case "og_tag": _ogTagJson = JsonStreamingHelper.ReadRawJsonString(inReader); return true;
                case "parent_message_id": parentMessageId = JsonStreamingHelper.ReadNullableLong(inReader); return true;
                case "parent_message_info": _parentMessageInfoJson = JsonStreamingHelper.ReadRawJsonString(inReader); return true;
                case "reactions": _reactionsJson = JsonStreamingHelper.ReadRawJsonString(inReader); return true;
                case "scheduled_at": scheduledAt = JsonStreamingHelper.ReadLong(inReader); return true;
                case "scheduled_message_id": scheduledMessageId = JsonStreamingHelper.ReadLong(inReader); return true;
                case "silent": silent = JsonStreamingHelper.ReadBool(inReader); return true;
                case "sorted_metaarray": _sortedMetaArrayDtos = MessageMetaArrayDto.ReadListFromJson(inReader); return true;
                case "thread_info": _threadInfoJson = JsonStreamingHelper.ReadRawJsonString(inReader); return true;
                case "updated_at": updatedAt = JsonStreamingHelper.ReadLong(inReader); return true;
                case "user": senderDto = SenderDto.ReadFromJson(inReader); return true;
                default: return false;
            }
        }

        internal static void ReadFields(JsonTextReader inReader, BaseMessageDto inDto)
        {
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();

                if (inDto.TryReadSubclassField(inReader, propName))
                    continue;

                switch (propName)
                {
                    case "req_id": inDto._messageReqId = JsonStreamingHelper.ReadString(inReader); break;
                    case "request_id": inDto._messageRequestId = JsonStreamingHelper.ReadString(inReader); break;
                    case "apple_critical_alert_options": inDto.appleCriticalAlertOptionsDto = AppleCriticalAlertOptionsDto.ReadFromJson(inReader); break;
                    case "auto_resend_registered": inDto.autoResendRegistered = JsonStreamingHelper.ReadBool(inReader); break;
                    case "channel_type": inDto._channelType = JsonStreamingHelper.ReadString(inReader); break;
                    case "channel_url": inDto.channelUrl = JsonStreamingHelper.ReadString(inReader); break;
                    case "check_reactions": inDto.checkReactions = JsonStreamingHelper.ReadBool(inReader); break;
                    case "created_at": inDto._createdAt = JsonStreamingHelper.ReadNullableLong(inReader); break;
                    case "ts": inDto._timestamp = JsonStreamingHelper.ReadNullableLong(inReader); break;
                    case "custom_type": inDto.customType = JsonStreamingHelper.ReadString(inReader); break;
                    case "data": inDto._data = JsonStreamingHelper.ReadString(inReader); break;
                    case "custom": inDto._custom = JsonStreamingHelper.ReadString(inReader); break;
                    case "error_code": inDto.errorCode = JsonStreamingHelper.ReadInt(inReader); break;
                    case "extended_message": inDto.extendedMessage = JsonStreamingHelper.ReadStringDictionary(inReader); break;
                    case "force_update_last_message": inDto.forceUpdateLastMessage = JsonStreamingHelper.ReadBool(inReader); break;
                    case "is_global_block": inDto.isGlobalBlocked = JsonStreamingHelper.ReadBool(inReader); break;
                    case "is_op_msg": inDto.isOperatorMessage = JsonStreamingHelper.ReadBool(inReader); break;
                    case "is_reply_to_channel": inDto.isReplyToChannel = JsonStreamingHelper.ReadBool(inReader); break;
                    case "is_guest_msg": inDto.isGuestMsg = JsonStreamingHelper.ReadBool(inReader); break;
                    case "is_super": inDto.isSuper = JsonStreamingHelper.ReadBool(inReader); break;
                    case "mention_type": inDto._mentionType = JsonStreamingHelper.ReadString(inReader); break;
                    case "mentioned_message_template": inDto.mentionedMessageTemplate = JsonStreamingHelper.ReadString(inReader); break;
                    case "mentioned_users": inDto.mentionedUserDtos = UserDto.ReadUserDtoListFromJson(inReader); break;
                    case "message": inDto.message = JsonStreamingHelper.ReadString(inReader); break;
                    case "message_events": inDto.messageEventsDto = MessageEventsDto.ReadFromJson(inReader); break;
                    case "message_id": inDto._messageId = JsonStreamingHelper.ReadNullableLong(inReader); break;
                    case "message_survival_seconds": inDto.messageSurvivalSeconds = JsonStreamingHelper.ReadInt(inReader); break;
                    case "metaarray": inDto._metaArray = JsonStreamingHelper.ReadStringListDictionary(inReader); break;
                    case "metaarray_key_order": inDto._metaArrayKeyOrder = JsonStreamingHelper.ReadStringList(inReader); break;
                    case "msg_id": inDto._msgId = JsonStreamingHelper.ReadNullableLong(inReader); break;
                    case "og_tag": inDto._ogTagJson = JsonStreamingHelper.ReadRawJsonString(inReader); break;
                    case "parent_message_id": inDto.parentMessageId = JsonStreamingHelper.ReadNullableLong(inReader); break;
                    case "parent_message_info": inDto._parentMessageInfoJson = JsonStreamingHelper.ReadRawJsonString(inReader); break;
                    case "reactions": inDto._reactionsJson = JsonStreamingHelper.ReadRawJsonString(inReader); break;
                    case "scheduled_at": inDto.scheduledAt = JsonStreamingHelper.ReadLong(inReader); break;
                    case "scheduled_message_id": inDto.scheduledMessageId = JsonStreamingHelper.ReadLong(inReader); break;
                    case "silent": inDto.silent = JsonStreamingHelper.ReadBool(inReader); break;
                    case "sorted_metaarray": inDto._sortedMetaArrayDtos = MessageMetaArrayDto.ReadListFromJson(inReader); break;
                    case "thread_info": inDto._threadInfoJson = JsonStreamingHelper.ReadRawJsonString(inReader); break;
                    case "updated_at": inDto.updatedAt = JsonStreamingHelper.ReadLong(inReader); break;
                    case "user": inDto.senderDto = SenderDto.ReadFromJson(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            inDto.PostDeserialize();
        }

        internal virtual void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            WriteBaseFields(inWriter);
            inWriter.WriteEndObject();
        }

        internal void WriteBaseFields(JsonTextWriter inWriter)
        {
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "req_id", _messageReqId);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "request_id", _messageRequestId);
            if (appleCriticalAlertOptionsDto != null)
            {
                inWriter.WritePropertyName("apple_critical_alert_options");
                appleCriticalAlertOptionsDto.WriteToJson(inWriter);
            }
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "auto_resend_registered", autoResendRegistered);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "channel_type", _channelType);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "channel_url", channelUrl);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "check_reactions", checkReactions);
            JsonStreamingHelper.WriteNullableLong(inWriter, "created_at", _createdAt);
            JsonStreamingHelper.WriteNullableLong(inWriter, "ts", _timestamp);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "custom_type", customType);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "data", _data);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "custom", _custom);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "error_code", errorCode);
            JsonStreamingHelper.WriteStringDictionary(inWriter, "extended_message", extendedMessage);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "force_update_last_message", forceUpdateLastMessage);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_global_block", isGlobalBlocked);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_op_msg", isOperatorMessage);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_reply_to_channel", isReplyToChannel);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_guest_msg", isGuestMsg);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_super", isSuper);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "mention_type", _mentionType);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "mentioned_message_template", mentionedMessageTemplate);
            UserDto.WriteListToJson(inWriter, "mentioned_users", mentionedUserDtos);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "message", message);
            if (messageEventsDto != null)
            {
                inWriter.WritePropertyName("message_events");
                messageEventsDto.WriteToJson(inWriter);
            }
            JsonStreamingHelper.WriteNullableLong(inWriter, "message_id", _messageId);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "message_survival_seconds", messageSurvivalSeconds);
            JsonStreamingHelper.WriteNullableLong(inWriter, "msg_id", _msgId);
            JsonStreamingHelper.WriteRawJsonProperty(inWriter, "og_tag", _ogTagJson);
            JsonStreamingHelper.WriteNullableLong(inWriter, "parent_message_id", parentMessageId);
            JsonStreamingHelper.WriteRawJsonProperty(inWriter, "parent_message_info", _parentMessageInfoJson);
            JsonStreamingHelper.WriteRawJsonProperty(inWriter, "reactions", _reactionsJson);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "scheduled_at", scheduledAt);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "scheduled_message_id", scheduledMessageId);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "silent", silent);
            MessageMetaArrayDto.WriteListToJson(inWriter, "sorted_metaarray", _sortedMetaArrayDtos);
            JsonStreamingHelper.WriteRawJsonProperty(inWriter, "thread_info", _threadInfoJson);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "updated_at", updatedAt);
            if (senderDto != null)
            {
                inWriter.WritePropertyName("user");
                senderDto.WriteToJson(inWriter);
            }
        }

        internal static BaseMessageDto JObjectToMessageDto(JObject inJObject)
        {
            if (inJObject == null)
                return null;

            string jsonString = inJObject.ToString(Formatting.None);
            return JsonStringToMessageDto(jsonString);
        }

        internal static BaseMessageDto JsonStringToMessageDto(string inJsonString)
        {
            if (string.IsNullOrEmpty(inJsonString))
                return null;

            try
            {
                return JsonStringToMessageDtoDirectDeserialize(inJsonString);
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.Command, $"JsonStringToMessageDto invalid format json:{inJsonString} exception:{exception.Message}");
                return null;
            }
        }

        private static BaseMessageDto JsonStringToMessageDtoDirectDeserialize(string inJsonString)
        {
            string typeString = NewtonsoftJsonExtension.ExtractTypeField(inJsonString);
            if (string.IsNullOrEmpty(typeString))
                return null;

            WsCommandType wsCommandType = WsCommandTypeExtension.JsonNameToType(typeString);
            if (wsCommandType == WsCommandType.UserMessage || wsCommandType == WsCommandType.UpdateUserMessage)
            {
                return UserMessageDto.DeserializeFromJson(inJsonString);
            }

            if (wsCommandType == WsCommandType.FileMessage || wsCommandType == WsCommandType.UpdateFileMessage)
            {
                return FileMessageDto.DeserializeFromJson(inJsonString);
            }

            if (wsCommandType == WsCommandType.AdminMessage || wsCommandType == WsCommandType.UpdateAdminMessage)
            {
                return AdminMessageDto.DeserializeFromJson(inJsonString);
            }

            Logger.Warning(Logger.CategoryType.Command, $"Invalid message type:{typeString} json:{inJsonString}");
            return null;
        }
    }
}
