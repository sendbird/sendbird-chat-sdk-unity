//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class SendUserMessageApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            private class Payload : SendMessageApiCommandAbstract.Payload
            {
                internal string message;
                internal List<string> translationTargetLanguages;
                internal long? pollId;
                internal string mentionedMessageTemplate;

                internal Payload(string inChannelUrl, string inRequestId, string inUserId, SbUserMessageCreateParams inParams)
                    : base(WsCommandType.UserMessage.ToJsonName(), inRequestId, inChannelUrl, inUserId, inParams) { }

                protected override void WriteFields(JsonTextWriter inWriter)
                {
                    base.WriteFields(inWriter);
                    JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "message", message);
                    JsonStreamingHelper.WriteStringList(inWriter, "target_langs", translationTargetLanguages);
                    JsonStreamingHelper.WriteNullableLong(inWriter, "poll_id", pollId);
                    JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "mentioned_message_template", mentionedMessageTemplate);
                }
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

                ContentBody = payload.ToJsonString();
            }
        }

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
