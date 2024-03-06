// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UnregisterPushTokenApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inUserId, SbPushTokenType inTokenType, string inToken, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                inToken = WebUtility.UrlEncode(inToken);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/push/{inTokenType.ToJsonName()}/{inToken}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
            }

            [Serializable]
            internal sealed class Response : ApiCommandAbstract.Response
            {
                [JsonProperty("device_token_last_deleted_at")] internal readonly long deviceTokenLastDeletedAt;
            }
        }
    }
}