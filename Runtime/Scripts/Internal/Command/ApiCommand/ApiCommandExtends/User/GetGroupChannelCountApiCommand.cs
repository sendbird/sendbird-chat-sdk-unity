// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetGroupChannelCountApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, SbMyMemberStateFilter inState, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/group_channel_count";

                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
                
                InsertQueryParamIfNotNullOrEmpty("state", inState.ToJsonName());
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("group_channel_count")] internal readonly int groupChannelCount;
        }
    }
}