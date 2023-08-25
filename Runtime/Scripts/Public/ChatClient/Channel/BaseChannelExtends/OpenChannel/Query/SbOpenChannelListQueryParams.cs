// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Params for creating a OpenChannelListQuery object.
    /// </summary>
    /// @since 4.0.0
    public class SbOpenChannelListQueryParams
    {
        /// <summary>
        /// Search keyword for channel name. If this is set, SbOpenChannelListQuery.LoadNextPage will return the list of OpenChannels of which name matches the specified name.
        /// </summary>
        /// @since 4.0.0
        public string NameKeyword { get; set; } = null;

        /// <summary>
        /// Search keyword for channel URL. If this is set, OpenChannelListQuery.LoadNextPage will return the list of OpenChannels of which URL matches the specified URL.
        /// </summary>
        /// @since 4.0.0
        public string UrlKeyword { get; set; } = null;

        /// <summary>
        /// A filter to return only channels with the specified customType.
        /// </summary>
        /// @since 4.0.0
        public string CustomTypeFilter { get; set; } = null;

        /// <summary>
        /// Indicate whether to include frozen channels or not. This flag is true by default.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeFrozen { get; set; }

        /// <summary>
        /// Indicate whether to include channel metadata on fetch. This flag is true by default.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeMetadata { get; set; }

        /// <summary>
        /// The maximum number of items per queried page.
        /// </summary>
        /// @since 4.0.0
        public int Limit { get; set; } = SendbirdChatMainContext.QUERY_DEFAULT_LIMIT;
    }
}