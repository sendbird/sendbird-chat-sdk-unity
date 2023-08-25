// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class OpenChannelListQueryApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inToken, int inLimit, string inNameContains, string inUrlContains, string inCustomType,
                             bool inShowFrozen, bool inShowMetadata, ResultHandler inResultHandler)
            {
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Open)}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty("token", inToken);
                InsertQueryParamIfNotNullOrEmpty("limit", inLimit);
                InsertQueryParamIfNotNullOrEmpty("name_contains", inNameContains);
                InsertQueryParamIfNotNullOrEmpty("url_contains", inUrlContains);
                InsertQueryParamIfNotNullOrEmpty("custom_type", inCustomType);
                InsertQueryParamIfNotNullOrEmpty("show_frozen", inShowFrozen);
                InsertQueryParamIfNotNullOrEmpty("show_metadata", inShowMetadata);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("next")] internal readonly string token;
            [JsonProperty("channels")] internal readonly List<OpenChannelDto> openChannelDtos;
        }
    }
}