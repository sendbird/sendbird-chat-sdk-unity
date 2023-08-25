// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbOpenChannelListQuery
    {
        private readonly string _nameKeyword;
        private readonly string _urlKeyword;
        private readonly string _customTypeFilter;
        private readonly bool _includeFrozen;
        private readonly bool _includeMetadata;
        private readonly int _limit;

        private readonly SendbirdChatMainContext _chatMainContextRef = null;
        private bool _hasNext;
        private bool _isLoading;
        private string _token;

        internal SbOpenChannelListQuery(SbOpenChannelListQueryParams inQueryParams, SendbirdChatMainContext inChatMainContext)
        {
            _chatMainContextRef = inChatMainContext;
            _hasNext = true;
            _isLoading = false;

            if (inQueryParams == null)
                return;

            _nameKeyword = inQueryParams.NameKeyword;
            _urlKeyword = inQueryParams.UrlKeyword;
            _customTypeFilter = inQueryParams.CustomTypeFilter;
            _includeFrozen = inQueryParams.IncludeFrozen;
            _includeMetadata = inQueryParams.IncludeMetadata;
            _limit = inQueryParams.Limit;
        }

        private void LoadNextPageInternal(SbOpenChannelListHandler inCompletionHandler)
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

                if (inError != null || !(inResponse is OpenChannelListQueryApiCommand.Response openChannelListQueryResponse))
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                _token = openChannelListQueryResponse.token;
                _hasNext = !string.IsNullOrEmpty(_token);
                
                List<SbOpenChannel> resultOpenChannels = null;
                if (openChannelListQueryResponse.openChannelDtos != null && 0 < openChannelListQueryResponse.openChannelDtos.Count)
                {
                    List<SbOpenChannel> tempOpenChannels = new List<SbOpenChannel>(openChannelListQueryResponse.openChannelDtos.Count);
                    foreach (OpenChannelDto channelDto in openChannelListQueryResponse.openChannelDtos)
                    {
                        SbOpenChannel openChannel = _chatMainContextRef.OpenChannelManager.CreateOrUpdateChannel(channelDto);
                        tempOpenChannels.Add(openChannel);
                    }

                    resultOpenChannels = new List<SbOpenChannel>(tempOpenChannels);
                }

                inCompletionHandler?.Invoke(resultOpenChannels, null);
            }

            OpenChannelListQueryApiCommand.Request request = new OpenChannelListQueryApiCommand.Request(
                _token, _limit, _nameKeyword, _urlKeyword, _customTypeFilter, _includeFrozen, _includeMetadata, OnResponse);

            _chatMainContextRef.CommandRouter.RequestApiCommand(request);
        }
    }
}