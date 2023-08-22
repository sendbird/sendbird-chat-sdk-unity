// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a base message params.
    /// </summary>
    /// @since 4.0.0
    public abstract partial class SbBaseMessageUpdateParams
    {
        /// <summary>
        /// The custom type of the message.
        /// </summary>
        /// @since 4.0.0
        public string CustomType { get => _customType; set => _customType = value; }

        /// <summary>
        /// The data of the message.
        /// </summary>
        /// @since 4.0.0
        public string Data { get => _data; set => _data = value; }

        /// <summary>
        /// The mention type of the message. Defaults to SbMentionType.Users.
        /// </summary>
        /// @since 4.0.0
        public SbMentionType MentionType { get => _mentionType; set => _mentionType = value; }

        /// <summary>
        /// The mentioned user ids of the message.
        /// </summary>
        /// @since 4.0.0
        public List<string> MentionedUserIds { get => _mentionedUserIds; set => _mentionedUserIds = value; }
    }
}