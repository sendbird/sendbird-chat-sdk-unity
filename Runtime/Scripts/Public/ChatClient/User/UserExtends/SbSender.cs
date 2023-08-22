// 
//  Copyright (c) 2022 Sendbird, Inc.
// 


namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a user that typically binds with message
    /// </summary>
    /// @since 4.0.0
    public partial class SbSender : SbUser
    {
        /// <summary>
        /// If true, the current user blocked the sender.
        /// </summary>
        /// @since 4.0.0
        public bool IsBlockedByMe => _isBlockedByMe;

        /// <summary>
        /// The role of the sender in the channel.
        /// </summary>
        /// @since 4.0.0
        public SbRole Role => _role;
    }
}