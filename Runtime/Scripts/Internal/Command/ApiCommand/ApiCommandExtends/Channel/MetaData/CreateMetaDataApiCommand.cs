// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class CreateMetaDataApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("metadata")] internal Dictionary<string, string> metaData;
                [JsonProperty("include_ts")] internal bool includeTs;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, SbChannelType inChannelType, Dictionary<string, string> inMetaData, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/metadata";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    metaData = inMetaData,
                    includeTs = true
                };
                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("metadata")] internal Dictionary<string, string> metaData;
            [JsonProperty("ts")] internal readonly long updatedAt;
        }
    }
}