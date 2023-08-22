// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// @since 4.0.0
    public enum SbPushTokenType
    {
        /// @since 4.0.0
        [JsonName("gcm")] Fcm,

        /// @since 4.0.0
        [JsonName("huawei")] Hms,

        /// @since 4.0.0
        [JsonName("apns")] Apns
    }
}