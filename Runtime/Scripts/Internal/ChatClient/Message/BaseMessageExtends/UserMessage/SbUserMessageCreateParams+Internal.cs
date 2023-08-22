// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbUserMessageCreateParams : SbBaseMessageCreateParams
    {
        private string _message = null;
        private string _mentionedMessageTemplate = null;
        private List<string> _translationTargetLanguages = null;

        private SbUserMessageCreateParams(SbUserMessageCreateParams inOtherParams) : base(inOtherParams)
        {
            if (inOtherParams != null)
            {
                _message = inOtherParams._message;
                _mentionedMessageTemplate = inOtherParams._mentionedMessageTemplate;
                if (inOtherParams._translationTargetLanguages != null && 0 < inOtherParams._translationTargetLanguages.Count)
                {
                    _translationTargetLanguages = new List<string>(inOtherParams._translationTargetLanguages);
                }
            }
        }

        internal SbUserMessageCreateParams(SbUserMessage inUserMessage) : base(inUserMessage)
        {
            if (inUserMessage != null)
            {
                _message = inUserMessage.Message;
                _mentionedMessageTemplate = inUserMessage.MentionedMessageTemplate;
                if (inUserMessage.TranslationTargetLanguages != null && 0 < inUserMessage.TranslationTargetLanguages.Count)
                    _translationTargetLanguages = new List<string>(inUserMessage.TranslationTargetLanguages);
            }
        }

        internal override SbBaseMessageCreateParams Clone()
        {
            return new SbUserMessageCreateParams(this);
        }
    }
}