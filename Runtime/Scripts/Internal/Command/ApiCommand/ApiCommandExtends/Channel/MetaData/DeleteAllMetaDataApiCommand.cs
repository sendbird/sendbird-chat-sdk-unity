// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class DeleteAllMetaDataApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("include_ts")] internal bool includeTs;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, SbChannelType inChannelType, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/metadata";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    includeTs = true
                };
                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }

        internal sealed class Response : ApiCommandAbstract.Response
        {
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
                            case "ts": updatedAt = JsonStreamingHelper.ReadLong(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }
        }
    }
}