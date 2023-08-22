// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// The GroupChannelListQuery class is a query class for getting the list of group channels.
    /// </summary>
    /// @since 4.0.0
    public partial class SbGroupChannelListQuery
    {
        /// <summary>
        /// Search fields. Refer to SbGroupChannelListQueryParams.SetSearchFilter and SearchField.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<SbGroupChannelListQuerySearchField> SearchFields => _searchFields;

        /// <summary>
        /// List of channel URL filter. It will return null if channel URL filter hasn't been set before. GroupChannel list containing only and exactly the passed GroupChannel URLs will be returned.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> ChannelUrlsFilter => _channelUrlsFilter;

        /// <summary>
        /// User IDs exact filter.GroupChannel list containing only and exactly the passed User IDs will be returned. This does not cooperate with other filters.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> UserIdsExactFilter => _userIdsExactFilter;

        /// <summary>
        /// User IDs include filter. It will return null if User IDs include filter hasn't been set before. This does not cooperate with other filters.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> UserIdsIncludeFilter => _userIdsIncludeFilter;

        /// <summary>
        /// Sets query type for includeMemberList.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelListQueryType QueryType => _queryType;

        /// <summary>
        /// Searches for GroupChannels with members whose nicknames contain the specified value. If you pass nickname such as "abc", then the returned channel list will be containing member like "abc". This does not cooperate with other filters.
        /// </summary>
        /// @since 4.0.0
        public string NicknameContainsFilter => _nicknameContainsFilter;

        /// <summary>
        /// Searches for GroupChannels with members whose nicknames match the specified value. This does not cooperate with other filters.
        /// </summary>
        /// @since 4.0.0
        public string NicknameExactMatchFilter => _nicknameExactMatchFilter;

        /// <summary>
        /// Searches for GroupChannels with members whose nicknames starts with the specified value. If you pass nickname such as "abc", then the returned channel list will be containing member like "abc*". This does not cooperate with other filters.
        /// </summary>
        /// @since 4.0.0
        public string NicknameStartsWithFilter => _nicknameStartsWithFilter;

        /// <summary>
        /// Sets to filter channels by the hidden state.
        /// </summary>
        /// @since 4.0.0
        public SbChannelHiddenStateFilter ChannelHiddenStateFilter => _channelHiddenStateFilter;

        /// <summary>
        /// Result order of channels. Refer to GroupChannelListQueryOrder. SbGroupChannelListOrder.ChannelMetaDataValueAlphabetical works with metaDataOrderKeyFilter.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelListOrder Order => _order;

        /// <summary>
        /// Public channel filter. Refer to SbGroupChannelPublicChannelFilter.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelPublicChannelFilter PublicChannelFilter => _publicChannelFilter;

        /// <summary>
        /// Super channel filter. Refer to SbGroupChannelSuperChannelFilter.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelSuperChannelFilter SuperChannelFilter => _superChannelFilter;

        /// <summary>
        /// A filter to return channels with the current User state matching to SbMyMemberStateFilter.
        /// </summary>
        /// @since 4.0.0
        public SbMyMemberStateFilter MyMemberStateFilter => _myMemberStateFilter;

        /// <summary>
        /// Unread channel filter. Refer to SbUnreadChannelFilter.
        /// </summary>
        /// @since 4.0.0
        public SbUnreadChannelFilter UnreadChannelFilter => _unreadChannelFilter;

        /// <summary>
        /// Whether to include chat notification GroupChannel.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeChatNotification => _includeChatNotification;

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
        /// Determines channel object of the list includes members list.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeMemberList => _includeMemberList;

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
        /// Query string.
        /// </summary>
        /// @since 4.0.0
        public string SearchQuery => _searchQuery;

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

        /// <summary>
        /// Verify that the given channel information matches the current query filter.
        /// </summary>
        /// <param name="inGroupChannel"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public bool BelongsTo(SbGroupChannel inGroupChannel)
        {
            return BelongsToInternal(inGroupChannel);
        }
    }
}