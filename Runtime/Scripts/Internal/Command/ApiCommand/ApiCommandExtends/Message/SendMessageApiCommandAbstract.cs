// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal abstract class SendMessageApiCommandAbstract
    {
        [Serializable]
        internal class Payload
        {
#pragma warning disable CS0649
            [JsonProperty("message_type")] private string _messageType;
            [JsonProperty("req_id")] private string _reqId;
            [JsonProperty("user_id")] private string _userId;
            [JsonProperty("parent_message_id")] private long? _parentMessageId;
            [JsonProperty("channel_url")] private string _channelUrl;
            [JsonProperty("data")] private string _data;
            [JsonProperty("custom_type")] private string _customType;
            [JsonProperty("mention_type")] private string _mentionType;
            [JsonProperty("mentioned_user_ids")] private List<string> _mentionedUserIds;
            [JsonProperty("push_option")] private string _pushNotificationDeliveryOption;
            [JsonProperty("metaarray")] private List<MessageMetaArrayDto> _metaArrays;
            [JsonProperty("apple_critical_alert_options")] private AppleCriticalAlertOptionsDto _appleCriticalAlertOptionsDto;
            [JsonProperty("reply_to_channel")] private bool _replyToChannel;
            [JsonProperty("pin_message")] private bool _isPinnedMessage;
#pragma warning restore CS0649

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
        }
    }
}