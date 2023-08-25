// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Linq;

namespace Sendbird.Chat
{
    public partial class SbMessageCollection
    {
        private void UpsertToSucceededMessagesList(SbMessageContext inMessageContext, IReadOnlyCollection<SbBaseMessage> inAddOrUpdateMessages,
                                                   IReadOnlyCollection<long> inDeletedMessageIds = null)
        {
            List<SbBaseMessage> addedResultMessages = null;
            List<SbBaseMessage> updatedResultMessages = null;
            List<SbBaseMessage> deletedResultMessages = null;
            bool isChangedSucceededMessageList = false;

            if (inAddOrUpdateMessages != null && 0 < inAddOrUpdateMessages.Count)
            {
                foreach (SbBaseMessage message in inAddOrUpdateMessages)
                {
                    if (message == null)
                        continue;

                    UpdateActionType updateAction = CalculateUpdateAction(message);
                    switch (updateAction)
                    {
                        case UpdateActionType.Add:
                        {
                            if (addedResultMessages == null) { addedResultMessages = new List<SbBaseMessage>(); }

                            addedResultMessages.Add(message);
                            InsertMessageToSucceededMessageList(message);
                            isChangedSucceededMessageList = true;
                            break;
                        }
                        case UpdateActionType.Update:
                        {
                            if (updatedResultMessages == null) { updatedResultMessages = new List<SbBaseMessage>(); }

                            updatedResultMessages.Add(message);
                            InsertMessageToSucceededMessageList(message);
                            isChangedSucceededMessageList = true;
                            break;
                        }
                        case UpdateActionType.Delete:
                        {
                            if (deletedResultMessages == null) { deletedResultMessages = new List<SbBaseMessage>(); }

                            deletedResultMessages.Add(message);
                            isChangedSucceededMessageList = true;
                            break;
                        }
                        case UpdateActionType.None: break;
                    }
                }
            }

            if (inDeletedMessageIds != null && 0 < inDeletedMessageIds.Count)
            {
                if (deletedResultMessages == null) { deletedResultMessages = new List<SbBaseMessage>(); }

                foreach (long deletedMessageId in inDeletedMessageIds)
                {
                    SbBaseMessage foundDeleteMessage = _succeededMessages.Find(inCachedMessage => inCachedMessage.MessageId == deletedMessageId);
                    if (foundDeleteMessage != null)
                    {
                        deletedResultMessages.Add(foundDeleteMessage);
                        isChangedSucceededMessageList = true;
                    }
                }
            }

            if (isChangedSucceededMessageList == false)
                return;

            if (deletedResultMessages != null)
            {
                foreach (SbBaseMessage deletedMessage in deletedResultMessages)
                {
                    int foundIndex = _succeededMessages.FindIndex(inCachedMessage => inCachedMessage.MessageId == deletedMessage.MessageId);
                    if (0 <= foundIndex)
                    {
                        _succeededMessages.RemoveAt(foundIndex);
                    }
                }
            }

            _succeededMessages.Sort(MessageComparison);

            if (inMessageContext != null)
            {
                if (addedResultMessages != null && 0 < addedResultMessages.Count)
                {
                    InvokeOnMessagesAddedHandler(inMessageContext, addedResultMessages);
                }

                if (updatedResultMessages != null && 0 < updatedResultMessages.Count)
                {
                    InvokeOnMessagesUpdatedHandler(inMessageContext, updatedResultMessages);
                }

                if (deletedResultMessages != null && 0 < deletedResultMessages.Count)
                {
                    InvokeOnMessagesDeletedHandler(inMessageContext, deletedResultMessages);
                }
            }
        }

        private UpdateActionType CalculateUpdateAction(SbBaseMessage inMessage)
        {
            bool shouldAddToView = ShouldAddToView(inMessage);
            bool contains = _succeededMessages.Any(inCachedMessage => inCachedMessage.MessageId == inMessage.MessageId);

            if (shouldAddToView) { return contains ? UpdateActionType.Update : UpdateActionType.Add; }
            else { return contains ? UpdateActionType.Delete : UpdateActionType.None; }
        }

        private bool ShouldAddToView(SbBaseMessage inMessage)
        {
            if (inMessage == null || _messageListParams.BelongsTo(inMessage) == false)
                return false;

            long messageCreateAt = inMessage.CreatedAt;
            long oldestMessageCreateAt = GetOldestCreateAt(_succeededMessages, _oldestSyncedTimestamp);
            long latestMessageCreateAt = GetLatestCreateAt(_succeededMessages, _latestSyncedTimestamp);

            if (oldestMessageCreateAt <= messageCreateAt && messageCreateAt <= latestMessageCreateAt)
                return true;

            if (messageCreateAt <= oldestMessageCreateAt && _hasPrevious == false)
                return true;

            if (latestMessageCreateAt <= messageCreateAt && _hasNext == false)
                return true;

            return false;
        }

        private int MessageComparison(SbBaseMessage inMessageA, SbBaseMessage inMessageB)
        {
            if (inMessageA == null && inMessageB == null)
                return 0;

            int compare = 0;
            if (inMessageA == null)
            {
                compare = 1;
            }
            else if (inMessageB == null)
            {
                compare = -1;
            }
            else
            {
                compare = inMessageA.CreatedAt.CompareTo(inMessageB.CreatedAt);
            }

            return _messageListParams.Reverse == false ? compare : -compare;
        }

        private void InsertMessageToSucceededMessageList(SbBaseMessage inMessage)
        {
            if (inMessage == null)
                return;

            int foundIndex = _succeededMessages.FindIndex(inCachedMessage => inCachedMessage.MessageId == inMessage.MessageId);
            if (0 <= foundIndex)
            {
                _succeededMessages[foundIndex] = inMessage;
            }
            else
            {
                _succeededMessages.Add(inMessage);
            }
        }

        private void InsertMessagesToSucceededMessageList(IReadOnlyList<SbBaseMessage> inMessages)
        {
            if (inMessages == null || inMessages.Count <= 0)
                return;

            foreach (SbBaseMessage baseMessage in inMessages)
            {
                InsertMessageToSucceededMessageList(baseMessage);
            }

            _succeededMessages.Sort(MessageComparison);
        }

        private void RemoveFromPendingMessagesList(string inRequestId)
        {
            if (string.IsNullOrEmpty(inRequestId) == false)
            {
                int foundIndex = _pendingMessages.FindIndex(inPendingMessage => inPendingMessage.RequestId == inRequestId);
                if (0 <= foundIndex)
                {
                    _pendingMessages.RemoveAt(foundIndex);
                }
            }
        }

        private bool RemoveFromFailedMessagesList(string inRequestId)
        {
            if (string.IsNullOrEmpty(inRequestId) == false)
            {
                int foundIndex = _failedMessages.FindIndex(inFailedMessage => inFailedMessage.RequestId == inRequestId);
                if (0 <= foundIndex)
                {
                    _failedMessages.RemoveAt(foundIndex);
                    return true;
                }
            }

            return false;
        }
    }
}