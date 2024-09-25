// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    internal sealed class MessageSearchQueryApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inToken, string inBeforeToken, string inAfterToken, SbMessageSearchQueryParams inParams, ResultHandler inResultHandler)
            {
                Url = $"{SEARCH_PREFIX_URL}/messages";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                if (inParams != null)
                {
                    InsertQueryParamIfNotNullOrEmpty("query", inParams.Keyword);
                    InsertQueryParamIfNotNullOrEmpty("channel_url", inParams.ChannelUrl);
                    InsertQueryParamIfNotNullOrEmpty("custom_type", inParams.ChannelCustomType);
                    InsertQueryParamIfNotNullOrEmpty("include_open", false);
                    InsertQueryParamIfNotNullOrEmpty("include_not_joined_public", false);
                    InsertQueryParamIfNotNullOrEmpty("sort_field", inParams.Order.ToJsonName());
                    InsertQueryParamIfNotNullOrEmpty("reverse", inParams.Reverse);
                    InsertQueryParamIfNotNullOrEmpty("exact_match", inParams.ExactMatch);
                    InsertQueryParamIfNotNullOrEmpty("advanced_query", inParams.AdvancedQuery);
                    InsertQueryParamWithListIfNotNullOrEmpty("target_fields", inParams.TargetFields);
                    InsertQueryParamIfNotNullOrEmpty("before", inBeforeToken);
                    InsertQueryParamIfNotNullOrEmpty("after", inAfterToken);
                    InsertQueryParamIfNotNullOrEmpty("token", inToken);
                    InsertQueryParamIfNotNullOrEmpty("limit", inParams.Limit);

                    if (0 < inParams.MessageTimestampTo)
                        InsertQueryParamIfNotNullOrEmpty("message_ts_to", inParams.MessageTimestampTo);

                    if (0 < inParams.MessageTimestampFrom)
                        InsertQueryParamIfNotNullOrEmpty("message_ts_from", inParams.MessageTimestampFrom);
                }
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
#pragma warning disable CS0649
            [JsonProperty("results")] private readonly List<JObject> _messageJObjects;
            [JsonProperty("total_count")] internal readonly int totalCount;
            [JsonProperty("end_cursor")] internal readonly string endToken;
            [JsonProperty("has_next")] internal readonly bool hasNext;
#pragma warning restore CS0649

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