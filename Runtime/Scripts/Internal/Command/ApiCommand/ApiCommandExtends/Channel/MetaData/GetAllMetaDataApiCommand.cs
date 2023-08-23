// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetAllMetaDataApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inChannelUrl, SbChannelType inChannelType, ResultHandler inResultHandler)
            {
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/metadata";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty("include_ts", true);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("metadata")] internal Dictionary<string, string> metaData;
            [JsonProperty("ts")] internal readonly long updatedAt;
        }
    }
}