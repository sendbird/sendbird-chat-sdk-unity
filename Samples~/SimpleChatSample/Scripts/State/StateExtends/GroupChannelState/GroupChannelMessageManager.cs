// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;

namespace Sendbird.Chat.Sample
{
    public class GroupChannelMessageManager
    {
        private SbMessageCollection _messageCollection = null;
        private SbGroupChannel _groupChannel = null;
        private bool _isLoadingMessages = false;
        private bool _isOpened = false;
        public Action<IReadOnlyList<SbBaseMessage>, bool> OnMessageListChanged { get; set; }

        public void OnOpenState(SbGroupChannel inGroupChannel)
        {
            _isOpened = true;
            _groupChannel = inGroupChannel;

            SbMessageListParams messageListParams = new SbMessageListParams();
            SbMessageCollectionHandler messageCollectionHandler = new SbMessageCollectionHandler
            {
                OnMessagesAdded = OnMessagesAddedHandler,
                OnMessagesUpdated = OnMessagesUpdatedHandler,
                OnMessagesDeleted = OnMessagesDeletedHandler
            };
            SbMessageCollectionCreateParams createParams = new SbMessageCollectionCreateParams(messageListParams, long.MaxValue, messageCollectionHandler);
            _messageCollection = _groupChannel.CreateMessageCollection(createParams);

            SbMessageCollectionInitHandler initHandler = new SbMessageCollectionInitHandler
            {
                OnApiResult = OnInitializeMessageCollectionCompletionHandler
            };
            SampleChatMain.Instance.BlockUI();
            _messageCollection.Initialize(initHandler);
        }

        public void OnCloseState()
        {
            _isOpened = false;
            if (_messageCollection != null)
            {
                _messageCollection.Dispose();
                _messageCollection = null;
            }

            OnMessageListChanged = null;
        }

        private void OnInitializeMessageCollectionCompletionHandler(IReadOnlyList<SbBaseMessage> inMessages, SbError inError)
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
                const bool CHANGED_FROM_LOAD = false;
                OnMessageListChanged?.Invoke(_messageCollection.SucceededMessages, CHANGED_FROM_LOAD);
            }
        }

        public bool LoadPrevMessageListIfHasPrevious()
        {
            if (_messageCollection == null || _isLoadingMessages)
                return false;

            void OnLoadCompletionHandler(IReadOnlyList<SbBaseMessage> inMessages, SbError inError)
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
                    const bool CHANGED_FROM_LOAD = true;
                    OnMessageListChanged?.Invoke(_messageCollection.SucceededMessages, CHANGED_FROM_LOAD);
                }
            }

            if (_messageCollection.HasPrevious)
            {
                SampleChatMain.Instance.BlockUI();
                _isLoadingMessages = true;
                _messageCollection.LoadPrevious(OnLoadCompletionHandler);
            }

            return true;
        }

        private void OnMessagesAddedHandler(SbMessageContext inContext, IReadOnlyList<SbBaseMessage> inAddedMessages)
        {
            const bool CHANGED_FROM_LOAD = false;
            OnMessageListChanged?.Invoke(_messageCollection.SucceededMessages, CHANGED_FROM_LOAD);
 			_groupChannel?.MarkAsRead(inCompletionHandler: null);
        }

        private void OnMessagesUpdatedHandler(SbMessageContext inContext, IReadOnlyList<SbBaseMessage> inUpdatedMessages)
        {
            const bool CHANGED_FROM_LOAD = false;
            OnMessageListChanged?.Invoke(_messageCollection.SucceededMessages, CHANGED_FROM_LOAD);
        }

        private void OnMessagesDeletedHandler(SbMessageContext inContext, IReadOnlyList<SbBaseMessage> inDeletedMessages)
        {
            const bool CHANGED_FROM_LOAD = false;
            OnMessageListChanged?.Invoke(_messageCollection.SucceededMessages, CHANGED_FROM_LOAD);
        }

        public void SendMessage(string inMessage, Action inCompletedHandler)
        {
            if (_groupChannel == null || string.IsNullOrEmpty(inMessage))
                return;

            void OnSendMessageCompleteHandler(SbBaseMessage inResultMessage, SbError inError)
            {
                SampleChatMain.Instance.UnblockUI();
                if (inError != null)
                {
                    SampleChatMain.Instance.OpenNotifyPopup($"ErrorCode:{inError.ErrorCode}\nErrorMessage:{inError.ErrorMessage}");
                }

                inCompletedHandler?.Invoke();
            }

            SampleChatMain.Instance.BlockUI();
            _groupChannel.SendUserMessage(inMessage, OnSendMessageCompleteHandler);
        }
    }
}