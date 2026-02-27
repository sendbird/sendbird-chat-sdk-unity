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

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal Dictionary<string, string> metaData;
            internal long updatedAt;

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (string.IsNullOrEmpty(inJsonString))
                    return;

                using (JsonTextReader reader = JsonStreamingPool.CreateReader(inJsonString))
                {
                    reader.Read();
                    if (reader.TokenType != JsonToken.StartObject)
                        return;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.EndObject)
                            break;

                        string propName = reader.Value as string;
                        reader.Read();
                        switch (propName)
                        {
                            case "metadata": metaData = JsonStreamingHelper.ReadStringDictionary(reader); break;
                            case "ts": updatedAt = JsonStreamingHelper.ReadLong(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }
        }
    }
}