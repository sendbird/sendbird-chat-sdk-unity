// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbParticipantListQuery
    {
        private readonly int _limit;

        private readonly SendbirdChatMainContext _chatMainContextRef;
        private readonly string _channelUrl;
        private bool _hasNext;
        private bool _isLoading = false;
        private string _token;

        internal SbParticipantListQuery(string inChannelUrl, SbParticipantListQueryParams inQueryParams, SendbirdChatMainContext inChatMainContext)
        {
            _chatMainContextRef = inChatMainContext;
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

                if (inError != null || !(inResponse is OpenChannelParticipantListQueryApiCommand.Response queryResponse))
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                _token = queryResponse.token;
                _hasNext = !string.IsNullOrEmpty(_token);
                
                List<SbUser> resultUsers = null;
                if (queryResponse.participantDtos != null && 0 < queryResponse.participantDtos.Count)
                {
                    resultUsers = new List<SbUser>(queryResponse.participantDtos.Count);
                    foreach (ParticipantDto participantDto in queryResponse.participantDtos)
                    {
                        SbParticipant participant = new SbParticipant(participantDto, _chatMainContextRef);
                        resultUsers.Add(participant);
                    }
                }

                inCompletionHandler?.Invoke(resultUsers, null);
            }

            OpenChannelParticipantListQueryApiCommand.Request request = new OpenChannelParticipantListQueryApiCommand.Request(_token, _limit, _channelUrl, OnResponse);

            _chatMainContextRef.CommandRouter.RequestApiCommand(request);
        }
    }
}