// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Params for creating a GroupChannelListQuery object.
    /// </summary>
    /// @since 4.0.0
    public partial class SbGroupChannelListQueryParams
    {
        /// <summary>
        /// Sets to filter channels by the hidden state. The default value is SbChannelHiddenStateFilter.UnhiddenOnly.
        /// </summary>
        /// @since 4.0.0
        public SbChannelHiddenStateFilter ChannelHiddenStateFilter { get => _channelHiddenStateFilter; set => _channelHiddenStateFilter = value; }

        /// <summary>
        /// Sets the order of the list. The order is defined in SbGroupChannelListOrder.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelListOrder Order { get => _order; set => _order = value; }

        /// <summary>
        /// Sets query type for includeMemberList.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelListQueryType QueryType { get => _queryType; set => _queryType = value; }

        /// <summary>
        /// Sets to filter public channel.Default is All
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelPublicChannelFilter PublicChannelFilter { get => _publicChannelFilter; set => _publicChannelFilter = value; }

        /// <summary>
        /// Sets to filter super channel. Default is All
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelSuperChannelFilter SuperChannelFilter { get => _superChannelFilter; set => _superChannelFilter = value; }

        /// <summary>
        /// Sets the member state filter.
        /// </summary>
        /// @since 4.0.0
        public SbMyMemberStateFilter MyMemberStateFilter { get => _myMemberStateFilter; set => _myMemberStateFilter = value; }

        /// <summary>
        /// Unread channel filter. Refer to SbUnreadChannelFilter.
        /// </summary>
        /// @since 4.0.0
        public SbUnreadChannelFilter UnreadChannelFilter { get => _unreadChannelFilter; set => _unreadChannelFilter = value; }

        /// <summary>
        /// Determines channel list includes empty channel. Default is true
        /// </summary>
        /// @since 4.0.0
        public bool IncludeEmptyChannel { get => _includeEmptyChannel; set => _includeEmptyChannel = value; }

        /// <summary>
        /// Determines channel list includes frozen channel. Default is true
        /// </summary>
        /// @since 4.0.0
        public bool IncludeFrozenChannel { get => _includeFrozenChannel; set => _includeFrozenChannel = value; }

        /// <summary>
        /// Determines channel object of the list includes members list.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeMemberList { get => _includeMemberList; set => _includeMemberList = value; }

        /// <summary>
        /// 
        /// </summary>
        /// @since 4.0.0
        public bool IncludeChatNotification { get => _includeChatNotification; set => _includeChatNotification = value; }

        /// <summary>
        /// The maximum number of items per queried page.
        /// </summary>
        /// @since 4.0.0
        public int Limit { get => _limit; set => _limit = value; }

        /// <summary>
        /// A channel name filter. GroupChannel list containing the passed channel name will be returned. If you pass name such as "abc", then the returned channel list will be containing name like "abc". It will return null if channel name filter hasn't been set before.
        /// </summary>
        /// @since 4.0.0
        public string ChannelNameContainsFilter { get => _channelNameContainsFilter; set => _channelNameContainsFilter = value; }

        /// <summary>
        /// Sets GroupChannel URLs filter. GroupChannel list containing only and exactly the passed GroupChannel URLs will be returned. This does not cooperate with other filters.
        /// </summary>
        /// @since 4.0.0
        public List<string> ChannelUrlsFilter { get => _channelUrlsFilter; set => _channelUrlsFilter = value; }

        /// <summary>
        /// Sets the custom types filter. The custom types to search.
        /// </summary>
        /// @since 4.0.0
        public List<string> CustomTypesFilter { get => _customTypesFilter; set => _customTypesFilter = value; }

        /// <summary>
        /// Sets to filter channels by custom type that starts with.
        /// </summary>
        /// @since 4.0.0
        public string CustomTypeStartsWithFilter { get => _customTypeStartsWithFilter; set => _customTypeStartsWithFilter = value; }

        /// <summary>
        /// Checks whether to include channel metadata on fetch. This flag is true by default.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeMetaData { get => _includeMetaData; set => _includeMetaData = value; }

        /// <summary>
        /// Works exclusively with MetaDataValueStartsWithFilter.
        /// </summary>
        /// @since 4.0.0
        public List<string> MetaDataValuesFilter => _metaDataValuesFilter;

        /// <summary>
        /// The metadataKey set with either metaDataValues or MetaDataValueStartsWithFilter.
        /// </summary>
        /// @since 4.0.0
        public string MetaDataKeyFilter => _metaDataKeyFilter;

        /// <summary>
        /// Meta data order key filter. It will return null if meta data order key filter hasn't been set before.
        /// </summary>
        /// @since 4.0.0
        public string MetaDataOrderKeyFilter { get => _metaDataOrderKeyFilter; set => _metaDataOrderKeyFilter = value; }

        /// <summary>
        /// Works exclusively with MetaDataValues.
        /// </summary>
        /// @since 4.0.0
        public string MetaDataValueStartsWithFilter => _metaDataValueStartsWithFilter;

        /// <summary>
        /// Sets the filter with nickname.
        /// </summary>
        /// @since 4.0.0
        public string NicknameContainsFilter { get => _nicknameContainsFilter; set => SetNicknameContainsFilter(value); }

        /// <summary>
        /// Sets the filter with nickname.
        /// </summary>
        /// @since 4.0.0
        public string NicknameExactMatchFilter { get => _nicknameExactMatchFilter; set => _nicknameExactMatchFilter = value; }

        /// <summary>
        /// Sets the filter with nickname prefix.
        /// </summary>
        /// @since 4.0.0
        public string NicknameStartsWithFilter { get => _nicknameStartsWithFilter; set => _nicknameStartsWithFilter = value; }

        /// <summary>
        /// The query property of the query specified.
        /// </summary>
        /// @since 4.0.0
        public string SearchQuery => _searchQuery;

        /// <summary>
        /// The fields properties of the query specified.
        /// </summary>
        /// @since 4.0.0
        public List<SbGroupChannelListQuerySearchField> SearchFields => _searchFields;

        /// <summary>
        /// Sets the filter with user IDs.
        /// </summary>
        /// @since 4.0.0
        public List<string> UserIdsExactFilter { get => _userIdsExactFilter; set => SetUserIdsExactFilter(value); }

        /// <summary>
        /// Sets the filter with user IDs.
        /// </summary>
        /// @since 4.0.0
        public List<string> UserIdsIncludeFilter => _userIdsIncludeFilter;
        
        /// <summary>
        /// Sets User IDs filter. GroupChannel list containing the passed User IDs and other members will be returned. This does not cooperate with other filters.
        /// </summary>
        /// <param name="inUserIds"></param>
        /// <param name="inQueryType"></param>
        /// @since 4.0.0
        public void SetUserIdsIncludeFilter(List<string> inUserIds, SbGroupChannelListQueryType inQueryType)
        {
            SetUserIdsIncludeFilterInternal(inUserIds, inQueryType);
        }

        /// <summary>
        /// Sets Search filter. GroupChannels will be included in the result if its data in specified SearchFields contains specified query string. Refer to SearchField. If you set multiple SbGroupChannelListQuerySearchField, the result will be union of each result.
        /// </summary>
        /// <param name="inQuery"></param>
        /// <param name="inSearchFields"></param>
        /// @since 4.0.0
        public void SetSearchFilter(string inQuery, List<SbGroupChannelListQuerySearchField> inSearchFields)
        {
            SetSearchFilterInternal(inQuery, inSearchFields);
        }

        /// <summary>
        /// Sets GroupChannel meta data filter. GroupChannel list containing only and exactly the passed GroupChannel meta data will be returned. If this is set, it will reset the filter set from MetaDataValueStartsWith.
        /// </summary>
        /// <param name="inMetaDataKey"></param>
        /// <param name="inMetaDataValues"></param>
        /// @since 4.0.0
        public void SetMetaDataValuesFilter(string inMetaDataKey, List<string> inMetaDataValues)
        {
            SetMetaDataValuesFilterInternal(inMetaDataKey, inMetaDataValues);
        }

        /// <summary>
        /// Sets GroupChannel meta data filter. GroupChannel list containing passed GroupChannel meta data key with values that starts with the passed on value will be returned. If this is set, it will reset the filter set from MetaDataValues.
        /// </summary>
        /// <param name="inMetaDataKey"></param>
        /// <param name="inMetaDataValueStartsWith"></param>
        /// @since 4.0.0
        public void SetMetaDataValueStartsWithFilter(string inMetaDataKey, string inMetaDataValueStartsWith)
        {
            SetMetaDataValueStartsWithFilterInternal(inMetaDataKey, inMetaDataValueStartsWith);
        }
    }
}