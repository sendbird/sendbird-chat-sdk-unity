// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetMyMutedInfoApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inChannelUrl, SbChannelType inChannelType, string inUserId, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/mute/{inUserId}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("is_muted")] internal readonly bool isMuted;
            [JsonProperty("remaining_duration")] internal readonly long remainingDuration;
            [JsonProperty("start_at")] internal readonly long startAt;
            [JsonProperty("end_at")] internal readonly long endAt;
            [JsonProperty("description")] internal readonly string description;
        }
    }
}