// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbBaseChannel
    {
        private SbUserMessage SendUserMessageInternal(SbUserMessageCreateParams inUserMessageCreateParams, string inRequestId, SbUserMessageHandler inCompletionHandler)
        {
            SbSender sender = new SbSender(chatMainContextRef.CurrentUserRef, chatMainContextRef, GetCurrentUserRole());
            bool isOperatorMessage = GetCurrentUserRole() == SbRole.Operator;
            SbUserMessage pendingMessage = SbUserMessage.CreateMessage(inUserMessageCreateParams, chatMainContextRef, sender, this, isOperatorMessage, inRequestId);
            pendingMessage.SetSendingStatus(SbSendingStatus.Pending);
            NotifyMessagePending(pendingMessage);

            if (inUserMessageCreateParams == null || string.IsNullOrEmpty(inUserMessageCreateParams.Message))
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("Params");
                SbUserMessage failedMessage = pendingMessage.CloneWithFailedStatus(error.ErrorCode) as SbUserMessage;
                NotifyMessageFailed(failedMessage);
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(failedMessage, error));
                return failedMessage;
            }

            if (chatMainContextRef.CurrentUserRef == null)
            {
                SbError error = SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR;
                SbUserMessage failedMessage = pendingMessage.CloneWithFailedStatus(error.ErrorCode) as SbUserMessage;
                NotifyMessageFailed(failedMessage);
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(failedMessage, error));
                return failedMessage;
            }

            void OnSendUserMessageApiCommandCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    SbUserMessage failedMessage = pendingMessage.CloneWithFailedStatus(inError.ErrorCode) as SbUserMessage;
                    NotifyMessageFailed(failedMessage);
                    inCompletionHandler?.Invoke(failedMessage, inError);
                    return;
                }

                if (inResponse is SendUserMessageApiCommand.Response messageResponse && messageResponse.UserMessageDtoDto != null)
                {
                    SbUserMessage userMessage = new SbUserMessage(messageResponse.UserMessageDtoDto, chatMainContextRef);
                    NotifyMessageSent(userMessage);
                    inCompletionHandler?.Invoke(userMessage, null);
                }
                else
                {
                    SbError error = SbErrorCodeExtension.MALFORMED_DATA_ERROR;
                    SbUserMessage failedMessage = pendingMessage.CloneWithFailedStatus(error.ErrorCode) as SbUserMessage;
                    NotifyMessageFailed(failedMessage);
                    inCompletionHandler?.Invoke(failedMessage, error);
                }
            }

            void OnSendUserMessageWsCommandAck(WsReceiveCommandAbstract inWsReceiveCommand, SbError inError)
            {
                if (inError != null)
                {
                    SendUserMessageApiCommand.Request apiCommand = new SendUserMessageApiCommand.Request(
                        pendingMessage.RequestId, Url, ChannelType, chatMainContextRef.CurrentUserId,
                        inUserMessageCreateParams, OnSendUserMessageApiCommandCompletionHandler);

                    chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
                    return;
                }

                SbUserMessage userMessage = null;
                if (inWsReceiveCommand is UserMessageWsReceiveCommand userMessageWsReceiveCommand)
                {
                    if (userMessageWsReceiveCommand.BaseMessageDto is UserMessageDto userMessageDto)
                    {
                        userMessage = new SbUserMessage(userMessageDto, chatMainContextRef);
                    }
                }

                if (userMessage != null)
                {
                    NotifyMessageSent(userMessage);
                    inCompletionHandler?.Invoke(userMessage, null);
                }
                else
                {
                    SbError error = SbErrorCodeExtension.MALFORMED_DATA_ERROR;
                    SbUserMessage failedMessage = pendingMessage.CloneWithFailedStatus(error.ErrorCode) as SbUserMessage;
                    NotifyMessageFailed(failedMessage);
                    inCompletionHandler?.Invoke(failedMessage, error);
                }
            }

            SendUserMessageWsSendCommand command = new SendUserMessageWsSendCommand(pendingMessage.RequestId, _url, inUserMessageCreateParams, OnSendUserMessageWsCommandAck);
            chatMainContextRef.CommandRouter.SendWsCommand(command);

            return pendingMessage;
        }

        private void UpdateUserMessageInternal(long inMessageId, SbUserMessageUpdateParams inParams, SbUserMessageHandler inCompletionHandler)
        {
            if (inMessageId <= SbBaseMessage.INVALID_MESSAGE_ID_MIN)
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("MessageId");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return;
            }

            if (inParams == null)
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("Params");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return;
            }

            if (chatMainContextRef.CurrentUserRef == null)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR));
                return;
            }

            void OnUpdateUserMessageWsCommandAck(WsReceiveCommandAbstract inWsReceiveCommand, SbError inError)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                SbUserMessage userMessage = null;
                if (inWsReceiveCommand is UpdateUserMessageWsReceiveCommand userMessageReceiveCommand && userMessageReceiveCommand.BaseMessageDto != null)
                {
                    if (userMessageReceiveCommand.BaseMessageDto is UserMessageDto userMessageDto)
                    {
                        userMessage = new SbUserMessage(userMessageDto, chatMainContextRef);
                    }
                }

                if (userMessage != null)
                {
                    inCompletionHandler?.Invoke(userMessage, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            UpdateUserMessageWsSendCommand command = new UpdateUserMessageWsSendCommand(_url, inMessageId, inParams, OnUpdateUserMessageWsCommandAck);
            chatMainContextRef.CommandRouter.SendWsCommand(command);
        }

        private SbUserMessage ResendUserMessageInternal(SbUserMessage inUserMessage, SbUserMessageHandler inCompletionHandler)
        {
            SbError error = null;
            if (inUserMessage == null)
            {
                error = SbErrorCodeExtension.CreateInvalidParameterError("Message");
            }
            else if (inUserMessage.ChannelUrl != _url)
            {
                error = new SbError(SbErrorCode.InvalidParameter, SbErrorCodeExtension.DescriptionDefines.MESSAGE_DOESNT_BELONG_TO_CHANNEL);
            }
            else if (inUserMessage.IsResendable() == false)
            {
                error = new SbError(SbErrorCode.InvalidParameter, SbErrorCodeExtension.DescriptionDefines.NOT_RESEND_ABLE);
            }

            if (error != null)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                inUserMessage?.SetErrorCode(error.ErrorCode);
                return inUserMessage;
            }

            SbUserMessageCreateParams userMessageCreateParams = new SbUserMessageCreateParams(inUserMessage);
            SendUserMessageInternal(userMessageCreateParams, inUserMessage?.RequestId, inCompletionHandler);

            return inUserMessage;
        }

        private SbUserMessage CopyUserMessageInternal(SbUserMessage inUserMessage, SbBaseChannel inToTargetChannel, SbUserMessageHandler inCompletionHandler)
        {
            SbError error = null;
            if (inUserMessage == null || inUserMessage.MessageId <= SbBaseMessage.INVALID_MESSAGE_ID_MIN)
            {
                error = SbErrorCodeExtension.CreateInvalidParameterError("Message");
            }
            else if (inUserMessage.ChannelUrl != _url)
            {
                error = new SbError(SbErrorCode.InvalidParameter, SbErrorCodeExtension.DescriptionDefines.MESSAGE_DOESNT_BELONG_TO_CHANNEL);
            }
            else if (inUserMessage.SendingStatus != SbSendingStatus.Succeeded)
            {
                error = new SbError(SbErrorCode.InvalidParameter, SbErrorCodeExtension.DescriptionDefines.NEED_SUCCEEDED_MESSAGE);
            }

            if (error != null)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return null;
            }

            SbUserMessageCreateParams userMessageCreateParams = new SbUserMessageCreateParams(inUserMessage);
            userMessageCreateParams.ParentMessageId = SbBaseMessage.INVALID_MESSAGE_ID_MIN;
            return inToTargetChannel.SendUserMessageInternal(userMessageCreateParams, inRequestId: null, inCompletionHandler);
        }
    }
}