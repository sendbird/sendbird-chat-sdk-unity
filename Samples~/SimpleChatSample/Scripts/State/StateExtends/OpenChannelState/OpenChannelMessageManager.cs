// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;

namespace Sendbird.Chat.Sample
{
    public class OpenChannelMessageManager
    {
        private SbOpenChannel _openChannel = null;
        private readonly List<SbBaseMessage> _messageList = new List<SbBaseMessage>();
        private long _prevMessageTimestamp = long.MaxValue;
        private bool _isLoadingMessages = false;
        private SbOpenChannelHandler _openChannelHandler = null;
        private readonly string _channelHandlerId = Guid.NewGuid().ToString();
        public Action<IReadOnlyList<SbBaseMessage>, bool> OnMessageListChanged { get; set; }
        public Action<SbOpenChannel> OnChannelChanged { get; set; }
        private bool _isOpened = false;

        public void OnOpenState(SbOpenChannel inOpenChannel)
        {
            _isOpened = true;
            _openChannel = inOpenChannel;
            _messageList.Clear();
            _prevMessageTimestamp = long.MaxValue;
            _isLoadingMessages = false;
            _openChannelHandler = new SbOpenChannelHandler
            {
                OnMessageReceived = OnMessagesReceivedHandler,
                OnMessageUpdated = OnMessagesUpdatedHandler,
                OnMessageDeleted = OnMessagesDeletedHandler,
                OnChannelChanged = OnChannelChangedHandler,
                OnUserEntered = OnUserEnteredHandler,
                OnUserExited = OnUserExitedHandler
            };
            SendbirdChat.OpenChannel.AddOpenChannelHandler(_channelHandlerId, _openChannelHandler);
            LoadPrevMessageListIfHasPrevious(inChangedFromLoad: false);
        }

        public void OnCloseState()
        {
            _isOpened = false;
            _messageList.Clear();
            if (_openChannelHandler != null)
            {
                SendbirdChat.OpenChannel.RemoveOpenChannelHandler(_channelHandlerId);
                _openChannelHandler = null;
            }

            OnMessageListChanged = null;
        }

        public bool LoadPrevMessageListIfHasPrevious(bool inChangedFromLoad = true)
        {
            if (_openChannel == null || _isLoadingMessages)
                return false;

            void OnGetMessagesCompletionHandler(IReadOnlyList<SbBaseMessage> inMessages, SbError inError)
            {
                SampleChatMain.Instance.UnblockUI();
                _isLoadingMessages = false;
                if (_isOpened == false)
                    return;

                if (inError != null)
                {
                    SampleChatMain.Instance.OpenNotifyPopup($"ErrorCode:{inError.ErrorCode}\nErrorMessage:{inError.ErrorMessage}");
                }
                else if (inMessages != null && 0 < inMessages.Count)
                {
                    foreach (SbBaseMessage baseMessage in inMessages)
                    {
                        InsertMessage(baseMessage);
                    }

                    _prevMessageTimestamp = inMessages[0].CreatedAt;
                    _messageList.Sort(MessageComparison);

                    OnMessageListChanged?.Invoke(_messageList, inChangedFromLoad);
                }
            }

            SampleChatMain.Instance.BlockUI();
            _isLoadingMessages = true;
            _openChannel.GetMessagesByTimestamp(_prevMessageTimestamp, new SbMessageListParams(), OnGetMessagesCompletionHandler);
            return true;
        }

        private void OnMessagesReceivedHandler(SbBaseChannel inBaseChannel, SbBaseMessage inReceivedMessage)
        {
            if (inBaseChannel == null || inReceivedMessage == null || inBaseChannel.Url != _openChannel.Url)
                return;

            InsertMessage(inReceivedMessage);

            const bool CHANGED_FROM_LOAD = false;
            OnMessageListChanged?.Invoke(_messageList, CHANGED_FROM_LOAD);
        }

        private void OnMessagesUpdatedHandler(SbBaseChannel inBaseChannel, SbBaseMessage inUpdatedMessage)
        {
            if (inBaseChannel == null || inUpdatedMessage == null || inBaseChannel.Url != _openChannel.Url)
                return;

            InsertMessage(inUpdatedMessage);

            const bool CHANGED_FROM_LOAD = false;
            OnMessageListChanged?.Invoke(_messageList, CHANGED_FROM_LOAD);
        }

        private void OnMessagesDeletedHandler(SbBaseChannel inBaseChannel, long inDeletedMessageId)
        {
            if (inBaseChannel == null || inBaseChannel.Url != _openChannel.Url)
                return;

            int foundIndex = _messageList.FindIndex(inMessage => inMessage.MessageId == inDeletedMessageId);
            if (0 <= foundIndex)
            {
                _messageList.RemoveAt(foundIndex);
            }

            const bool CHANGED_FROM_LOAD = false;
            OnMessageListChanged?.Invoke(_messageList, CHANGED_FROM_LOAD);
        }

        private void OnChannelChangedHandler(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null || inBaseChannel.Url != _openChannel.Url)
                return;

            _openChannel = inBaseChannel as SbOpenChannel;
            OnChannelChanged?.Invoke(_openChannel);
        }

        private void OnUserEnteredHandler(SbOpenChannel inOpenChannel, SbUser inUser)
        {
            if (inOpenChannel == null || inOpenChannel.Url != _openChannel.Url)
                return;

            _openChannel = inOpenChannel;
            OnChannelChanged?.Invoke(_openChannel);
        }

        private void OnUserExitedHandler(SbOpenChannel inOpenChannel, SbUser inUser)
        {
            if (inOpenChannel == null || inOpenChannel.Url != _openChannel.Url)
                return;

            _openChannel = inOpenChannel;
            OnChannelChanged?.Invoke(_openChannel);
        }

        private void InsertMessage(SbBaseMessage inReceivedOrUpdatedMessage)
        {
            int foundIndex = _messageList.FindIndex(inMessage => inMessage.MessageId == inReceivedOrUpdatedMessage.MessageId);
            if (0 <= foundIndex)
            {
                _messageList[foundIndex] = inReceivedOrUpdatedMessage;
            }
            else
            {
                _messageList.Add(inReceivedOrUpdatedMessage);
            }
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

            return compare;
        }

        public void SendMessage(string inMessage, Action inCompletedHandler)
        {
            if (_openChannel == null || string.IsNullOrEmpty(inMessage))
                return;

            void OnSendMessageCompleteHandler(SbBaseMessage inResultMessage, SbError inError)
            {
                SampleChatMain.Instance.UnblockUI();
                if (inError != null)
                {
                    SampleChatMain.Instance.OpenNotifyPopup($"ErrorCode:{inError.ErrorCode}\nErrorMessage:{inError.ErrorMessage}");
                }

                InsertMessage(inResultMessage);
                inCompletedHandler?.Invoke();

                const bool CHANGED_FROM_LOAD = false;
                OnMessageListChanged?.Invoke(_messageList, CHANGED_FROM_LOAD);
            }

            SampleChatMain.Instance.BlockUI();
            _openChannel.SendUserMessage(inMessage, OnSendMessageCompleteHandler);
        }
    }
}