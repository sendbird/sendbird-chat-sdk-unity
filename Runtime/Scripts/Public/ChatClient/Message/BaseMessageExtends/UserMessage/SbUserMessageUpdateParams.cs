// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a user message update params.
    /// </summary>
    /// @since 4.0.0
    public partial class SbUserMessageUpdateParams : SbBaseMessageUpdateParams
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
        
        /// @since 4.0.0
        public SbUserMessageUpdateParams(string inMessage = null)
        {
            Message = inMessage;
        }
    }
}