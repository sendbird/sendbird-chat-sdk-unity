// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class RemoveAllOperatorsApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("delete_all")] internal bool deleteAll;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, SbChannelType inChannelType, ResultHandler inResultHandler)
            {
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/operators";
                resultHandler = inResultHandler;
                Payload tempPayload = new Payload
                {
                    deleteAll = true
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }
    }
}