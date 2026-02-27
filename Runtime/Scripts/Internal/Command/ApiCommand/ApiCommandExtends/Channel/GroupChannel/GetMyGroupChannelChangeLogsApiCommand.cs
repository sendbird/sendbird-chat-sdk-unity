// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetMyGroupChannelChangeLogsApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, string inToken, long? inTimestamp, SbGroupChannelChangeLogsParams inParams, int inLimit, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/my_group_channels/changelogs";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                if (inTimestamp != null)
                {
                    InsertQueryParamIfNotNullOrEmpty("change_ts", inTimestamp.Value);
                }
                else
                {
                    InsertQueryParamIfNotNullOrEmpty("token", inToken);
                }

                InsertQueryParamWithListIfNotNullOrEmpty("custom_types", inParams.CustomTypes);
                InsertQueryParamIfNotNullOrEmpty("show_frozen", inParams.IncludeFrozen);
                InsertQueryParamIfNotNullOrEmpty("show_empty", inParams.IncludeEmpty);
                InsertQueryParamIfNotNullOrEmpty("limit", inLimit);
                InsertQueryParamIfNotNullOrEmpty("show_member", true);
                InsertQueryParamIfNotNullOrEmpty("show_delivery_receipt", true);
                InsertQueryParamIfNotNullOrEmpty("show_read_receipt", true);
                InsertQueryParamIfNotNullOrEmpty("include_chat_notification", false);
            }
        }

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal List<GroupChannelDto> updatedGroupChannelDtos;
            internal List<string> deletedChannelUrls;
            internal bool hasMore;
            internal string token;

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
                            case "updated": updatedGroupChannelDtos = GroupChannelDto.ReadListFromJson(reader); break;
                            case "deleted": deletedChannelUrls = JsonStreamingHelper.ReadStringList(reader); break;
                            case "has_more": hasMore = JsonStreamingHelper.ReadBool(reader); break;
                            case "next": token = JsonStreamingHelper.ReadString(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }
        }
    }
}