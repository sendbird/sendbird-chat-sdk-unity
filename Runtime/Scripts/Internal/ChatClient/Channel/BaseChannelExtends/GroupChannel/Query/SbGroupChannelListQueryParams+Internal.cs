// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbGroupChannelListQueryParams
    {
        private SbChannelHiddenStateFilter _channelHiddenStateFilter = SbChannelHiddenStateFilter.UnhiddenOnly;
        private SbGroupChannelListOrder _order = SbGroupChannelListOrder.Chronological;
        private SbGroupChannelListQueryType _queryType = SbGroupChannelListQueryType.And;
        private SbGroupChannelPublicChannelFilter _publicChannelFilter = SbGroupChannelPublicChannelFilter.All;
        private SbGroupChannelSuperChannelFilter _superChannelFilter = SbGroupChannelSuperChannelFilter.All;
        private SbMyMemberStateFilter _myMemberStateFilter = SbMyMemberStateFilter.All;
        private SbUnreadChannelFilter _unreadChannelFilter = SbUnreadChannelFilter.All;
        private bool _includeChatNotification = false;
        private bool _includeEmptyChannel = true;
        private bool _includeFrozenChannel = true;
        private bool _includeMemberList = true;
        private int _limit = SendbirdChatMainContext.QUERY_DEFAULT_LIMIT;
        private string _channelNameContainsFilter = null;
        private List<string> _channelUrlsFilter = null;
        private List<string> _customTypesFilter = null;
        private string _customTypeStartsWithFilter = null;
        private bool _includeMetaData = true;
        private List<string> _metaDataValuesFilter = null;
        private string _metaDataKeyFilter = null;
        private string _metaDataOrderKeyFilter = null;
        private string _metaDataValueStartsWithFilter = null;
        private string _nicknameContainsFilter = null;
        private string _nicknameExactMatchFilter = null;
        private string _nicknameStartsWithFilter = null;
        private string _searchQuery = null;
        private List<SbGroupChannelListQuerySearchField> _searchFields = null;
        private List<string> _userIdsExactFilter = null;
        private List<string> _userIdsIncludeFilter = null;


        private void SetNicknameContainsFilter(string inNickname)
        {
            _nicknameContainsFilter = inNickname;
            _userIdsExactFilter = null;
            _userIdsIncludeFilter = null;
        }
        
        private void SetUserIdsExactFilter(List<string> inUserIds)
        {
            _userIdsExactFilter = inUserIds;
            _nicknameContainsFilter = null;
            _userIdsIncludeFilter = null;
        }
        
        private void SetUserIdsIncludeFilterInternal(List<string> inUserIds, SbGroupChannelListQueryType inQueryType)
        {
            _userIdsIncludeFilter = inUserIds;
            _queryType = inQueryType;
            _nicknameContainsFilter = null;
            _userIdsExactFilter = null;
        }

        private void SetSearchFilterInternal(string inQuery, List<SbGroupChannelListQuerySearchField> inSearchFields)
        {
            _searchQuery = inQuery;
            _searchFields = inSearchFields;
        }

        private void SetMetaDataValuesFilterInternal(string inMetaDataKey, List<string> inMetaDataValues)
        {
            _metaDataValueStartsWithFilter = null;
            _metaDataKeyFilter = inMetaDataKey;
            _metaDataValuesFilter = inMetaDataValues;
        }

        private void SetMetaDataValueStartsWithFilterInternal(string inMetaDataKey, string inMetaDataValueStartsWith)
        {
            _metaDataValuesFilter = null;
            _metaDataValueStartsWithFilter = inMetaDataValueStartsWith;
            _metaDataKeyFilter = inMetaDataKey;
        }
    }
}