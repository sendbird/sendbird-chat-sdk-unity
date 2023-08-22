// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UnregisterAllPushTokenApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inUserId, ResultHandler inResultHandler)
            {
                Url = $"{USERS_PREFIX_URL}/{inUserId}/push";
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