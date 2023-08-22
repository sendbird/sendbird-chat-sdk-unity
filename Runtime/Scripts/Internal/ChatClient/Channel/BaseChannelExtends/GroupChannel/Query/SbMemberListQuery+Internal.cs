// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbMemberListQuery
    {
        private readonly string _nicknameStartsWithFilter;
        private readonly SbGroupChannelOperatorFilter _operatorFilter;
        private readonly SbGroupChannelMutedMemberFilter _mutedMemberFilter;
        private readonly SbMemberStateFilter _memberStateFilter;
        private readonly SbMemberListOrder _order;
        private readonly int _limit;

        private readonly SendbirdChatMainContext _chatMainContextRef;
        private readonly string _channelUrl;
        private bool _hasNext;
        private bool _isLoading = false;
        private string _token;

        internal SbMemberListQuery(string inChannelUrl, SbMemberListQueryParams inQueryParams, SendbirdChatMainContext inChatMainContext)
        {
            _chatMainContextRef = inChatMainContext;
            _channelUrl = inChannelUrl;
            _hasNext = true;
            _isLoading = false;

            if (inQueryParams == null)
                return;

            _nicknameStartsWithFilter = inQueryParams.NicknameStartsWithFilter;
            _operatorFilter = inQueryParams.OperatorFilter;
            _mutedMemberFilter = inQueryParams.MutedMemberFilter;
            _memberStateFilter = inQueryParams.MemberStateFilter;
            _order = inQueryParams.Order;
            _limit = inQueryParams.Limit;
        }

        private void LoadNextPageInternal(SbMemberListHandler inCompletionHandler)
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

                if (inError != null || !(inResponse is GroupChannelMemberListQueryApiCommand.Response queryResponse))
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                _token = queryResponse.token;
                _hasNext = !string.IsNullOrEmpty(_token);
                
                List<SbMember> resultMembers = null;
                if (queryResponse.memberDtos != null && 0 < queryResponse.memberDtos.Count)
                {
                    resultMembers = new List<SbMember>(queryResponse.memberDtos.Count);
                    foreach (MemberDto memberDto in queryResponse.memberDtos)
                    {
                        SbMember sbMember = new SbMember(memberDto, _chatMainContextRef);
                        resultMembers.Add(sbMember);
                    }
                }

                inCompletionHandler?.Invoke(resultMembers, null);
            }

            GroupChannelMemberListQueryApiCommand.Request request = new GroupChannelMemberListQueryApiCommand.Request(
                _token, _limit, _channelUrl, _nicknameStartsWithFilter, _operatorFilter, _mutedMemberFilter, _memberStateFilter, _order, OnResponse);

            _chatMainContextRef.CommandRouter.RequestApiCommand(request);
        }
    }
}