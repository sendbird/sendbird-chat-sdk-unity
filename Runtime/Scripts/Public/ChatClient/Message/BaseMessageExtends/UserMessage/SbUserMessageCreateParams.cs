// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a user message params.
    /// </summary>
    /// @since 4.0.0
    public partial class SbUserMessageCreateParams : SbBaseMessageCreateParams
    {
        /// <summary>
        /// The message text of the message.
        /// </summary>
        /// @since 4.0.0
        public string Message { get => _message; set => _message = value; }

        /// <summary>
        /// The message's mentioned message template of the message.
        /// </summary>
        /// @since 4.0.0
        public string MentionedMessageTemplate { get => _mentionedMessageTemplate; set => _mentionedMessageTemplate = value; }

        /// <summary>
        /// The translation target languages.
        /// </summary>
        /// @since 4.0.0
        public List<string> TranslationTargetLanguages { get => _translationTargetLanguages; set => _translationTargetLanguages = value; }

        /// @since 4.0.0
        public SbUserMessageCreateParams(string inMessage = null)
        {
            Message = inMessage;
        }
    }
}