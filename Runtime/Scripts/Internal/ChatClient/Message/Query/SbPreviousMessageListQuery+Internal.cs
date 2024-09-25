// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbPreviousMessageListQuery
    {
        private readonly string _channelUrl;
        private readonly SbChannelType _channelType;
        private readonly bool _reverse = false;
        private readonly SbMessageTypeFilter _messageTypeFilter = SbMessageTypeFilter.All;
        private readonly List<string> _customTypesFilter = new List<string>();
        private readonly List<string> _senderUserIdsFilter = new List<string>();
        private readonly SbReplyType _replyType = SbReplyType.None;
        private readonly bool _includeMetaArray = false;
        private readonly bool _includeReactions = false;
        private readonly bool _includeThreadInfo = false;
        private readonly bool _includeParentMessageInfo = false;
        private readonly bool _showSubChannelMessagesOnly = false;
        private readonly int _limit;
        private readonly SendbirdChatMainContext _chatMainContextRef;
        private bool _hasNext;
        private bool _isLoading = false;
        private long _minimumTimestamp = long.MaxValue;

        internal SbPreviousMessageListQuery(SbChannelType inChannelType, string inChannelUrl, SbPreviousMessageListQueryParams inQueryParams, SendbirdChatMainContext inChatMainContext)
        {
            _chatMainContextRef = inChatMainContext;
            _channelType = inChannelType;
            _channelUrl = inChannelUrl;
            _hasNext = true;
            _isLoading = false;

            if (inQueryParams == null)
                return;

            _reverse = inQueryParams.Reverse;
            _messageTypeFilter = inQueryParams.MessageTypeFilter;
            _replyType = inQueryParams.ReplyType;

            _showSubChannelMessagesOnly = inQueryParams.ShowSubChannelMessagesOnly;
            _limit = inQueryParams.Limit;

            _includeReactions = inQueryParams.IncludeReactions;
            _includeMetaArray = inQueryParams.IncludeMetaArray;
            _includeThreadInfo = inQueryParams.IncludeThreadInfo;
            _includeParentMessageInfo = inQueryParams.IncludeParentMessageInfo;

            if (inQueryParams.CustomTypesFilter != null && 0 < inQueryParams.CustomTypesFilter.Count)
                _customTypesFilter.AddRange(inQueryParams.CustomTypesFilter);

            if (inQueryParams.SenderUserIdsFilter != null && 0 < inQueryParams.SenderUserIdsFilter.Count)
                _senderUserIdsFilter.AddRange(inQueryParams.SenderUserIdsFilter);
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

                if (!(inResponse is MessageListQueryApiCommand.Response queryResponse))
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                    return;
                }

                List<SbBaseMessage> resultMessages = null;
                if (queryResponse.BaseMessageDtos != null && 0 < queryResponse.BaseMessageDtos.Count)
                {
                    long prevTimestamp = _minimumTimestamp;
                    resultMessages = new List<SbBaseMessage>(queryResponse.BaseMessageDtos.Count);
                    foreach (BaseMessageDto messageDto in queryResponse.BaseMessageDtos)
                    {
                        _minimumTimestamp = Math.Min(messageDto.CreatedAt, _minimumTimestamp);
                        SbBaseMessage message = messageDto.CreateMessageInstance(_chatMainContextRef);
                        resultMessages.Add(message);
                    }

                    _hasNext = _minimumTimestamp != prevTimestamp;
                    if (queryResponse.BaseMessageDtos.Count < _limit)
                        _hasNext = false;
                }
                else
                {
                    _hasNext = false;
                }

                inCompletionHandler?.Invoke(resultMessages, null);
            }

            MessageListQueryApiCommand.Request.Params requestParams = new MessageListQueryApiCommand.Request.Params()
            {
                messageTimestamp = _minimumTimestamp,
                messageId = SbBaseMessage.INVALID_MESSAGE_ID_MIN,
                prevLimit = _limit,
                nextLimit = 0,
                inclusive = false,
                reverse = _reverse,
                messageTypeFilter = _messageTypeFilter,
                customTypes = _customTypesFilter,
                senderUserIds = _senderUserIdsFilter,
                parentMessageId = SbBaseMessage.INVALID_MESSAGE_ID_MIN,
                showSubChannelMessagesOnly = _showSubChannelMessagesOnly,
                includeMetaArray = _includeMetaArray,
                includeReactions = _includeReactions,
                includeThreadInfo = _includeThreadInfo,
                includeParentMessageInfo = _includeParentMessageInfo,
                replyType = _replyType
            };

            MessageListQueryApiCommand.Request request = new MessageListQueryApiCommand.Request(_channelType, _channelUrl, requestParams, OnResponse);
            _chatMainContextRef.CommandRouter.RequestApiCommand(request);
        }
    }
}