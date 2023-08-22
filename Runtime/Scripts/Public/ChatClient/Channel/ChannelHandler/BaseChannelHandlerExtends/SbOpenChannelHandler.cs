// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sendbird.Chat
{
    /// <summary>
    /// OpenChannel handler. This handler provides callbacks for events related OpenChannel.
    /// </summary>
    /// @since 4.0.0
    public class SbOpenChannelHandler : SbBaseChannelHandler
    {
        /// @since 4.0.0
        public delegate void UserEnteredDelegate(SbOpenChannel inOpenChannel, SbUser inUser);

        /// @since 4.0.0
        public delegate void UserExitedDelegate(SbOpenChannel inOpenChannel, SbUser inUser);

        /// @since 4.0.0
        public delegate void ChannelParticipantCountChangedDelegate(IReadOnlyList<SbOpenChannel> inOpenChannels);

        /// <summary>
        /// A callback for when a User has entered OpenChannel. To use the updated participant count, refer to SbOpenChannel.ParticipantCount.
        /// </summary>
        /// @since 4.0.0
        public UserEnteredDelegate OnUserEntered { get; set; }

        /// <summary>
        /// A callback for when a User has exited OpenChannel. To use the updated participant count, refer to SbOpenChannel.ParticipantCount.
        /// </summary>
        /// @since 4.0.0
        public UserExitedDelegate OnUserExited { get; set; }

        /// <summary>
        /// Called when one or more open channel's member counts are changed.
        /// </summary>
        /// @since 4.0.0
        public ChannelParticipantCountChangedDelegate OnChannelParticipantCountChanged { get; set; }
    }
}