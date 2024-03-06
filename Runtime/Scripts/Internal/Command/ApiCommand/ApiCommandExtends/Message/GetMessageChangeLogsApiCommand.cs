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
    internal sealed class GetMessageChangeLogsApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inChannelUrl, SbChannelType inChannelType, string inToken, long inTimestamp, SbMessageChangeLogsParams inParams, ResultHandler inResultHandler)
            {
                if (inParams == null)
                {
                    inResultHandler?.Invoke(null, SbErrorCodeExtension.INVALID_PARAMETER_ERROR, false);
                    return;
                }

                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/messages/changelogs";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                if (0 < inTimestamp)
                {
                    InsertQueryParamIfNotNullOrEmpty("change_ts", inTimestamp);
                }
                else
                {
                    InsertQueryParamIfNotNullOrEmpty("token", inToken);
                }

                InsertQueryParamIfNotNullOrEmpty("with_sorted_meta_array", inParams.IncludeMetaArray);
                InsertQueryParamIfNotNullOrEmpty("include_reactions", inParams.IncludeReactions);
                InsertQueryParamIfNotNullOrEmpty("include_thread_info", inParams.IncludeThreadInfo);
                InsertQueryParamIfNotNullOrEmpty("include_parent_message_info", inParams.IncludeParentMessageInfo);
                InsertQueryParamIfNotNullOrEmpty("includeReplyType", inParams.ReplyType.ToJsonName());
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("updated")] private readonly List<JObject> _updatedMessageJObjects;
            [JsonProperty("deleted")] internal readonly List<DeletedMessageDto> deletedMessageDtos;
            [JsonProperty("has_more")] internal readonly bool hasMore;
            [JsonProperty("next")] internal readonly string token;
            internal List<BaseMessageDto> UpdatedBaseMessageDtos { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (_updatedMessageJObjects != null && 0 < _updatedMessageJObjects.Count)
                {
                    UpdatedBaseMessageDtos = new List<BaseMessageDto>(_updatedMessageJObjects.Count);
                    foreach (JObject baseMessageJObject in _updatedMessageJObjects)
                    {
                        UpdatedBaseMessageDtos.Add(BaseMessageDto.JObjectToMessageDto(baseMessageJObject));
                    }
                }
            }
        }
    }
}