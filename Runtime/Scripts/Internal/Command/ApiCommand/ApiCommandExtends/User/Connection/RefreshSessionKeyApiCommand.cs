// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
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

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("key")] private readonly string _key;
            [JsonProperty("new_key")] private readonly string _newKey;

            internal string Key { get; private set; }

            [OnDeserialized]
            private void OnDeserialized(StreamingContext inStreamingContext)
            {
                Key = _key ?? _newKey;
            }
        }
    }
}