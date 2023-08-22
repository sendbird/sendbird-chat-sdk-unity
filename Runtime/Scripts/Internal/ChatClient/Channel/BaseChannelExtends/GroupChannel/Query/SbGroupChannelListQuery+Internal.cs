// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sendbird.Chat
{
    public partial class SbGroupChannelListQuery
    {
        private readonly bool _includeEmptyChannel;
        private readonly bool _includeFrozenChannel;
        private readonly bool _includeMemberList;
        private readonly bool _includeMetaData;
        private readonly SbGroupChannelListQueryType _queryType;
        private readonly SbMyMemberStateFilter _myMemberStateFilter;
        private readonly List<string> _channelUrlsFilter;
        private readonly SbGroupChannelPublicChannelFilter _publicChannelFilter;
        private readonly SbGroupChannelSuperChannelFilter _superChannelFilter;
        private readonly string _customTypeStartsWithFilter;
        private readonly List<string> _customTypesFilter;
        private readonly string _nicknameContainsFilter;
        private readonly string _nicknameExactMatchFilter;
        private readonly string _nicknameStartsWithFilter;
        private readonly List<string> _userIdsExactFilter;
        private readonly List<string> _userIdsIncludeFilter;
        private readonly string _channelNameContainsFilter;
        private readonly SbUnreadChannelFilter _unreadChannelFilter;
        private readonly string _metaDataKeyFilter;
        private readonly List<string> _metaDataValuesFilter;
        private readonly string _metaDataOrderKeyFilter;
        private readonly string _metaDataValueStartsWithFilter;
        private readonly SbChannelHiddenStateFilter _channelHiddenStateFilter;
        private readonly string _searchQuery;
        private readonly List<SbGroupChannelListQuerySearchField> _searchFields;
        private readonly SbGroupChannelListOrder _order;
        private readonly bool _includeChatNotification;
        private readonly int _limit;

        private string _token = null;
        private bool _hasNext;
        private bool _isLoading;
        private readonly SendbirdChatMainContext _chatMainContextRef;

        internal SbGroupChannelListQuery(SbGroupChannelListQueryParams inQueryParams, SendbirdChatMainContext inChatMainContext)
        {
            _chatMainContextRef = inChatMainContext;
            _hasNext = true;
            _isLoading = false;

            if (inQueryParams == null)
                return;

            _searchFields = inQueryParams.SearchFields;
            _channelUrlsFilter = inQueryParams.ChannelUrlsFilter;
            _customTypesFilter = inQueryParams.CustomTypesFilter;
            _metaDataValuesFilter = inQueryParams.MetaDataValuesFilter;
            _userIdsExactFilter = inQueryParams.UserIdsExactFilter;
            _userIdsIncludeFilter = inQueryParams.UserIdsIncludeFilter;
            _channelHiddenStateFilter = inQueryParams.ChannelHiddenStateFilter;
            _order = inQueryParams.Order;
            _queryType = inQueryParams.QueryType;
            _publicChannelFilter = inQueryParams.PublicChannelFilter;
            _superChannelFilter = inQueryParams.SuperChannelFilter;
            _myMemberStateFilter = inQueryParams.MyMemberStateFilter;
            _unreadChannelFilter = inQueryParams.UnreadChannelFilter;
            _includeEmptyChannel = inQueryParams.IncludeEmptyChannel;
            _includeFrozenChannel = inQueryParams.IncludeFrozenChannel;
            _includeMemberList = inQueryParams.IncludeMemberList;
            _includeMetaData = inQueryParams.IncludeMetaData;
            _includeChatNotification = inQueryParams.IncludeChatNotification;
            _limit = inQueryParams.Limit;
            _channelNameContainsFilter = inQueryParams.ChannelNameContainsFilter;
            _customTypeStartsWithFilter = inQueryParams.CustomTypeStartsWithFilter;
            _metaDataKeyFilter = inQueryParams.MetaDataKeyFilter;
            _metaDataOrderKeyFilter = inQueryParams.MetaDataOrderKeyFilter;
            _metaDataValueStartsWithFilter = inQueryParams.MetaDataValueStartsWithFilter;
            _nicknameContainsFilter = inQueryParams.NicknameContainsFilter;
            _nicknameExactMatchFilter = inQueryParams.NicknameExactMatchFilter;
            _nicknameStartsWithFilter = inQueryParams.NicknameStartsWithFilter;
            _searchQuery = inQueryParams.SearchQuery;
        }

        private void LoadNextPageInternal(SbGroupChannelListHandler inCompletionHandler)
        {
            if (_isLoading)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler?.Invoke(null, SbErrorCodeExtension.QUERY_IN_PROGRESS); });
                return;
            }

            _isLoading = true;

            void OnResponse(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                _isLoading = false;

                if (inError != null || !(inResponse is MyGroupChannelListQueryApiCommand.Response groupChannelListQueryResponse))
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                _token = groupChannelListQueryResponse.token;
                _hasNext = !string.IsNullOrEmpty(_token);
                
                List<SbGroupChannel> resultOpenChannels = null;
                if (groupChannelListQueryResponse.groupChannelDtos != null && 0 < groupChannelListQueryResponse.groupChannelDtos.Count)
                {
                    List<SbGroupChannel> tempOpenChannels = new List<SbGroupChannel>(groupChannelListQueryResponse.groupChannelDtos.Count);
                    foreach (GroupChannelDto channelDto in groupChannelListQueryResponse.groupChannelDtos)
                    {
                        SbGroupChannel openChannel = _chatMainContextRef.GroupChannelManager.CreateOrUpdateChannel(channelDto);
                        tempOpenChannels.Add(openChannel);
                    }

                    resultOpenChannels = new List<SbGroupChannel>(tempOpenChannels);
                }

                inCompletionHandler?.Invoke(resultOpenChannels, null);
            }

            MyGroupChannelListQueryApiCommand.Request request = new MyGroupChannelListQueryApiCommand.Request(_chatMainContextRef.CurrentUserId, _token, this, OnResponse);

            _chatMainContextRef.CommandRouter.RequestApiCommand(request);
        }

        private bool BelongsToInternal(SbGroupChannel inGroupChannel)
        {
            if (_includeEmptyChannel == false && inGroupChannel.LastMessage == null)
                return false;

            if (_includeFrozenChannel == false && inGroupChannel.IsFrozen)
                return false;

            if (_superChannelFilter == SbGroupChannelSuperChannelFilter.SuperChannelOnly && inGroupChannel.IsSuper == false)
                return false;

            if (_superChannelFilter == SbGroupChannelSuperChannelFilter.NonSuperChannelOnly && inGroupChannel.IsSuper)
                return false;

            if (_superChannelFilter == SbGroupChannelSuperChannelFilter.BroadcastOnly && inGroupChannel.IsBroadcast == false)
                return false;

            if (_publicChannelFilter == SbGroupChannelPublicChannelFilter.Public && inGroupChannel.IsPublic == false)
                return false;

            if (_publicChannelFilter == SbGroupChannelPublicChannelFilter.Private && inGroupChannel.IsPublic)
                return false;

            if (inGroupChannel.IsPublic && inGroupChannel.MyMemberState == SbMemberState.None)
                return false;

            if (_channelHiddenStateFilter == SbChannelHiddenStateFilter.UnhiddenOnly && inGroupChannel.IsHidden)
                return false;

            if (_channelHiddenStateFilter == SbChannelHiddenStateFilter.HiddenOnly && inGroupChannel.IsHidden == false)
                return false;

            if (_channelHiddenStateFilter == SbChannelHiddenStateFilter.HiddenAllowAutoUnhide && inGroupChannel.HiddenState != SbGroupChannelHiddenState.HiddenAllowAutoUnhide)
                return false;

            if (_channelHiddenStateFilter == SbChannelHiddenStateFilter.HiddenPreventAutoUnhide && inGroupChannel.HiddenState != SbGroupChannelHiddenState.HiddenPreventAutoUnhide)
                return false;

            if (_channelUrlsFilter != null && _channelUrlsFilter.Contains(inGroupChannel.Url) == false)
                return false;

            if (string.IsNullOrEmpty(_channelNameContainsFilter) == false && inGroupChannel.Name?.Contains(_channelNameContainsFilter) == false)
                return false;

            if (string.IsNullOrEmpty(_customTypeStartsWithFilter) == false && inGroupChannel.CustomType?.StartsWith(_customTypeStartsWithFilter) == false)
                return false;

            if (_customTypesFilter != null && _customTypesFilter.Contains(inGroupChannel.CustomType) == false)
                return false;

            if (_userIdsExactFilter != null && 0 < _userIdsExactFilter.Count)
            {
                if (_userIdsExactFilter.Count != inGroupChannel.Members.Count)
                    return false;

                foreach (SbMember member in inGroupChannel.Members)
                {
                    if (_userIdsExactFilter.Contains(member.UserId) == false)
                        return false;
                }
            }

            if (_userIdsIncludeFilter != null && 0 < _userIdsIncludeFilter.Count)
            {
                int memberCount = inGroupChannel.Members.Count(inMember => _userIdsIncludeFilter.Contains(inMember.UserId));
                if (_queryType == SbGroupChannelListQueryType.And && memberCount < _userIdsIncludeFilter.Count)
                    return false;

                if (_queryType == SbGroupChannelListQueryType.Or && memberCount == 0)
                    return false;
            }

            if (string.IsNullOrEmpty(_nicknameContainsFilter) == false)
            {
                bool OnNicknameContainsPredicate(SbMember inMember)
                {
                    if (inMember.UserId != _chatMainContextRef.CurrentUserId && inMember.Nickname.Contains(_nicknameContainsFilter))
                        return true;

                    return false;
                }

                if (inGroupChannel.Members.Any(OnNicknameContainsPredicate) == false)
                    return false;
            }

            if (string.IsNullOrEmpty(_nicknameStartsWithFilter) == false)
            {
                bool OnNicknameStartsWithPredicate(SbMember inMember)
                {
                    if (inMember.UserId != _chatMainContextRef.CurrentUserId && inMember.Nickname.StartsWith(_nicknameStartsWithFilter))
                        return true;

                    return false;
                }

                if (inGroupChannel.Members.Any(OnNicknameStartsWithPredicate) == false)
                    return false;
            }

            if (string.IsNullOrEmpty(_nicknameExactMatchFilter) == false)
            {
                bool OnNicknameExactMatchPredicate(SbMember inMember)
                {
                    if (inMember.UserId != _chatMainContextRef.CurrentUserId && inMember.Nickname.Equals(_nicknameExactMatchFilter, StringComparison.OrdinalIgnoreCase))
                        return true;

                    return false;
                }

                if (inGroupChannel.Members.Any(OnNicknameExactMatchPredicate) == false)
                    return false;
            }

            switch (_myMemberStateFilter)
            {
                case SbMyMemberStateFilter.JoinedOnly:
                {
                    if (inGroupChannel.MyMemberState != SbMemberState.Joined)
                        return false;
                    break;
                }
                case SbMyMemberStateFilter.InvitedOnly:
                case SbMyMemberStateFilter.InvitedByFriend:
                case SbMyMemberStateFilter.InvitedByNonFriend:
                {
                    if (inGroupChannel.MyMemberState != SbMemberState.Invited)
                        return false;
                    break;
                }
                case SbMyMemberStateFilter.All: break;
            }

            if (string.IsNullOrEmpty(_metaDataKeyFilter) == false && inGroupChannel.CachedMetaData != null)
            {
                if (inGroupChannel.CachedMetaData.TryGetValue(_metaDataKeyFilter, out string metaDataValue) == false)
                    return false;

                if (_metaDataValuesFilter != null && 0 < _metaDataValuesFilter.Count)
                {
                    if (string.IsNullOrEmpty(metaDataValue) || _metaDataValuesFilter.Contains(metaDataValue) == false)
                        return false;
                }
                else if (string.IsNullOrEmpty(_metaDataValueStartsWithFilter) == false)
                {
                    if (string.IsNullOrEmpty(metaDataValue) || metaDataValue.StartsWith(_metaDataValueStartsWithFilter) == false)
                        return false;
                }
            }

            return true;
        }
    }
}