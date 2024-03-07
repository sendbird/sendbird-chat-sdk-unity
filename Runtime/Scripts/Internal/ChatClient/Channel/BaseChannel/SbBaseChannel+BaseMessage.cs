// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbBaseChannel
    {
        private void DeleteMessageInternal(long inMessageId, SbErrorHandler inCompletionHandler)
        {
            SbError error = null;
            if (inMessageId <= SbBaseMessage.INVALID_MESSAGE_ID_MIN)
            {
                error = SbErrorCodeExtension.CreateInvalidParameterError("Message");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(error));
                return;
            }

            void OnDeleteMessageCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            DeleteMessageApiCommand.Request request = new DeleteMessageApiCommand.Request(_url, ChannelType, inMessageId, OnDeleteMessageCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(request);
        }

        private void TranslateUserMessageInternal(SbBaseMessage inBaseMessage, List<string> inTargetLanguages, SbUserMessageHandler inCompletionHandler)
        {
            SbError error = null;
            if (inBaseMessage == null || inBaseMessage.MessageId <= SbBaseMessage.INVALID_MESSAGE_ID_MIN)
            {
                error = SbErrorCodeExtension.CreateInvalidParameterError("Message");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return;
            }

            if (inBaseMessage.ChannelUrl != _url)
            {
                error = new SbError(SbErrorCode.InvalidParameter, SbErrorCodeExtension.DescriptionDefines.MESSAGE_DOESNT_BELONG_TO_CHANNEL);
            }
            else if (inBaseMessage.SendingStatus != SbSendingStatus.Succeeded)
            {
                error = new SbError(SbErrorCode.InvalidParameter, SbErrorCodeExtension.DescriptionDefines.NEED_SUCCEEDED_MESSAGE);
            }

            if (error != null)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return;
            }

            void OnTranslateUserMessageCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                if (inResponse is TranslateUserMessageApiCommand.Response messageResponse && messageResponse.UserMessageDtoDto != null)
                {
                    SbUserMessage userMessage = new SbUserMessage(messageResponse.UserMessageDtoDto, chatMainContextRef);
                    inCompletionHandler?.Invoke(userMessage, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            TranslateUserMessageApiCommand.Request command = new TranslateUserMessageApiCommand.Request(
                _url, ChannelType, inBaseMessage.MessageId, inTargetLanguages, OnTranslateUserMessageCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(command);
        }

        private SbPreviousMessageListQuery CreatePreviousMessageListQueryInternal(SbPreviousMessageListQueryParams inParams = null)
        {
            if (inParams == null)
                inParams = new SbPreviousMessageListQueryParams();
            
            return new SbPreviousMessageListQuery(ChannelType, _url, inParams, chatMainContextRef);
        }
    }
}