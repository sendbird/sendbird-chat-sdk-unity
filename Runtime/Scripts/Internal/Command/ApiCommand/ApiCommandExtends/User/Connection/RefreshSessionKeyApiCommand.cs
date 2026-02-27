// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class RefreshSessionKeyApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("expiring_session")] internal bool expiringSession;
#pragma warning restore CS0649
            }

            internal Request(string inAppId, string inUserId, string inAuthToken, bool inExpiringSession, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/session_key";
                IsSessionKeyRequired = false;
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                CustomHeaders.Add("App-Id", inAppId);
                CustomHeaders.Add("Access-Token", inAuthToken);

                Payload tempPayload = new Payload { expiringSession = inExpiringSession };
                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }

        internal sealed class Response : ApiCommandAbstract.Response
        {
            private string _key;
            private string _newKey;

            internal string Key { get; private set; }

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
                            case "key": _key = JsonStreamingHelper.ReadString(reader); break;
                            case "new_key": _newKey = JsonStreamingHelper.ReadString(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }

                Key = _key ?? _newKey;
            }
        }
    }
}