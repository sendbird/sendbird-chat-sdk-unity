//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal abstract class SendMessageWsSendCommandAbstract : WsSendCommandAbstract
    {
        private readonly long? _parentMessageId;
        private readonly string _channelUrl;
        private readonly string _data;
        private readonly string _customType;
        private readonly string _mentionType;
        private readonly List<string> _mentionedUserIds;
        private readonly string _pushNotificationDeliveryOption;
        private readonly List<MessageMetaArrayDto> _metaArrays;
        private readonly AppleCriticalAlertOptionsDto _appleCriticalAlertOptionsDto;
        private readonly bool _replyToChannel;
        private readonly bool _isPinnedMessage;

        internal SendMessageWsSendCommandAbstract(WsCommandType inWsCommandType, string inReqId, string inChannelUrl,
                                              SbBaseMessageCreateParams inBaseMessageCreateParams, AckHandler inAckHandler)
            : base(inWsCommandType, inAckHandler, inReqId)
        {
            _channelUrl = inChannelUrl;
            _data = inBaseMessageCreateParams.Data;
            _customType = inBaseMessageCreateParams.CustomType;
            _replyToChannel = inBaseMessageCreateParams.ReplyToChannel;
            _isPinnedMessage = inBaseMessageCreateParams.IsPinnedMessage;

            _mentionType = inBaseMessageCreateParams.MentionType.ToJsonName();

            if (inBaseMessageCreateParams.MentionedUserIds != null && 0 < inBaseMessageCreateParams.MentionedUserIds.Count)
                _mentionedUserIds = new List<string>(inBaseMessageCreateParams.MentionedUserIds);

            if (SbBaseMessage.INVALID_MESSAGE_ID_MIN < inBaseMessageCreateParams.ParentMessageId)
            {
                _parentMessageId = inBaseMessageCreateParams.ParentMessageId;
            }

            if (inBaseMessageCreateParams.PushNotificationDeliveryOption == SbPushNotificationDeliveryOption.Suppress)
                _pushNotificationDeliveryOption = inBaseMessageCreateParams.PushNotificationDeliveryOption.ToJsonName();

            if (inBaseMessageCreateParams.AppleCriticalAlertOptions != null)
                _appleCriticalAlertOptionsDto = new AppleCriticalAlertOptionsDto(inBaseMessageCreateParams.AppleCriticalAlertOptions);

            if (inBaseMessageCreateParams.MetaArrays != null && 0 < inBaseMessageCreateParams.MetaArrays.Count)
            {
                _metaArrays = new List<MessageMetaArrayDto>(inBaseMessageCreateParams.MetaArrays.Count);
                foreach (SbMessageMetaArray messageMetaArray in inBaseMessageCreateParams.MetaArrays)
                {
                    _metaArrays.Add(new MessageMetaArrayDto(messageMetaArray.Key, messageMetaArray.ValueInternal));
                }
            }
        }

        protected override string SerializeToJsonString()
        {
            return JsonStreamingPool.WriteIgnoreException(writer =>
            {
                writer.WriteStartObject();
                WriteFields(writer);
                writer.WriteEndObject();
            });
        }

        protected virtual void WriteFields(JsonTextWriter inWriter)
        {
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "req_id", ReqId);
            JsonStreamingHelper.WriteNullableLong(inWriter, "parent_message_id", _parentMessageId);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "channel_url", _channelUrl);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "data", _data);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "custom_type", _customType);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "mention_type", _mentionType);
            JsonStreamingHelper.WriteStringList(inWriter, "mentioned_user_ids", _mentionedUserIds);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "push_option", _pushNotificationDeliveryOption);
            MessageMetaArrayDto.WriteListToJson(inWriter, "metaarray", _metaArrays);
            if (_appleCriticalAlertOptionsDto != null)
            {
                inWriter.WritePropertyName("apple_critical_alert_options");
                _appleCriticalAlertOptionsDto.WriteToJson(inWriter);
            }
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "reply_to_channel", _replyToChannel);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "pin_message", _isPinnedMessage);
        }
    }
}
