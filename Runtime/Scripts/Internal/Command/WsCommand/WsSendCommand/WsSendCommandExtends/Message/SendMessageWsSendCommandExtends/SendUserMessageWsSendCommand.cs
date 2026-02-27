//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class SendUserMessageWsSendCommand : SendMessageWsSendCommandAbstract
    {
        private readonly string _message;
        private readonly List<string> _translationTargetLanguages;
        private readonly string _mentionedMessageTemplate;

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

        protected override void WriteFields(JsonTextWriter inWriter)
        {
            base.WriteFields(inWriter);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "message", _message);
            JsonStreamingHelper.WriteStringList(inWriter, "target_langs", _translationTargetLanguages);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "mentioned_message_template", _mentionedMessageTemplate);
        }
    }
}
