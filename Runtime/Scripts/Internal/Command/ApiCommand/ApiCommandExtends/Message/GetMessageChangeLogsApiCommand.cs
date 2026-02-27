// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

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

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal List<DeletedMessageDto> deletedMessageDtos;
            internal bool hasMore;
            internal string token;
            internal List<BaseMessageDto> UpdatedBaseMessageDtos { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (string.IsNullOrEmpty(inJsonString))
                    return;

                using (JsonTextReader reader = JsonStreamingPool.CreateReader(inJsonString))
                {
                    ReadFromJsonReader(reader);
                }
            }

            internal override void OnResponseAfterDeserialize(byte[] inResponseBytes)
            {
                if (inResponseBytes == null || inResponseBytes.Length == 0)
                    return;

                using (JsonTextReader reader = JsonStreamingPool.CreateReader(inResponseBytes))
                {
                    ReadFromJsonReader(reader);
                }
            }

            private void ReadFromJsonReader(JsonTextReader inReader)
            {
                inReader.Read();
                if (inReader.TokenType != JsonToken.StartObject)
                    return;

                while (inReader.Read())
                {
                    if (inReader.TokenType == JsonToken.EndObject)
                        break;

                    string propName = inReader.Value as string;
                    inReader.Read();
                    switch (propName)
                    {
                        case "updated": UpdatedBaseMessageDtos = JsonStreamingHelper.ReadMessageDtoListDirect(inReader); break;
                        case "deleted": deletedMessageDtos = DeletedMessageDto.ReadListFromJson(inReader); break;
                        case "has_more": hasMore = JsonStreamingHelper.ReadBool(inReader); break;
                        case "next": token = JsonStreamingHelper.ReadString(inReader); break;
                        default: JsonStreamingHelper.SkipValue(inReader); break;
                    }
                }
            }
        }
    }
}