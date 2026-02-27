//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal abstract class SendMessageApiCommandAbstract
    {
        internal class Payload
        {
            private string _messageType;
            private string _reqId;
            private string _userId;
            private long? _parentMessageId;
            private string _channelUrl;
            private string _data;
            private string _customType;
            private string _mentionType;
            private List<string> _mentionedUserIds;
            private string _pushNotificationDeliveryOption;
            private List<MessageMetaArrayDto> _metaArrays;
            private AppleCriticalAlertOptionsDto _appleCriticalAlertOptionsDto;
            private bool _replyToChannel;
            private bool _isPinnedMessage;

            protected Payload(string inMessageType, string inRequestId, string inChannelUrl, string inUserId, SbBaseMessageCreateParams inParams)
            {
                _messageType = inMessageType;
                _channelUrl = inChannelUrl;
                _reqId = inRequestId;
                _userId = inUserId;

                if (inParams == null)
                    return;

                _data = inParams.Data;
                _customType = inParams.CustomType;
                _replyToChannel = inParams.ReplyToChannel;
                _isPinnedMessage = inParams.IsPinnedMessage;
                _mentionType = inParams.MentionType.ToJsonName();

                if (inParams.MentionedUserIds != null && 0 < inParams.MentionedUserIds.Count)
                    _mentionedUserIds = new List<string>(inParams.MentionedUserIds);

                if (SbBaseMessage.INVALID_MESSAGE_ID_MIN < inParams.ParentMessageId)
                    _parentMessageId = inParams.ParentMessageId;

                if (inParams.PushNotificationDeliveryOption == SbPushNotificationDeliveryOption.Suppress)
                    _pushNotificationDeliveryOption = inParams.PushNotificationDeliveryOption.ToJsonName();

                if (inParams.AppleCriticalAlertOptions != null)
                    _appleCriticalAlertOptionsDto = new AppleCriticalAlertOptionsDto(inParams.AppleCriticalAlertOptions);

                if (inParams.MetaArrays != null && 0 < inParams.MetaArrays.Count)
                {
                    _metaArrays = new List<MessageMetaArrayDto>(inParams.MetaArrays.Count);
                    foreach (SbMessageMetaArray messageMetaArray in inParams.MetaArrays)
                    {
                        _metaArrays.Add(new MessageMetaArrayDto(messageMetaArray.Key, messageMetaArray.ValueInternal));
                    }
                }
            }

            internal string ToJsonString()
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
                JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "message_type", _messageType);
                JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "req_id", _reqId);
                JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "user_id", _userId);
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
}
