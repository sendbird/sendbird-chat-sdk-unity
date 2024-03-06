// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class HideGroupChannelApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PutRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("user_id")] internal string userId;
                [JsonProperty("hide_previous_messages")] internal bool hidePreviousMessages;
                [JsonProperty("allow_auto_unhide")] internal bool allowAutoUnhide;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, string inUserId, bool inHidePreviousMessages, bool inAllowAutoUnhide, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Group)}/{inChannelUrl}/hide";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    userId = inUserId,
                    hidePreviousMessages = inHidePreviousMessages,
                    allowAutoUnhide = inAllowAutoUnhide
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("ts_message_offset")] internal readonly long? messageOffsetTimestamp;
        }
    }
}