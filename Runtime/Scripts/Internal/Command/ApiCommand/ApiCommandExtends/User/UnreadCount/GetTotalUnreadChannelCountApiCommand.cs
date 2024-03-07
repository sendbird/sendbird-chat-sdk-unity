// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetTotalUnreadChannelCountApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, List<string> inCustomTypes, SbGroupChannelSuperChannelFilter inSuperChannelFilter, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/unread_channel_count";

                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamWithListIfNotNullOrEmpty("custom_types", inCustomTypes);
                InsertQueryParamIfNotNullOrEmpty("super_mode", inSuperChannelFilter.ToJsonName());
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("unread_count")] internal readonly int unreadCount;
        }
    }
}