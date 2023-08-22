// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UserListQueryApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inToken, int inLimit, List<string> inUserIds, string inMetaDataKye, List<string> inMetaDataValues, 
                             string inNicknameStartsWith, ResultHandler inResultHandler)
            {
                Url = $"{USERS_PREFIX_URL}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty("token", inToken);
                InsertQueryParamIfNotNullOrEmpty("limit", inLimit);
                InsertQueryParamIfNotNullOrEmpty("nickname_startswith", inNicknameStartsWith);
                InsertQueryParamWithListIfNotNullOrEmpty("user_ids", inUserIds);
                
                InsertQueryParamIfNotNullOrEmpty("metadatakey", inMetaDataKye);
                InsertQueryParamWithListIfNotNullOrEmpty("metadatavalues_in", inMetaDataValues);
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