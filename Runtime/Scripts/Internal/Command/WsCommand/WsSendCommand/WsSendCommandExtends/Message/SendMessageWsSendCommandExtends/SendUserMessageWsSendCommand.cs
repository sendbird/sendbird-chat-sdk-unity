// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class SendUserMessageWsSendCommand : SendMessageWsSendCommandAbstract
    {
#pragma warning disable CS0649
        [JsonProperty("message")] private readonly string _message;
        [JsonProperty("target_langs")] private readonly List<string> _translationTargetLanguages;
        //[JsonProperty("poll_id")] private readonly long? _pollId = null;
        [JsonProperty("mentioned_message_template")] private readonly string _mentionedMessageTemplate;
#pragma warning restore CS0649

        internal SendUserMessageWsSendCommand(string inReqId, string inChannelUrl, SbUserMessageCreateParams inUserMessageCreateParams, AckHandler inAckHandler)
            : base(WsCommandType.UserMessage, inReqId, inChannelUrl, inUserMessageCreateParams, inAckHandler)
        {
            _message = inUserMessageCreateParams.Message;
            _mentionedMessageTemplate = inUserMessageCreateParams.MentionedMessageTemplate;
            if (inUserMessageCreateParams.TranslationTargetLanguages != null && 0 < inUserMessageCreateParams.TranslationTargetLanguages.Count)
            {
                _translationTargetLanguages = new List<string>(inUserMessageCreateParams.TranslationTargetLanguages);
            }
        }
    }
}