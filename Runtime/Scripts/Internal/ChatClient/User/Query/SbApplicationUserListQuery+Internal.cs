// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbApplicationUserListQuery
    {
        private readonly List<string> _userIdsFilter;
        private readonly string _nicknameStartsWithFilter;
        private readonly int _limit;
        private readonly string _metaDataKeyFilter;
        private readonly List<string> _metaDataValuesFilter;

        private readonly SendbirdChatMainContext _chatMainContextRef = null;
        private bool _hasNext;
        private bool _isLoading;
        private string _token;

        internal SbApplicationUserListQuery(SbApplicationUserListQueryParams inQueryParams, SendbirdChatMainContext inChatMainContext)
        {
            _chatMainContextRef = inChatMainContext;
            _hasNext = true;
            _isLoading = false;
            
            if (inQueryParams == null)
                return;

            _userIdsFilter = inQueryParams.UserIdsFilter;
            _nicknameStartsWithFilter = inQueryParams.NicknameStartsWithFilter;
            _metaDataKeyFilter = inQueryParams.MetaDataKeyFilter;
            _metaDataValuesFilter = inQueryParams.MetaDataValuesFilter;
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

                if (inError != null || !(inResponse is UserListQueryApiCommand.Response userListQueryResponse))
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                _token = userListQueryResponse.token;
                _hasNext = !string.IsNullOrEmpty(_token);
                
                List<SbUser> resultUsers = null;
                if (userListQueryResponse.userDtos != null && 0 < userListQueryResponse.userDtos.Count)
                {
                    resultUsers = new List<SbUser>(userListQueryResponse.userDtos.Count);
                    foreach (UserDto userDto in userListQueryResponse.userDtos)
                    {
                        SbUser user = new SbUser(userDto, _chatMainContextRef);
                        resultUsers.Add(user);
                    }
                }

                inCompletionHandler?.Invoke(resultUsers, null);
            }

            UserListQueryApiCommand.Request request = new UserListQueryApiCommand.Request(_token, _limit, _userIdsFilter, _metaDataKeyFilter,
                                                                                          _metaDataValuesFilter, _nicknameStartsWithFilter, OnResponse);

            _chatMainContextRef.CommandRouter.RequestApiCommand(request);
        }
    }
}