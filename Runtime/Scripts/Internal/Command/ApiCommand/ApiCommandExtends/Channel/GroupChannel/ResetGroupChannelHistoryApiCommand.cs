// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class ResetGroupChannelHistoryApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PutRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("user_id")] internal string userId;
                [JsonProperty("reset_all")] internal bool resetAll;
#pragma warning restore CS0649
            }

            internal override bool IsLoginRequired => false;

            internal Request(string inChannelUrl, string inUserId, bool inResetAll, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Group)}/{inChannelUrl}/reset_user_history";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    userId = inUserId,
                    resetAll = inResetAll
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("ts_message_offset")] internal readonly long messageOffsetTimestamp;
        }
    }
}