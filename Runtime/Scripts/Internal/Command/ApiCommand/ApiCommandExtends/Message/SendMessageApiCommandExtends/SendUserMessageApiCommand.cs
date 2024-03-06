// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class SendUserMessageApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            [Serializable]
            private class Payload : SendMessageApiCommandAbstract.Payload
            {
#pragma warning disable CS0649
                [JsonProperty("message")] internal string message;
                [JsonProperty("target_langs")] internal List<string> translationTargetLanguages;
                [JsonProperty("poll_id")] internal long? pollId;
                [JsonProperty("mentioned_message_template")] internal string mentionedMessageTemplate;
#pragma warning restore CS0649

                internal Payload(string inChannelUrl, string inRequestId, string inUserId, SbUserMessageCreateParams inParams)
                    : base(WsCommandType.UserMessage.ToJsonName(), inRequestId, inChannelUrl, inUserId, inParams) { }
            }

            internal Request(string inRequestId, string inChannelUrl, SbChannelType inChannelType, string inUserId, SbUserMessageCreateParams inParams, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/messages";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                Payload payload = new Payload(inChannelUrl, inRequestId, inUserId, inParams);
                {
                    if (inParams != null)
                    {
                        payload.message = inParams.Message;
                        payload.translationTargetLanguages = inParams.TranslationTargetLanguages;
                        payload.mentionedMessageTemplate = inParams.MentionedMessageTemplate;
                    }
                }

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