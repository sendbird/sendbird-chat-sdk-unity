// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The order type for SbPublicGroupChannelListQuery.
    /// </summary>
    /// @since 4.0.0
    public enum SbPublicGroupChannelListOrder
    {
        /// <summary>
        /// Chronological order for public group channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("chronological")] Chronological,

        /// <summary>
        /// Alphabetical name order for public group channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("channel_name_alphabetical")] ChannelNameAlphabetical,

        /// <summary>
        /// Alphabetical value order of a selected key in meta data for public group channel.
        /// </summary>
        /// @since 4.0.0
        [JsonName("metadata_value_alphabetical")] ChannelMetaDataValueAlphabetical
    }
}