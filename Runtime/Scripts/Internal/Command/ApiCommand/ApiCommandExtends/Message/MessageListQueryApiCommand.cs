// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    internal sealed class MessageListQueryApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal struct Params
            {
                internal long messageTimestamp;
                internal long messageId;
                internal int prevLimit;
                internal int nextLimit;
                internal bool inclusive;
                internal bool reverse;
                internal SbMessageTypeFilter messageTypeFilter;
                internal List<string> customTypes;
                internal List<string> senderUserIds;
                internal long parentMessageId;
                internal bool showSubChannelMessagesOnly;
                internal bool? checkContinuousMessages;
                internal bool includeMetaArray;
                internal bool includeReactions;
                internal bool includeThreadInfo;
                internal bool includeParentMessageInfo;
                internal SbReplyType replyType;
            }

            internal Request(SbChannelType inChannelType, string inChannelUrl, Params inParams, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/messages";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty("is_sdk", true);
                InsertQueryParamIfNotNullOrEmpty("prev_limit", inParams.prevLimit);
                InsertQueryParamIfNotNullOrEmpty("next_limit", inParams.nextLimit);
                InsertQueryParamIfNotNullOrEmpty("include", inParams.inclusive);
                InsertQueryParamIfNotNullOrEmpty("reverse", inParams.reverse);
                InsertQueryParamIfNotNullOrEmpty("message_type", inParams.messageTypeFilter.ToJsonName());
                InsertQueryParamWithListIfNotNullOrEmpty("custom_types", inParams.customTypes);
                InsertQueryParamWithListIfNotNullOrEmpty("sender_ids", inParams.senderUserIds);
                InsertQueryParamIfNotNullOrEmpty("with_sorted_meta_array", inParams.includeMetaArray);
                InsertQueryParamIfNotNullOrEmpty("include_reactions", inParams.includeReactions);
                InsertQueryParamIfNotNullOrEmpty("include_thread_info", inParams.includeThreadInfo);
                InsertQueryParamIfNotNullOrEmpty("include_parent_message_info", inParams.includeParentMessageInfo);
                InsertQueryParamIfNotNullOrEmpty("include_reply_type", inParams.replyType.ToJsonName());

                if (inChannelType == SbChannelType.Open)
                {
                    InsertQueryParamIfNotNullOrEmpty("show_subchannel_messages_only", inParams.showSubChannelMessagesOnly);
                }

                if (SbBaseMessage.INVALID_MESSAGE_ID_MIN < inParams.messageId)
                {
                    InsertQueryParamIfNotNullOrEmpty("message_id", inParams.messageId);
                }
                else
                {
                    InsertQueryParamIfNotNullOrEmpty("message_ts", inParams.messageTimestamp);
                }

                if (SbBaseMessage.INVALID_MESSAGE_ID_MIN < inParams.parentMessageId)
                    InsertQueryParamIfNotNullOrEmpty("parent_message_id", inParams.parentMessageId);

                if (inParams.checkContinuousMessages != null)
                    InsertQueryParamIfNotNullOrEmpty("checking_continuous_messages", inParams.checkContinuousMessages.Value);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("messages")] private readonly List<JObject> _messageJObjects;
            [JsonProperty("is_continuous_messages")] internal readonly bool? isContinuousMessages;

            internal List<BaseMessageDto> BaseMessageDtos { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (_messageJObjects != null && 0 < _messageJObjects.Count)
                {
                    BaseMessageDtos = new List<BaseMessageDto>(_messageJObjects.Count);
                    foreach (JObject messageJObject in _messageJObjects)
                    {
                        if (messageJObject == null)
                            continue;

                        BaseMessageDto baseMessageDto = BaseMessageDto.JObjectToMessageDto(messageJObject);
                        if (baseMessageDto == null)
                            continue;

                        BaseMessageDtos.Add(baseMessageDto);
                    }
                }
            }
        }
    }
}