// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The options to choose which push notification for the current user to receive in a group channel.
    /// </summary>
    /// @since 4.0.0
    public enum SbGroupChannelPushTriggerOption
    {
        /// <summary>
        /// Follow the push trigger of current user.
        /// </summary>
        /// @since 4.0.0
        [JsonName("default")] Default,

        /// <summary>
        /// Receive all of remote push notification.
        /// </summary>
        /// @since 4.0.0
        [JsonName("all")] All,

        /// <summary>
        /// Do NOT receive any remote push notification.
        /// </summary>
        /// @since 4.0.0
        [JsonName("off")] Off,

        /// <summary>
        /// Receive only mentioned messages’s notification.
        /// </summary>
        /// @since 4.0.0
        [JsonName("mention_only")] MentionOnly
    }
}