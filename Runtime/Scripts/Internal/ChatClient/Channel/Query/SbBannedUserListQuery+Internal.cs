// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbBannedUserListQuery
    {
        private readonly int _limit;

        private readonly SendbirdChatMainContext _chatMainContextRef;
        private readonly string _channelUrl;
        private readonly SbChannelType _channelType;
        private bool _hasNext;
        private bool _isLoading = false;
        private string _token;

        internal SbBannedUserListQuery(SbChannelType inChannelType, string inChannelUrl, SbBannedUserListQueryParams inQueryParams, SendbirdChatMainContext inChatMainContext)
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

                if (inError != null || !(inResponse is BannedUserListQueryApiCommand.Response queryResponse))
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                _token = queryResponse.token;
                _hasNext = !string.IsNullOrEmpty(_token);
                
                List<SbRestrictedUser> resultUsers = null;
                if (queryResponse.restrictedUserDtos != null && 0 < queryResponse.restrictedUserDtos.Count)
                {
                    resultUsers = new List<SbRestrictedUser>(queryResponse.restrictedUserDtos.Count);
                    foreach (RestrictedUserDto restrictedUserDto in queryResponse.restrictedUserDtos)
                    {
                        SbRestrictedUser user = new SbRestrictedUser(restrictedUserDto, _chatMainContextRef);
                        resultUsers.Add(user);
                    }
                }

                inCompletionHandler?.Invoke(resultUsers, null);
            }

            BannedUserListQueryApiCommand.Request request = new BannedUserListQueryApiCommand.Request(_token, _limit, _channelType, _channelUrl, OnResponse);

            _chatMainContextRef.CommandRouter.RequestApiCommand(request);
        }
    }
}