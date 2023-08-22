// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Objects representing a reaction event.
    /// </summary>
    /// @since 4.0.0
    public partial class SbReactionEvent
    {
        /// <summary>
        /// The key of the SbReaction this event belongs to.
        /// </summary>
        /// @since 4.0.0
        public string Key => _key;

        /// <summary>
        /// The message ID which contains the SbReaction of this event.
        /// </summary>
        /// @since 4.0.0
        public long MessageId => _messageId;

        /// <summary>
        /// The updated timestamp of the reaction event in milliseconds.
        /// </summary>
        /// @since 4.0.0
        public long UpdatedAt => _updatedAt;

        /// <summary>
        /// The user ID of the User who have created this reaction event..
        /// </summary>
        /// @since 4.0.0
        public string UserId => _userId;

        /// <summary>
        /// Refer to SbReactionEventAction.
        /// </summary>
        /// @since 4.0.0
        public SbReactionEventAction Operation => _operation;
    }
}