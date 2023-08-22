// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal abstract class UpdateMessageWsSendCommandAbstract : WsSendCommandAbstract
    {
#pragma warning disable CS0649
        [JsonProperty("channel_url")] private readonly string _channelUrl;
        [JsonProperty("msg_id")] private readonly long _messageId;
        [JsonProperty("data")] private readonly string _data;
        [JsonProperty("custom_type")] private readonly string _customType;
        [JsonProperty("mention_type")] private readonly string _mentionType;
        [JsonProperty("mentioned_user_ids")] private readonly List<string> _mentionedUserIds;
        [JsonProperty("metaarray")] private readonly MessageMetaArrayUpdateDto _metaArray;
#pragma warning restore CS0649
        internal UpdateMessageWsSendCommandAbstract(WsCommandType inWsCommandType, string inChannelUrl, long inMessageId,
                                                    SbBaseMessageUpdateParams inBaseMessageUpdateParams, AckHandler inAckHandler)
            : base(inWsCommandType, inAckHandler)
        {
            _channelUrl = inChannelUrl;
            _messageId = inMessageId;
            _data = inBaseMessageUpdateParams.Data;
            _customType = inBaseMessageUpdateParams.CustomType;
            _mentionType = inBaseMessageUpdateParams.MentionType.ToJsonName();

            if (inBaseMessageUpdateParams.MentionType == SbMentionType.Users && inBaseMessageUpdateParams.MentionedUserIds != null && 0 < inBaseMessageUpdateParams.MentionedUserIds.Count)
            {
                _mentionedUserIds = new List<string>(inBaseMessageUpdateParams.MentionedUserIds);
            }
        }

        internal UpdateMessageWsSendCommandAbstract(WsCommandType inWsCommandType, string inChannelUrl, long inMessageId, MessageMetaArrayUpdateDto inMetaArrayUpdateDto, AckHandler inAckHandler)
            : base(inWsCommandType, inAckHandler)
        {
            _channelUrl = inChannelUrl;
            _messageId = inMessageId;
            _metaArray = inMetaArrayUpdateDto;
        }
    }
}