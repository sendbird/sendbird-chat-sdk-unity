// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents message sending status.
    /// </summary>
    /// @since 4.0.0
    public enum SbSendingStatus
    {
        /// @since 4.0.0
        Scheduled,

        /// @since 4.0.0
        Canceled,

        /// @since 4.0.0
        Succeeded,

        /// @since 4.0.0
        Failed,

        /// @since 4.0.0
        Pending,

        /// @since 4.0.0
        None,
    }
}