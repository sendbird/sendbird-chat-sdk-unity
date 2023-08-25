// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbMessageSearchQuery
    {
        private readonly SbMessageSearchQueryParams _queryParams = null;
        private readonly SendbirdChatMainContext _chatMainContextRef;
        private bool _hasNext;
        private bool _isLoading = false;
        private string _afterToken;
        private int _totalCount = -1;

        internal SbMessageSearchQuery(SbMessageSearchQueryParams inQueryParams, SendbirdChatMainContext inChatMainContext)
        {
            _chatMainContextRef = inChatMainContext;
            _hasNext = true;
            _isLoading = false;

            if (inQueryParams == null)
            {
                _queryParams = new SbMessageSearchQueryParams();
            }
            else
            {
                _queryParams = inQueryParams.Clone();
            }
        }

        private void LoadNextPageInternal(SbMessageListHandler inCompletionHandler)
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

                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                if (!(inResponse is MessageSearchQueryApiCommand.Response queryResponse))
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                    return;
                }

                _hasNext = queryResponse.hasNext;
                _afterToken = queryResponse.endToken;
                _totalCount = queryResponse.totalCount;

                List<SbBaseMessage> resultMessages = null;
                if (queryResponse.BaseMessageDtos != null && 0 < queryResponse.BaseMessageDtos.Count)
                {
                    resultMessages = new List<SbBaseMessage>(queryResponse.BaseMessageDtos.Count);
                    foreach (BaseMessageDto messageDto in queryResponse.BaseMessageDtos)
                    {
                        SbBaseMessage message = messageDto.CreateMessageInstance(_chatMainContextRef);
                        resultMessages.Add(message);
                    }
                }

                inCompletionHandler?.Invoke(resultMessages, null);
            }


            MessageSearchQueryApiCommand.Request request = new MessageSearchQueryApiCommand.Request(inToken:null, inBeforeToken:null, _afterToken, _queryParams, OnResponse);
            _chatMainContextRef.CommandRouter.RequestApiCommand(request);
        }
    }
}