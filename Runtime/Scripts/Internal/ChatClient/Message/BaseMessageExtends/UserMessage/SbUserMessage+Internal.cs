// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbUserMessage : SbBaseMessage
    {
        private readonly Dictionary<string, string> _translations = null;
        private readonly List<string> _translationTargetLanguages = null;

        private SbUserMessage(SbUserMessage inOtherMessage) : base(inOtherMessage)
        {
            if (inOtherMessage._translations != null && 0 < inOtherMessage._translations.Count)
                _translations = new Dictionary<string, string>(inOtherMessage._translations);

            if (inOtherMessage._translationTargetLanguages != null && 0 < inOtherMessage._translationTargetLanguages.Count)
                _translationTargetLanguages = new List<string>(inOtherMessage._translationTargetLanguages);
        }

        private SbUserMessage(SbUserMessageCreateParams inUserMessageCreateParams, SendbirdChatMainContext inChatMainContext, 
                              SbSender inSender, SbBaseChannel inBaseChannel, string inRequestId = null)
            : base(inUserMessageCreateParams, inChatMainContext, inSender, inBaseChannel, inRequestId)
        {
            if (inUserMessageCreateParams != null)
            {
                message = inUserMessageCreateParams.Message;

                if (inUserMessageCreateParams.TranslationTargetLanguages != null && 0 < inUserMessageCreateParams.TranslationTargetLanguages.Count)
                    _translationTargetLanguages = new List<string>(inUserMessageCreateParams.TranslationTargetLanguages);
            }
        }

        internal SbUserMessage(UserMessageDto inUserMessageDto, SendbirdChatMainContext inChatMainContext)
            : base(inUserMessageDto, inChatMainContext)
        {
            if (inUserMessageDto != null)
            {
                _translations = inUserMessageDto.translations;
                _translationTargetLanguages = inUserMessageDto.translationTargetLanguages;
            }
        }

        protected override SbBaseMessage Clone()
        {
            return new SbUserMessage(this);
        }

        internal static SbUserMessage CreateMessage(SbUserMessageCreateParams inUserMessageCreateParams, SendbirdChatMainContext inChatMainContext,
                                                    SbSender inSender, SbBaseChannel inBaseChannel, bool inIsOperatorMessage, string inRequestId = null)
        {
            SbUserMessage userMessage = new SbUserMessage(inUserMessageCreateParams, inChatMainContext, inSender, inBaseChannel, inRequestId)
            {
                isOperatorMessage = inIsOperatorMessage
            };
            return userMessage;
        }
    }
}