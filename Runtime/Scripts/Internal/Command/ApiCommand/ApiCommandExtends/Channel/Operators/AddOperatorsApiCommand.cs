// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class AddOperatorsApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("operator_ids")] internal List<string> userIds;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, SbChannelType inChannelType, List<string> inUserIds, ResultHandler inResultHandler)
            {
                inChannelUrl = HttpUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/operators";
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    userIds = inUserIds
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }
    }
}