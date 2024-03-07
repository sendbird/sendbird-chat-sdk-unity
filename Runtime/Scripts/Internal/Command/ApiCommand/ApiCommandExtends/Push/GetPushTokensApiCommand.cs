// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetPushTokensApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, SbPushTokenType inTokenType, string inToken, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/push/{inTokenType.ToJsonName()}/device_tokens";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty("token", inToken);
            }
        }
        
        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("device_tokens")] internal readonly List<string> deviceTokens;
            [JsonProperty("type")] private readonly string _pushTokenType;
            [JsonProperty("token")] internal readonly string token;
            [JsonProperty("has_more")] internal readonly bool hasMore;

            internal SbPushTokenType PushTokenType { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (string.IsNullOrEmpty(_pushTokenType) == false)
                {
                    PushTokenType = SbPushTokenTypeExtension.JsonNameToType(_pushTokenType);
                }
            }
        }
    }
}