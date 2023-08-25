// 
//  Copyright (c) 2022 Sendbird, Inc.
// 


namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a participant in SbOpenChannel.
    /// </summary>
    /// @since 4.0.0
    public partial class SbParticipant : SbUser
    {
        /// <summary>
        /// The muted state of the participant in the channel.
        /// </summary>
        /// @since 4.0.0
        public bool IsMuted => _isMuted;
    }
}