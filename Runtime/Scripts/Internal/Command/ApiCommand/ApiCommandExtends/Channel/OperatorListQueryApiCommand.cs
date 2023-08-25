// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class OperatorListQueryApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inToken, int inLimit, SbChannelType inChannelType, string inChannelUrl, ResultHandler inResultHandler)
            {
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/operators";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty("token", inToken);
                InsertQueryParamIfNotNullOrEmpty("limit", inLimit);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("next")] internal readonly string token;
            [JsonProperty("operators")] internal readonly List<UserDto> userDtos;
        }
    }
}