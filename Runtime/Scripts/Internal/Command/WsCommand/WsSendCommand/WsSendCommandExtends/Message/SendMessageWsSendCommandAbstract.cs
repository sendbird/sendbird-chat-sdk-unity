// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal abstract class SendMessageWsSendCommandAbstract : WsSendCommandAbstract
    {
#pragma warning disable CS0649
        [JsonProperty("parent_message_id")] private readonly long? _parentMessageId;
        [JsonProperty("channel_url")] private readonly string _channelUrl;
        [JsonProperty("data")] private readonly string _data;
        [JsonProperty("custom_type")] private readonly string _customType;
        [JsonProperty("mention_type")] private readonly string _mentionType;
        [JsonProperty("mentioned_user_ids")] private readonly List<string> _mentionedUserIds;
        [JsonProperty("push_option")] private readonly string _pushNotificationDeliveryOption;
        [JsonProperty("metaarray")] private readonly List<MessageMetaArrayDto> _metaArrays;
        [JsonProperty("apple_critical_alert_options")] private readonly AppleCriticalAlertOptionsDto _appleCriticalAlertOptionsDto;
        [JsonProperty("reply_to_channel")] private readonly bool _replyToChannel;
        [JsonProperty("pin_message")] private readonly bool _isPinnedMessage;
#pragma warning restore CS0649
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
    }
}