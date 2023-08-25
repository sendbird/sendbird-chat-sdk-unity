// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Push options for messages.
    /// </summary>
    /// @since 4.0.0
    public enum SbPushNotificationDeliveryOption
    {
        /// <summary>
        /// The push notification will be delivered by the opposite user's setting.
        /// </summary>
        /// @since 4.0.0
        [JsonName("default")] Default,

        /// <summary>
        /// The push notification will never be delivered.
        /// </summary>
        /// @since 4.0.0
        [JsonName("suppress")] Suppress,
    }
}