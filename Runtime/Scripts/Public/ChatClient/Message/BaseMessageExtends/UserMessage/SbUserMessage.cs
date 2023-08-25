// 
//  Copyright (c) 2023 Sendbird, Inc.
// 


using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Object representing a user message.
    /// </summary>
    /// @since 4.0.0
    public partial class SbUserMessage : SbBaseMessage
    {
        /// <summary>
        /// The translated messages (key-value map) for the language codes in key.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyDictionary<string, string> Translations => _translations;

        /// <summary>
        /// The list of target translation languages with the language codes.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> TranslationTargetLanguages => _translationTargetLanguages;
    }
}