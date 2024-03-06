// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class AddReactionApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("user_id")] internal string userId;
                [JsonProperty("reaction")] internal string reaction;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, SbChannelType inChannelType, long inMessageId, string inUserId, string inReaction, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/messages/{inMessageId}/reactions";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                Payload payload = new Payload
                {
                    userId = inUserId,
                    reaction = inReaction
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(payload);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal ReactionEventDto ReactionEventDto { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                ReactionEventDto = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<ReactionEventDto>(inJsonString);
            }
        }
    }
}