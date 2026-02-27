// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class MarkAsDeliveredGroupChannelApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PutRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("message_id")] internal long messageId;
#pragma warning restore CS0649
            }

            internal override bool IsLoginRequired => false;

            internal Request(string inChannelUrl, long inMessageId, string inSessionKey, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Group)}/{inChannelUrl}/messages/mark_as_delivered";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
                OverrideSessionKey = inSessionKey;

                Payload tempPayload = new Payload
                {
                    messageId = inMessageId
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal long timestamp;

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
                            case "ts": timestamp = JsonStreamingHelper.ReadLong(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }
        }
    }
}