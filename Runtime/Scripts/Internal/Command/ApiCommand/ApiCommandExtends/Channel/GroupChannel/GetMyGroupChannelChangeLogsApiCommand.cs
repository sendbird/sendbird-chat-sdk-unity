// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
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

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("updated")] internal readonly List<GroupChannelDto> updatedGroupChannelDtos;
            [JsonProperty("deleted")] internal readonly List<string> deletedChannelUrls;
            [JsonProperty("has_more")] internal readonly bool hasMore;
            [JsonProperty("next")] internal readonly string token;
        }
    }
}