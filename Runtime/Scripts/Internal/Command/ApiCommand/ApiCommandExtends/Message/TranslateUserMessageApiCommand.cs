// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class TranslateUserMessageApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("target_langs")] internal List<string> translationTargetLanguages;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, SbChannelType inChannelType, long inMessageId, List<string> inTargetLanguages, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/messages/{inMessageId}/translation";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                Payload payload = new Payload
                {
                    translationTargetLanguages = inTargetLanguages,
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(payload);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal UserMessageDto UserMessageDtoDto { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                UserMessageDtoDto = UserMessageDto.DeserializeFromJson(inJsonString);
            }
        }
    }
}