// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using Newtonsoft.Json;

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

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal int totalCount;
            internal string endToken;
            internal bool hasNext;
            internal List<BaseMessageDto> BaseMessageDtos { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (string.IsNullOrEmpty(inJsonString))
                    return;

                using (JsonTextReader reader = JsonStreamingPool.CreateReader(inJsonString))
                {
                    reader.Read();
                    if (reader.TokenType != JsonToken.StartObject)
                        return;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.EndObject)
                            break;

                        string propName = reader.Value as string;
                        reader.Read();
                        switch (propName)
                        {
                            case "results": BaseMessageDtos = JsonStreamingHelper.ReadMessageDtoListDirect(reader); break;
                            case "total_count": totalCount = JsonStreamingHelper.ReadInt(reader); break;
                            case "end_cursor": endToken = JsonStreamingHelper.ReadString(reader); break;
                            case "has_next": hasNext = JsonStreamingHelper.ReadBool(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }
        }
    }
}