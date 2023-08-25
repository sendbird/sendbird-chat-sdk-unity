// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class BlockedUserListQueryApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inToken, int inLimit, string inUserId, List<string> inUserIds,  ResultHandler inResultHandler)
            {
                Url = $"{USERS_PREFIX_URL}/{inUserId}/block";
                
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty("token", inToken);
                InsertQueryParamIfNotNullOrEmpty("limit", inLimit);
                InsertQueryParamWithListIfNotNullOrEmpty("user_ids", inUserIds);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("next")] internal readonly string token;
            [JsonProperty("users")] internal readonly List<UserDto> userDtos;
        }
    }
}