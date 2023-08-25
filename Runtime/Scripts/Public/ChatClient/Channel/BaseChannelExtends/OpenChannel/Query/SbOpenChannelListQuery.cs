// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// A class representing query to retrieve OpenChannel list.
    /// </summary>
    /// @since 4.0.0
    public partial class SbOpenChannelListQuery
    {
        /// <summary>
        /// Indicate search keyword for channel name.
        /// </summary>
        /// @since 4.0.0
        public string NameKeyword => _nameKeyword;

        /// <summary>
        /// Indicate search keyword for channel URL.
        /// </summary>
        /// @since 4.0.0
        public string UrlKeyword => _urlKeyword;

        /// <summary>
        /// A filter to return only channels with the specified customType.
        /// </summary>
        /// @since 4.0.0
        public string CustomTypeFilter => _customTypeFilter;

        /// <summary>
        /// Indicate whether if there is a next page.
        /// </summary>
        /// @since 4.0.0
        public bool HasNext => _hasNext;

        /// <summary>
        /// Indicate whether if the current query is in communication progress with server.
        /// </summary>
        /// @since 4.0.0
        public bool IsLoading => _isLoading;

        /// <summary>
        /// Indicate whether to include frozen channels or not.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeFrozen => _includeFrozen;

        /// <summary>
        /// Indicate whether to include channel metadata on fetch.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeMetadata => _includeMetadata;

        /// <summary>
        /// The maximum number of items per queried page.
        /// </summary>
        /// @since 4.0.0
        public int Limit => _limit;

        /// <summary>
        /// Gets the list of OpenChannels. The queried result is passed to handler as List. If this method is repeatedly called after each next is finished, it retrieves the following pages of the OpenChannel list.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void LoadNextPage(SbOpenChannelListHandler inCompletionHandler)
        {
            LoadNextPageInternal(inCompletionHandler);
        }
    }
}