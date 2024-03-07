// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UpdateMetaCountersApiCommand
    {
        internal enum UpdateModeType
        {
            [JsonName("set")] Set,
            [JsonName("increase")] Increase,
            [JsonName("decrease")] Decrease
        }

        internal sealed class Request : ApiCommandAbstract.PutRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("metacounter")] internal Dictionary<string, int> metaCounters;
                [JsonProperty("upsert")] internal bool upsert;
                [JsonProperty("mode")] internal string mode;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, SbChannelType inChannelType, Dictionary<string, int> inMetaCounters, 
                             UpdateModeType inUpdateModeType, bool inUpsert, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/metacounter";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    metaCounters = inMetaCounters,
                    mode = inUpdateModeType.ToJsonName(),
                    upsert = inUpsert
                };
                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
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