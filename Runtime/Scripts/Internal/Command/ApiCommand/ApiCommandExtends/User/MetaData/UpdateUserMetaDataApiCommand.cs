// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UpdateUserMetaDataApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PutRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("metadata")] internal Dictionary<string, string> metaData;
                [JsonProperty("upsert")] internal bool upsert;
#pragma warning restore CS0649
            }

            internal Request(string inUserId, bool inUpsert, Dictionary<string, string> inMetaData, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/metadata";

                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    metaData = inMetaData,
                    upsert = inUpsert
                };
                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal Dictionary<string, string> MetaData { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                MetaData = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<Dictionary<string, string>>(inJsonString);
            }
        }
    }
}