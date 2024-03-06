// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetTotalUnreadMessageCountApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, List<string> inCustomTypes, SbGroupChannelSuperChannelFilter inSuperChannelFilter, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/unread_message_count";

                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamWithListIfNotNullOrEmpty("custom_types", inCustomTypes);
                InsertQueryParamIfNotNullOrEmpty("super_mode", inSuperChannelFilter.ToJsonName());
                InsertQueryParamIfNotNullOrEmpty("include_feed_channel", false);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("unread_count")] internal readonly int groupChannelUnreadCount;
            [JsonProperty("unread_feed_count")] internal readonly int feedChannelUnreadCount;
        }
    }
}