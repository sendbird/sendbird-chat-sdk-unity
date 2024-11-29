// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetDoNotDisturbApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/push_preference";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("do_not_disturb")] internal bool isDoNotDisturbEnable;
            [JsonProperty("start_hour")] internal int startHour;
            [JsonProperty("start_min")] internal int startMin;
            [JsonProperty("end_hour")] internal int endHour;
            [JsonProperty("end_min")] internal int endMin;
            [JsonProperty("timezone")] internal string timezone;
        }
    }
}