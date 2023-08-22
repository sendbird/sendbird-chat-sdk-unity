// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbBlockedUserListQuery
    {
        private readonly List<string> _userIdsFilter;
        private readonly int _limit;

        private readonly SendbirdChatMainContext _chatMainContextRef = null;
        private bool _hasNext;
        private bool _isLoading;
        private string _token;

        internal SbBlockedUserListQuery(SbBlockedUserListQueryParams inQueryParams, SendbirdChatMainContext inChatMainContext)
        {
            _chatMainContextRef = inChatMainContext;
            _hasNext = true;
            _isLoading = false;

            if (inQueryParams == null)
                return;

            _userIdsFilter = inQueryParams.UserIdsFilter;
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

                if (inError != null || !(inResponse is BlockedUserListQueryApiCommand.Response userListQueryResponse))
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                List<SbUser> resultUsers = null;
                if (userListQueryResponse.userDtos != null && 0 < userListQueryResponse.userDtos.Count)
                {
                    resultUsers = new List<SbUser>(userListQueryResponse.userDtos.Count);
                    foreach (UserDto userDto in userListQueryResponse.userDtos)
                    {
                        _token = userListQueryResponse.token;
                        _hasNext = !string.IsNullOrEmpty(_token);
                        SbUser user = new SbUser(userDto, _chatMainContextRef);
                        resultUsers.Add(user);
                    }
                }

                inCompletionHandler?.Invoke(resultUsers, null);
            }
            
            BlockedUserListQueryApiCommand.Request request = new BlockedUserListQueryApiCommand.Request(_token, _limit, _chatMainContextRef.CurrentUserId, _userIdsFilter, OnResponse);
            _chatMainContextRef.CommandRouter.RequestApiCommand(request);
        }
    }
}