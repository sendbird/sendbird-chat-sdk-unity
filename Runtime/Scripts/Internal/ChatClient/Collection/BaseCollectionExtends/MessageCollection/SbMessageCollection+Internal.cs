// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sendbird.Chat
{
    public partial class SbMessageCollection
    {
        private enum UpdateActionType
        {
            Add,
            Update,
            Delete,
            None,
        }

        private enum StateType
        {
            Created,
            InitializeStarted,
            Initialized,
            Disposed
        }

        private const int DEFAULT_QUERY_RESULT_SIZE = 40;
        private readonly SbGroupChannel _groupChannel;
        private readonly SbMessageListParams _messageListParams = null;
        private readonly int _messageListPreviousResultSize = DEFAULT_QUERY_RESULT_SIZE;
        private readonly int _messageListNextResultSize = DEFAULT_QUERY_RESULT_SIZE;
        private readonly SbMessageChangeLogsParams _messageChangeLogsParams = null;
        private SbMessageCollectionHandler _messageCollectionHandler = null;
        private readonly List<SbBaseMessage> _succeededMessages = new List<SbBaseMessage>();
        private readonly List<SbBaseMessage> _failedMessages = new List<SbBaseMessage>();
        private readonly List<SbBaseMessage> _pendingMessages = new List<SbBaseMessage>();
        private readonly long _startingPoint = long.MaxValue;
        private long _messageOffsetTimestamp;
        private long _oldestSyncedTimestamp;
        private long _latestSyncedTimestamp;
        private string _lastSyncChangeLogsToken;
        private bool _hasNext;
        private bool _hasPrevious;
        private bool _isLoading = false;
        private StateType _stateType;
        private CoroutineJob _autoUnmuteJob = null;

        internal SbMessageCollection(SbGroupChannel inGroupChannel, SbMessageCollectionCreateParams inCreateParams, SendbirdChatMainContext inChatMainContext) : base(inChatMainContext)
        {
            if (inGroupChannel == null || inCreateParams == null)
            {
                Logger.Error(Logger.CategoryType.Collection, "SbMessageCollection() groupChannel or createParams is null.");
                return;
            }

            _groupChannel = inGroupChannel;
            _stateType = StateType.Created;

            _messageCollectionHandler = inCreateParams.MessageCollectionHandler;
            _messageListParams = inCreateParams.MessageListParams?.Clone();
            if (_messageListParams == null)
                _messageListParams = new SbMessageListParams();

            if (0 < _messageListParams.PreviousResultSize)
                _messageListPreviousResultSize = _messageListParams.PreviousResultSize;

            if (0 < _messageListParams.NextResultSize)
                _messageListNextResultSize = _messageListParams.NextResultSize;

            _startingPoint = inCreateParams.StartingPoint;

            _messageOffsetTimestamp = _groupChannel.MessageOffsetTimestamp;
            _hasPrevious = true;
            _hasNext = _startingPoint < long.MaxValue;

            _messageChangeLogsParams = new SbMessageChangeLogsParams
            {
                IncludeReactions = _messageListParams.IncludeReactions,
                IncludeMetaArray = _messageListParams.IncludeMetaArray,
                IncludeThreadInfo = _messageListParams.IncludeThreadInfo,
                IncludeParentMessageInfo = _messageListParams.IncludeParentMessageInfo,
                ReplyType = _messageListParams.ReplyType
            };
        }

        private void InitializeInternal(SbMessageCollectionInitHandler inMessageCollectionInitHandler)
        {
            if (_stateType != StateType.Created)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => { inMessageCollectionInitHandler?.OnApiResult?.Invoke(null, SbErrorCodeExtension.INVALID_INITIALIZATION_ERROR); });
                return;
            }

            _stateType = StateType.InitializeStarted;
            _hasPrevious = true;
            _hasNext = _startingPoint < long.MaxValue;
            _oldestSyncedTimestamp = _startingPoint <= 0 ? long.MaxValue : _startingPoint;
            _latestSyncedTimestamp = _startingPoint == long.MaxValue ? 0 : _startingPoint;
            _isLoading = false;

            _succeededMessages.Clear();
            _pendingMessages.Clear();
            _failedMessages.Clear();

            void OnChannelRefreshHandler(SbError inError)
            {
                if (inError == null)
                {
                    OnChannelUpdated(SbCollectionEventSource.ChannelRefresh, _groupChannel);
                }
            }

            void OnGetMessageHandler(IReadOnlyList<SbBaseMessage> inResponseMessages, SbError inError)
            {
                _stateType = StateType.Initialized;
                _groupChannel.Refresh(OnChannelRefreshHandler);

                if (inError != null)
                {
                    inMessageCollectionInitHandler?.OnApiResult?.Invoke(null, inError);
                    return;
                }

                if (inResponseMessages == null || inResponseMessages.Count <= 0)
                {
                    _hasNext = false;
                    _hasPrevious = false;
                }
                else
                {
                    int prevCount = inResponseMessages.Count(inMessage => inMessage.CreatedAt < _startingPoint);
                    _hasPrevious = _messageListParams.PreviousResultSize <= prevCount;

                    int nextCount = inResponseMessages.Count(inMessage => _startingPoint < inMessage.CreatedAt);
                    _hasNext = _messageListParams.NextResultSize <= nextCount;

                    _oldestSyncedTimestamp = GetOldestCreateAt(inResponseMessages, _oldestSyncedTimestamp);
                    _latestSyncedTimestamp = GetLatestCreateAt(inResponseMessages, _latestSyncedTimestamp);
                    InsertMessagesToSucceededMessageList(inResponseMessages);
                }

                inMessageCollectionInitHandler?.OnApiResult?.Invoke(inResponseMessages, null);
            }

            if (_messageListParams != null)
            {
                _messageListParams.PreviousResultSize = _messageListPreviousResultSize;
                _messageListParams.NextResultSize = _messageListNextResultSize;
                _messageListParams.IsInclusive = true;
            }

            _groupChannel.GetMessagesByTimestampInternal(_startingPoint, _messageListParams, OnGetMessageHandler);
        }

        private protected override void DisposeInternal()
        {
            StopAutoUnmuteCoroutineIfStarted();
            _succeededMessages.Clear();
            _pendingMessages.Clear();
            _failedMessages.Clear();
            _hasPrevious = false;
            _hasNext = false;
            _lastSyncChangeLogsToken = null;
            _stateType = StateType.Disposed;
            base.DisposeInternal();
        }

        private void LoadNextInternal(SbMessageListHandler inCompletionHandler)
        {
            SbError error = null;
            if (_stateType == StateType.Disposed) { error = SbErrorCodeExtension.COLLECTION_DISPOSED; }
            else if (_stateType != StateType.Initialized) { error = SbErrorCodeExtension.INVALID_INITIALIZATION_ERROR; }
            else if (_isLoading) { error = SbErrorCodeExtension.QUERY_IN_PROGRESS; }
            else if (_hasNext == false) { error = SbErrorCodeExtension.LOCAL_DATABASE_ERROR; }

            if (error != null)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler?.Invoke(null, error); });
                return;
            }

            void OnGetMessageHandler(IReadOnlyList<SbBaseMessage> inResponseMessages, SbError inError)
            {
                _isLoading = false;
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                if (inResponseMessages == null || inResponseMessages.Count <= 0)
                {
                    _hasNext = false;
                }
                else
                {
                    int nextCount = inResponseMessages.Count(inMessage => _latestSyncedTimestamp < inMessage.CreatedAt);
                    _hasNext = _messageListParams.NextResultSize <= nextCount;

                    InsertMessagesToSucceededMessageList(inResponseMessages);
                    _latestSyncedTimestamp = GetLatestCreateAt(_succeededMessages, _latestSyncedTimestamp);
                }

                inCompletionHandler?.Invoke(inResponseMessages, null);
            }

            if (_messageListParams != null)
            {
                _messageListParams.PreviousResultSize = 0;
                _messageListParams.NextResultSize = _messageListNextResultSize;
                _messageListParams.IsInclusive = true;
            }

            _isLoading = true;
            _groupChannel.GetMessagesByTimestampInternal(_latestSyncedTimestamp, _messageListParams, OnGetMessageHandler);
        }

        private void LoadPreviousInternal(SbMessageListHandler inCompletionHandler)
        {
            SbError error = null;
            if (_stateType == StateType.Disposed) { error = SbErrorCodeExtension.COLLECTION_DISPOSED; }
            else if (_stateType != StateType.Initialized) { error = SbErrorCodeExtension.INVALID_INITIALIZATION_ERROR; }
            else if (_isLoading) { error = SbErrorCodeExtension.QUERY_IN_PROGRESS; }
            else if (_hasPrevious == false) { error = SbErrorCodeExtension.LOCAL_DATABASE_ERROR; }

            if (error != null)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler?.Invoke(null, error); });
                return;
            }

            void OnGetMessageHandler(IReadOnlyList<SbBaseMessage> inResponseMessages, SbError inError)
            {
                _isLoading = false;
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                if (inResponseMessages == null || inResponseMessages.Count <= 0)
                {
                    _hasPrevious = false;
                }
                else
                {
                    int previousCount = inResponseMessages.Count(inMessage => inMessage.CreatedAt < _oldestSyncedTimestamp);
                    _hasPrevious = _messageListParams.PreviousResultSize <= previousCount;

                    InsertMessagesToSucceededMessageList(inResponseMessages);
                    _oldestSyncedTimestamp = GetOldestCreateAt(_succeededMessages, _oldestSyncedTimestamp);
                }

                inCompletionHandler?.Invoke(inResponseMessages, null);
            }

            if (_messageListParams != null)
            {
                _messageListParams.PreviousResultSize = _messageListPreviousResultSize;
                _messageListParams.NextResultSize = 0;
                _messageListParams.IsInclusive = true;
            }

            _isLoading = true;
            _groupChannel.GetMessagesByTimestampInternal(_oldestSyncedTimestamp, _messageListParams, OnGetMessageHandler);
        }

        private void RemoveAllFailedMessagesInternal(SbErrorHandler inCompletionHandler)
        {
            _failedMessages.Clear();
            CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler?.Invoke(null); });
        }

        private void RemoveFailedMessagesInternal(List<SbBaseMessage> inFailedMessages, SbRemoveFailedMessagesHandler inCompletionHandler)
        {
            if (inFailedMessages == null || inFailedMessages.Count <= 0)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler?.Invoke(ListExtension.EMPTY_STRING_LIST, null); });
                return;
            }

            List<string> removedFailedMessageRequestIds = new List<string>(inFailedMessages.Count);
            foreach (SbBaseMessage failedMessage in inFailedMessages)
            {
                if (failedMessage != null && RemoveFromFailedMessagesList(failedMessage.RequestId))
                {
                    removedFailedMessageRequestIds.Add(failedMessage.RequestId);
                }
            }

            CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler?.Invoke(removedFailedMessageRequestIds, null); });
        }

        private long GetOldestCreateAt(IReadOnlyCollection<SbBaseMessage> inMessages, long inDefault)
        {
            if (inMessages == null || inMessages.Count <= 0)
                return inDefault;

            long oldestCreateAt = long.MaxValue;
            foreach (SbBaseMessage baseMessage in inMessages)
            {
                oldestCreateAt = Math.Min(oldestCreateAt, baseMessage.CreatedAt);
            }

            return oldestCreateAt;
        }

        private long GetLatestCreateAt(IReadOnlyCollection<SbBaseMessage> inMessages, long inDefault)
        {
            if (inMessages == null || inMessages.Count <= 0)
                return inDefault;

            long latestCreateAt = long.MinValue;
            foreach (SbBaseMessage baseMessage in inMessages)
            {
                latestCreateAt = Math.Max(latestCreateAt, baseMessage.CreatedAt);
            }

            return latestCreateAt;
        }

        private void RequestAllChangeLogs()
        {
            List<SbBaseMessage> updatedMessages = null;
            List<long> deletedMessageIds = null;
            string lastSucceededResultToken = _lastSyncChangeLogsToken;
            void OnCompleteChangeLogs(IReadOnlyList<SbBaseMessage> inUpdatedMessages, IReadOnlyList<long> inDeletedMessageIds, bool inHasMore, string inToken, SbError inError)
            {
                if (inError == null)
                {
                    if (inUpdatedMessages != null && 0 < inUpdatedMessages.Count)
                    {
                        if (updatedMessages == null) { updatedMessages = new List<SbBaseMessage>(); }

                        updatedMessages.AddRange(inUpdatedMessages);
                    }

                    if (inDeletedMessageIds != null && 0 < inDeletedMessageIds.Count)
                    {
                        if (deletedMessageIds == null) { deletedMessageIds = new List<long>(); }

                        deletedMessageIds.AddRange(inDeletedMessageIds);
                    }

                    lastSucceededResultToken = inToken;
                    if (inHasMore)
                    {
                        _groupChannel.GetMessageChangeLogsSinceToken(lastSucceededResultToken, _messageChangeLogsParams, OnCompleteChangeLogs);
                        return;
                    }
                }

                if (updatedMessages != null || deletedMessageIds != null)
                {
                    UpsertToSucceededMessagesList(SbCollectionEventSource.MessageChangelog.ToCachedMessageContext(), updatedMessages, deletedMessageIds);
                }

                _lastSyncChangeLogsToken = lastSucceededResultToken;
            }

            if (string.IsNullOrEmpty(_lastSyncChangeLogsToken) == false)
            {
                _groupChannel.GetMessageChangeLogsSinceToken(_lastSyncChangeLogsToken, _messageChangeLogsParams, OnCompleteChangeLogs);
            }
            else
            {
                long changeLogsTimestamp = _groupChannel.LastSyncedChangeLogsTimestamp;
                if (changeLogsTimestamp <= 0)
                {
                    changeLogsTimestamp = _oldestSyncedTimestamp;
                }

                _groupChannel.GetMessageChangeLogsSinceTimestamp(changeLogsTimestamp, _messageChangeLogsParams, OnCompleteChangeLogs);
            }
        }

        private void CheckHugeGapAndFill()
        {
            if (_stateType != StateType.Initialized || _succeededMessages.Count <= 0)
                return;

            long cachedOldestTs = GetOldestCreateAt(_succeededMessages, 0);
            long cachedLatestTs = GetLatestCreateAt(_succeededMessages, long.MaxValue);
            if (_hasNext == false)
                cachedLatestTs = long.MaxValue;

            if (_hasNext && _oldestSyncedTimestamp <= cachedOldestTs && cachedLatestTs <= _latestSyncedTimestamp)
                return;

            GetGroupChannelHugeGapApiCommand.Request.Params requestParams;
            {
                requestParams.channelUrl = _groupChannel.Url;
                requestParams.reverse = _messageListParams.Reverse;
                requestParams.customTypes = _messageListParams.CustomTypes;
                requestParams.senderUserIds = _messageListParams.SenderUserIds;
                requestParams.messageTypeFilter = _messageListParams.MessageTypeFilter;
                //if use local cache, checking_continuous_messages is true
                requestParams.checkContinuousMessages = false;
                requestParams.prevStartTs = cachedOldestTs;
                requestParams.prevEndTs = _oldestSyncedTimestamp;
                requestParams.prevCacheCount = _succeededMessages.Count(inMessage => inMessage.CreatedAt <= _oldestSyncedTimestamp);
                requestParams.nextStartTs = _latestSyncedTimestamp;
                requestParams.nextEndTs = cachedLatestTs;
                requestParams.nextCacheCount = _succeededMessages.Count(inMessage => _latestSyncedTimestamp <= inMessage.CreatedAt);
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null || !(inResponse is GetGroupChannelHugeGapApiCommand.Response hugeGapResponse))
                    return;

                if (hugeGapResponse.isHugeGap)
                {
                    InvokeOnHugeGapDetectedHandler();
                    return;
                }

                List<SbBaseMessage> totalMessages = new List<SbBaseMessage>();
                if (hugeGapResponse.PrevMessageDtos != null && 0 < hugeGapResponse.PrevMessageDtos.Count)
                {
                    List<SbBaseMessage> prevMessages = hugeGapResponse.PrevMessageDtos.Select(inMessageDto => inMessageDto.CreateMessageInstance(chatMainContextRef)).ToList();
                    totalMessages.AddRange(prevMessages);
                    long oldestTs = GetOldestCreateAt(prevMessages, _oldestSyncedTimestamp);
                    if (oldestTs < _oldestSyncedTimestamp)
                        _oldestSyncedTimestamp = oldestTs;

                    if (requestParams.prevCacheCount <= hugeGapResponse.PrevMessageDtos.Count)
                        _hasPrevious = true;
                }

                if (hugeGapResponse.NextMessageDtos != null && 0 < hugeGapResponse.NextMessageDtos.Count)
                {
                    List<SbBaseMessage> nextMessages = hugeGapResponse.NextMessageDtos.Select(inMessageDto => inMessageDto.CreateMessageInstance(chatMainContextRef)).ToList();
                    totalMessages.AddRange(nextMessages);
                    long oldestTs = GetLatestCreateAt(nextMessages, _latestSyncedTimestamp);
                    if (_latestSyncedTimestamp < oldestTs)
                        _latestSyncedTimestamp = oldestTs;
                }

                if (0 < totalMessages.Count)
                {
                    UpsertToSucceededMessagesList(SbMessageContext.MESSAGE_FILL, totalMessages);
                }
            }

            GetGroupChannelHugeGapApiCommand.Request apiCommand = new GetGroupChannelHugeGapApiCommand.Request(requestParams, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}