// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The enum type to filter my group channels with the hidden state.
    /// </summary>
    /// @since 4.0.0
    public enum SbChannelHiddenStateFilter
    {
        /// <summary>
        /// Shows the unhidden channels only.
        /// </summary>
        /// @since 4.0.0
        [JsonName("unhidden_only")] UnhiddenOnly,

        /// <summary>
        /// Shows the hidden channels only.
        /// </summary>
        /// @since 4.0.0
        [JsonName("hidden_only")] HiddenOnly,

        /// <summary>
        /// Shows the channels will be unhidden automatically when there is a new message in the channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("hidden_allow_auto_unhide")] HiddenAllowAutoUnhide,

        /// <summary>
        /// Shows the channels will not be unhidden automatically.
        /// </summary>
        /// @since 4.0.0
        [JsonName("hidden_prevent_auto_unhide")] HiddenPreventAutoUnhide,

        /// <summary>
        /// Shows all channels.
        /// </summary>
        /// @since 4.0.0
        [JsonName("all")] All
    }
}