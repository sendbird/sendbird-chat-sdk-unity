// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a handler to receive user relates events
    /// </summary>
    /// @since 4.0.0
    public class SbUserEventHandler
    {
        /// @since 4.0.0
        public delegate void OnTotalUnreadMessageCountChangedDelegate(SbUnreadMessageCount inUnreadMessageCount);

        /// @since 4.0.0
        public delegate void OnFriendsDiscoveredDelegate(List<SbUser> inUsers);

        /// <summary>
        /// Invoked when list of users has been discovered
        /// </summary>
        /// @since 4.0.0
        public OnFriendsDiscoveredDelegate OnFriendsDiscovered { get; set; }

        /// <summary>
        /// Invoked when total unread message count has been updated
        /// </summary>
        /// @since 4.0.0
        public OnTotalUnreadMessageCountChangedDelegate OnTotalUnreadMessageCountChanged { get; set; }
    }
}