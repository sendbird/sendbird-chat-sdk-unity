// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbOperatorListQuery
    {
        private readonly int _limit;

        private readonly SendbirdChatMainContext _chatMainContextRef;
        private readonly string _channelUrl;
        private readonly SbChannelType _channelType;
        private bool _hasNext;
        private bool _isLoading = false;
        private string _token;

        internal SbOperatorListQuery(SbChannelType inChannelType, string inChannelUrl, SbOperatorListQueryParams inQueryParams, SendbirdChatMainContext inChatMainContext)
        {
            _chatMainContextRef = inChatMainContext;
            _channelType = inChannelType;
            _channelUrl = inChannelUrl;
            _hasNext = true;
            _isLoading = false;

            if (inQueryParams == null)
                return;

            _limit = inQueryParams.Limit;
        }

        private void LoadNextPageInternal(SbUserListHandler inCompletionHandler)
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

                if (inError != null || !(inResponse is OperatorListQueryApiCommand.Response queryResponse))
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                _token = queryResponse.token;
                _hasNext = !string.IsNullOrEmpty(_token);
                
                List<SbUser> resultUsers = null;
                if (queryResponse.userDtos != null && 0 < queryResponse.userDtos.Count)
                {
                    resultUsers = new List<SbUser>(queryResponse.userDtos.Count);
                    foreach (UserDto participantDto in queryResponse.userDtos)
                    {
                        SbUser user = new SbUser(participantDto, _chatMainContextRef);
                        resultUsers.Add(user);
                    }
                }

                inCompletionHandler?.Invoke(resultUsers, null);
            }

            OperatorListQueryApiCommand.Request request = new OperatorListQueryApiCommand.Request(_token, _limit, _channelType, _channelUrl, OnResponse);

            _chatMainContextRef.CommandRouter.RequestApiCommand(request);
        }
    }
}