// 
//  Copyright (c) 2024 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The options to choose which push notification for the current user to receive.
    /// </summary>
    /// @since 4.1.0
    public enum SbPushTriggerOption
    {
        /// <summary>
        /// Receive all of remote push notification.
        /// </summary>
        /// @since 4.1.0
        [JsonName("all")] All,

        /// <summary>
        /// Do NOT receive any remote push notification.
        /// </summary>
        /// @since 4.1.0
        [JsonName("off")] Off,
        
        /// <summary>
        /// Receive only mentioned messages’s notification.
        /// </summary>
        /// @since 4.1.0
        [JsonName("mention_only")] MentionOnly
    }
}