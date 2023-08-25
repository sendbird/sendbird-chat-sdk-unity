// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    /// @since 4.0.0
    public class SbConnectionHandler
    {
        /// <summary>
        /// Invoked when connected.
        /// </summary>
        /// @since 4.0.0
        public Action<string /*inUserId*/> OnConnected { get; set; }

        /// <summary>
        /// Invoked when disconnected.
        /// </summary>
        /// @since 4.0.0
        public Action<string /*inUserId*/> OnDisconnected { get; set; }

        /// <summary>
        /// Invoked when reconnection starts.
        /// </summary>
        /// @since 4.0.0
        public Action OnReconnectStarted { get; set; }

        /// <summary>
        /// Invoked when reconnection is succeeded.
        /// </summary>
        /// @since 4.0.0
        public Action OnReconnectSucceeded { get; set; }

        /// <summary>
        /// Invoked when reconnection is failed.
        /// </summary>
        /// @since 4.0.0
        public Action OnReconnectFailed { get; set; }
    }
}