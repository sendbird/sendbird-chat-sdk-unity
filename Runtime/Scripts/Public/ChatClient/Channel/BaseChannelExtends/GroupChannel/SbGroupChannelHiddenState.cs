// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The enum type for the hidden state of a group channel.
    /// </summary>
    /// @since 4.0.0
    public enum SbGroupChannelHiddenState
    {
        /// <summary>
        /// Shows the channel is unhidden.
        /// </summary>
        /// @since 4.0.0
        [JsonName("unhidden")] Unhidden,

        /// <summary>
        /// Shows the channel will be unhidden automatically when there is a new message in the channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("hidden_allow_auto_unhide")] HiddenAllowAutoUnhide,

        /// <summary>
        /// Shows the channel will not be unhidden automatically.
        /// </summary>
        /// @since 4.0.0
        [JsonName("hidden_prevent_auto_unhide")] HiddenPreventAutoUnhide
    }
}