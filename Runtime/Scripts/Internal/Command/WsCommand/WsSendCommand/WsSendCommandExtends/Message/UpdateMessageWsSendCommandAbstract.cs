//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal abstract class UpdateMessageWsSendCommandAbstract : WsSendCommandAbstract
    {
        private readonly string _channelUrl;
        private readonly long _messageId;
        private readonly string _data;
        private readonly string _customType;
        private readonly string _mentionType;
        private readonly List<string> _mentionedUserIds;
        private readonly MessageMetaArrayUpdateDto _metaArray;

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
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "channel_url", _channelUrl);
            JsonStreamingHelper.WriteProperty(inWriter, "msg_id", _messageId);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "data", _data);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "custom_type", _customType);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "mention_type", _mentionType);
            JsonStreamingHelper.WriteStringList(inWriter, "mentioned_user_ids", _mentionedUserIds);
            if (_metaArray != null)
            {
                inWriter.WritePropertyName("metaarray");
                _metaArray.WriteToJson(inWriter);
            }
        }
    }
}
