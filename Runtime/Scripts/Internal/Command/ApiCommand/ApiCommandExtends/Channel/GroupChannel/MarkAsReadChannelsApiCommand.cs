// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class MarkAsReadChannelsApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PutRequest
        {

            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("channel_urls")] internal List<string> channelUrls;
#pragma warning restore CS0649
            }

            internal Request(string inUserId, List<string> inChannelUrls, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/mark_as_read_all";
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    channelUrls = inChannelUrls
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }
    }
}