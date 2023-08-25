// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The query result order type.
    /// </summary>
    /// @since 4.0.0
    public enum SbGroupChannelListOrder
    {
        /// <summary>
        /// query returns the result as by event time descending order.
        /// </summary>
        /// @since 4.0.0
        [JsonName("chronological")] Chronological,

        /// <summary>
        /// query returns the result as by event time descending order.
        /// </summary>
        /// @since 4.0.0
        [JsonName("latest_last_message")] LatestLastMessage,

        /// <summary>
        /// query returns the result as by channel name alphabetical order.
        /// </summary>
        /// @since 4.0.0
        [JsonName("channel_name_alphabetical")] ChannelNameAlphabetical,

        /// <summary>
        /// Alphabetical value order of a selected key in meta data for group channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("metadata_value_alphabetical")] ChannelMetaDataValueAlphabetical
    }
}