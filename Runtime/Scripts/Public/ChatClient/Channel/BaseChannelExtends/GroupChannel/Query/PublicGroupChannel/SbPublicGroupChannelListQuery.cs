// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// A class representing query to retrieve GroupChannel list for the current User.
    /// </summary>
    /// @since 4.0.0
    public partial class SbPublicGroupChannelListQuery
    {
        /// <summary>
        /// List of channel URL filter. It will return null if channel URL filter hasn't been set before. GroupChannel list containing only and exactly the passed GroupChannel URLs will be returned.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> ChannelUrlsFilter => _channelUrlsFilter;

        /// <summary>
        /// Result order of channels. Refer to GroupChannelListQueryOrder. SbPublicGroupChannelListOrder.ChannelMetaDataValueAlphabetical works with metaDataOrderKeyFilter.
        /// </summary>
        /// @since 4.0.0
        public SbPublicGroupChannelListOrder Order => _order;

        /// <summary>
        /// Super channel filter. Refer to SbGroupChannelSuperChannelFilter.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelSuperChannelFilter SuperChannelFilter => _superChannelFilter;

        /// <summary>
        /// Membership filter. Default is SbPublicGroupChannelMembershipFilter.Joined.
        /// </summary>
        /// @since 4.0.0
        public SbPublicGroupChannelMembershipFilter PublicMembershipFilter => _publicMembershipFilter;

        /// <summary>
        /// Checks whether query result includes empty channels. (channels without messages).
        /// </summary>
        /// @since 4.0.0
        public bool IncludeEmptyChannel => _includeEmptyChannel;

        /// <summary>
        /// Checks whether query result includes frozen channels.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeFrozenChannel => _includeFrozenChannel;

        /// <summary>
        /// A channel name filter. GroupChannel list containing the passed channel name will be returned. If you pass name such as "abc", then the returned channel list will be containing name like "abc". It will return null if channel name filter hasn't been set before.
        /// </summary>
        /// @since 4.0.0
        public string ChannelNameContainsFilter => _channelNameContainsFilter;

        /// <summary>
        /// List of custom type filter. GroupChannel list containing only and exactly the passed custom types will be returned. It will return null if custom types filter hasn't been set before.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> CustomTypesFilter => _customTypesFilter;

        /// <summary>
        /// A filter to return channels that start with the specified Custom Type. It will return null if custom type starts with filter hasn't been set before.
        /// </summary>
        /// @since 4.0.0
        public string CustomTypeStartsWithFilter => _customTypeStartsWithFilter;

        /// <summary>
        /// Whether to include channel metadata on fetch.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeMetaData => _includeMetaData;

        /// <summary>
        /// The metadataKey set with either MetaDataValuesFilter or MetaDataValueStartsWithFilter.
        /// </summary>
        /// @since 4.0.0
        public string MetaDataKeyFilter => _metaDataKeyFilter;

        /// <summary>
        /// Meta data order key filter. It will return null if meta data order key filter hasn't been set before.
        /// </summary>
        /// @since 4.0.0
        public string MetaDataOrderKeyFilter => _metaDataOrderKeyFilter;

        /// <summary>
        /// Works exclusively with MetaDataValuesFilter.
        /// </summary>
        /// @since 4.0.0
        public string MetaDataValueStartsWithFilter => _metaDataValueStartsWithFilter;

        /// <summary>
        /// Works exclusively with MetaDataValueStartsWithFilter.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> MetaDataValuesFilter => _metaDataValuesFilter;

        /// <summary>
        /// The maximum number of GroupChannels per page.
        /// </summary>
        /// @since 4.0.0
        public int Limit => _limit;

        /// <summary>
        /// Whether there is a next page.
        /// </summary>
        /// @since 4.0.0
        public bool HasNext => _hasNext;

        /// <summary>
        /// Whether the current query is in communication progress with server.
        /// </summary>
        /// @since 4.0.0
        public bool IsLoading => _isLoading;

        /// <summary>
        /// Gets the list of GroupChannels. The queried result is passed to handler as list. If this method is repeatedly called after each next is finished, it retrieves the following pages of the GroupChannel list. If there is no more pages to be read, an empty list (not null). is returned to handler.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void LoadNextPage(SbGroupChannelListHandler inCompletionHandler)
        {
            LoadNextPageInternal(inCompletionHandler);
        }
    }
}