// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Status of push token registration. If connection is not made when token is requested to be registered, PENDING will be returned at callback handler.
    /// </summary>
    /// @since 4.0.0
    public enum SbPushTokenRegistrationStatus
    {
        /// @since 4.0.0
        Success,

        /// @since 4.0.0
        Pending,

        /// @since 4.0.0
        Error
    }
}