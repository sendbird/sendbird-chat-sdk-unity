// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetAllMetaCountersApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("metacounter")] internal Dictionary<string, string> metaCounters;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, SbChannelType inChannelType, ResultHandler inResultHandler)
            {
                inChannelUrl = HttpUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/metacounter";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal Dictionary<string, int> MetaCounters { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                MetaCounters = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<Dictionary<string, int>>(inJsonString);
            }
        }
    }
}