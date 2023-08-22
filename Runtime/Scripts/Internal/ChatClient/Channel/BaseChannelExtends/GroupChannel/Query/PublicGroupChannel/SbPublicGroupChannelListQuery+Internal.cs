// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbPublicGroupChannelListQuery
    {
        private readonly string _channelNameContainsFilter;
        private readonly List<string> _channelUrlsFilter;
        private readonly List<string> _customTypesFilter;
        private readonly string _customTypeStartsWithFilter;
        private readonly bool _includeEmptyChannel;
        private readonly bool _includeFrozenChannel;
        private readonly bool _includeMetaData;
        private readonly int _limit;
        private readonly SbPublicGroupChannelMembershipFilter _publicMembershipFilter;
        private readonly string _metaDataKeyFilter;
        private readonly string _metaDataOrderKeyFilter;
        private readonly List<string> _metaDataValuesFilter;
        private readonly string _metaDataValueStartsWithFilter;
        private readonly SbPublicGroupChannelListOrder _order;
        private readonly SbGroupChannelSuperChannelFilter _superChannelFilter;

        private string _token = null;
        private bool _hasNext = true;
        private bool _isLoading = false;
        private readonly SendbirdChatMainContext _chatMainContextRef;

        internal SbPublicGroupChannelListQuery(SbPublicGroupChannelListQueryParams inQueryParams, SendbirdChatMainContext inChatMainContext)
        {
            _channelUrlsFilter = inQueryParams.ChannelUrlsFilter;
            _customTypesFilter = inQueryParams.CustomTypesFilter;
            _metaDataValuesFilter = inQueryParams.MetaDataValuesFilter;
            _order = inQueryParams.Order;
            _superChannelFilter = inQueryParams.SuperChannelFilter;
            _includeEmptyChannel = inQueryParams.IncludeEmptyChannel;
            _includeFrozenChannel = inQueryParams.IncludeFrozenChannel;
            _includeMetaData = inQueryParams.IncludeMetaData;
            _limit = inQueryParams.Limit;
            _publicMembershipFilter = inQueryParams.PublicMembershipFilter;
            _channelNameContainsFilter = inQueryParams.ChannelNameContainsFilter;
            _customTypeStartsWithFilter = inQueryParams.CustomTypeStartsWithFilter;
            _metaDataKeyFilter = inQueryParams.MetaDataKeyFilter;
            _metaDataOrderKeyFilter = inQueryParams.MetaDataOrderKeyFilter;
            _metaDataValueStartsWithFilter = inQueryParams.MetaDataValueStartsWithFilter;

            _chatMainContextRef = inChatMainContext;
        }

        private void LoadNextPageInternal(SbGroupChannelListHandler inCompletionHandler)
        {
            if (_isLoading)
            {
                SbError error = new SbError(SbErrorCode.QueryInProgress);
                CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler?.Invoke(null, error); });
                return;
            }

            _isLoading = true;

            void OnResponse(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                _isLoading = false;

                if (inError != null || !(inResponse is PublicGroupChannelListQueryApiCommand.Response groupChannelListQueryResponse))
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                List<SbGroupChannel> resultOpenChannels = null;
                if (groupChannelListQueryResponse.groupChannelDtos != null && 0 < groupChannelListQueryResponse.groupChannelDtos.Count)
                {
                    List<SbGroupChannel> tempOpenChannels = new List<SbGroupChannel>(groupChannelListQueryResponse.groupChannelDtos.Count);
                    foreach (GroupChannelDto channelDto in groupChannelListQueryResponse.groupChannelDtos)
                    {
                        _token = groupChannelListQueryResponse.token;
                        _hasNext = !string.IsNullOrEmpty(_token);
                        SbGroupChannel openChannel = _chatMainContextRef.GroupChannelManager.CreateOrUpdateChannel(channelDto);
                        tempOpenChannels.Add(openChannel);
                    }

                    resultOpenChannels = new List<SbGroupChannel>(tempOpenChannels);
                }

                inCompletionHandler?.Invoke(resultOpenChannels, null);
            }
            
            PublicGroupChannelListQueryApiCommand.Request request = new PublicGroupChannelListQueryApiCommand.Request(_token, this, OnResponse);

            _chatMainContextRef.CommandRouter.RequestApiCommand(request);
        }
    }
}