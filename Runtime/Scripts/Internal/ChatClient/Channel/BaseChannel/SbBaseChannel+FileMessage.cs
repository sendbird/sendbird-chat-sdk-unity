// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.IO;

namespace Sendbird.Chat
{
    public partial class SbBaseChannel
    {
        internal SbFileMessage SendFileMessageInternalForceApi(SbFileMessageCreateParams inFileMessageCreateParams, SbMultiProgressHandler inProgressHandler, SbFileMessageHandler inCompletionHandler)
        {
            return SendFileMessageInternal(inFileMessageCreateParams, inRequestId: null, inProgressHandler, inCompletionHandler, inForceApi: true);
        }
        private SbFileMessage SendFileMessageInternal(SbFileMessageCreateParams inFileMessageCreateParams, string inRequestId,
                                                      SbMultiProgressHandler inProgressHandler, SbFileMessageHandler inCompletionHandler, bool inForceApi = false)
        {
            SbSender sender = new SbSender(chatMainContextRef.CurrentUserRef, chatMainContextRef, GetCurrentUserRole());
            bool isOperatorMessage = GetCurrentUserRole() == SbRole.Operator;
            SbFileMessage pendingMessage = SbFileMessage.CreateMessage(inFileMessageCreateParams, chatMainContextRef, sender, this, isOperatorMessage, inRequestId);
            pendingMessage.SetSendingStatus(SbSendingStatus.Pending);
            NotifyMessagePending(pendingMessage);

            SbFileMessageCreateParams fileMessageCreateParams = pendingMessage.MessageCreateParams as SbFileMessageCreateParams;
            bool requireAuth = false;
            List<ThumbnailDto> thumbnailDtos = null;
            inFileMessageCreateParams = null;

            if (fileMessageCreateParams == null || (fileMessageCreateParams.File == null && string.IsNullOrEmpty(fileMessageCreateParams.FileUrl)))
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("File");
                SbFileMessage failedMessage = pendingMessage.CloneWithFailedStatus(error.ErrorCode) as SbFileMessage;
                NotifyMessageFailed(failedMessage);
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(failedMessage, error));
                return failedMessage;
            }

            if (chatMainContextRef.CurrentUserRef == null)
            {
                SbError error = SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR;
                SbFileMessage failedMessage = pendingMessage.CloneWithFailedStatus(error.ErrorCode) as SbFileMessage;
                NotifyMessageFailed(failedMessage);
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(failedMessage, error));
                return pendingMessage;
            }

            void OnSendFileApiCommandCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    SbFileMessage failedMessage = pendingMessage.CloneWithFailedStatus(inError.ErrorCode) as SbFileMessage;
                    NotifyMessageFailed(failedMessage);
                    inCompletionHandler?.Invoke(failedMessage, inError);
                    return;
                }

                if (inResponse is SendFileMessageApiCommand.Response fileMessageResponse && fileMessageResponse.FileMessageDtoDto != null)
                {
                    SbFileMessage fileMessage = new SbFileMessage(fileMessageResponse.FileMessageDtoDto, chatMainContextRef);
                    NotifyMessageSent(fileMessage);
                    inCompletionHandler?.Invoke(fileMessage, null);
                }
                else
                {
                    SbError error = SbErrorCodeExtension.MALFORMED_DATA_ERROR;
                    SbFileMessage failedMessage = pendingMessage.CloneWithFailedStatus(error.ErrorCode) as SbFileMessage;
                    NotifyMessageFailed(failedMessage);
                    inCompletionHandler?.Invoke(failedMessage, error);
                }
            }

            void OnSendFileMessageWsCommandAck(WsReceiveCommandAbstract inWsReceiveCommand, SbError inError)
            {
                if (inError != null)
                {
                    SendFileMessageApiCommand.Request apiCommand = new SendFileMessageApiCommand.Request(
                        pendingMessage.RequestId, Url, ChannelType, chatMainContextRef.CurrentUserId,
                        fileMessageCreateParams, requireAuth, thumbnailDtos, OnSendFileApiCommandCompletionHandler);

                    chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
                    return;
                }

                SbFileMessage fileMessage = null;
                if (inWsReceiveCommand is FileMessageWsReceiveCommand fileMessageReceiveCommand)
                {
                    if (fileMessageReceiveCommand.BaseMessageDto is FileMessageDto fileMessageDto)
                    {
                        fileMessage = new SbFileMessage(fileMessageDto, chatMainContextRef);
                    }
                }

                if (fileMessage != null)
                {
                    NotifyMessageSent(fileMessage);
                    inCompletionHandler?.Invoke(fileMessage, null);
                }
                else
                {
                    SbError error = SbErrorCodeExtension.MALFORMED_DATA_ERROR;
                    SbFileMessage failedMessage = pendingMessage.CloneWithFailedStatus(error.ErrorCode) as SbFileMessage;
                    NotifyMessageFailed(failedMessage);
                    inCompletionHandler?.Invoke(failedMessage, error);
                }
            }

            void OnUploadFileCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is UploadFileApiCommand.Response uploadFileResponse)
                {
                    fileMessageCreateParams.SetFileUrlAndNullFile(uploadFileResponse.url);
                    fileMessageCreateParams.FileSize = uploadFileResponse.fileSize;
                    requireAuth = uploadFileResponse.requireAuth;
                    thumbnailDtos = uploadFileResponse.thumbnailDtos;
                    if (inForceApi == false)
                    {
                        SendFileMessageWsSendCommand command = new SendFileMessageWsSendCommand(
                            pendingMessage.RequestId, _url, requireAuth, thumbnailDtos, fileMessageCreateParams, OnSendFileMessageWsCommandAck);

                        chatMainContextRef.CommandRouter.SendWsCommand(command);
                    }
                    else
                    {
                        SendFileMessageApiCommand.Request apiCommand = new SendFileMessageApiCommand.Request(
                            pendingMessage.RequestId, Url, ChannelType, chatMainContextRef.CurrentUserId,
                            fileMessageCreateParams, requireAuth, thumbnailDtos, OnSendFileApiCommandCompletionHandler);

                        chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
                    }
                   
                }
                else if (inIsCanceled)
                {
                    SbError error = new SbError(SbErrorCode.FileUploadCanceled);
                    SbFileMessage canceledMessage = pendingMessage.CloneWithFailedStatus(error.ErrorCode) as SbFileMessage;
                    NotifyMessageCanceled(canceledMessage);
                    inCompletionHandler?.Invoke(canceledMessage, error);
                }
                else
                {
                    SbFileMessage failedMessage = pendingMessage.CloneWithFailedStatus(inError.ErrorCode) as SbFileMessage;
                    NotifyMessageFailed(failedMessage);
                    inCompletionHandler?.Invoke(failedMessage, inError);
                }
            }

            if (fileMessageCreateParams.File != null)
            {
                UploadFileApiCommand.Request apiCommand = new UploadFileApiCommand.Request(
                    Url, pendingMessage.RequestId, fileMessageCreateParams.FileName, fileMessageCreateParams.File, fileMessageCreateParams.ThumbnailSizes, OnUploadFileCompletionHandler, inProgressHandler);

                chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
            }
            else
            {
                SendFileMessageWsSendCommand command = new SendFileMessageWsSendCommand(pendingMessage.RequestId, _url, false, null, fileMessageCreateParams, OnSendFileMessageWsCommandAck);
                chatMainContextRef.CommandRouter.SendWsCommand(command);
            }

            return pendingMessage;
        }

        private bool CancelUploadingFileMessageInternal(string inRequestId)
        {
            if (string.IsNullOrEmpty(inRequestId))
                return false;

            return chatMainContextRef.CommandRouter.CancelRequestingApiCommand(inRequestId);
        }

        private void UpdateFileMessageInternal(long inMessageId, SbFileMessageUpdateParams inParams, SbFileMessageHandler inCompletionHandler)
        {
            SbError error = null;
            if (inMessageId <= SbBaseMessage.INVALID_MESSAGE_ID_MIN)
                error = SbErrorCodeExtension.CreateInvalidParameterError("MessageId");

            if (inParams == null)
                error = SbErrorCodeExtension.CreateInvalidParameterError("Params");

            if (chatMainContextRef.CurrentUserRef == null)
                error = SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR;

            if (error != null)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return;
            }

            void OnUpdateFileWsCommandAck(WsReceiveCommandAbstract inWsReceiveCommand, SbError inError)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                SbFileMessage fileMessage = null;
                if (inWsReceiveCommand is UpdateFileMessageWsReceiveCommand fileMessageReceiveCommand && fileMessageReceiveCommand.BaseMessageDto != null)
                {
                    if (fileMessageReceiveCommand.BaseMessageDto is FileMessageDto fileMessageDto)
                    {
                        fileMessage = new SbFileMessage(
                            fileMessageDto, chatMainContextRef);
                    }
                }

                if (fileMessage != null)
                {
                    inCompletionHandler?.Invoke(fileMessage, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            UpdateFileMessageWsSendCommand command = new UpdateFileMessageWsSendCommand(_url, inMessageId, inParams, OnUpdateFileWsCommandAck);
            chatMainContextRef.CommandRouter.SendWsCommand(command);
        }

        private SbFileMessage ResendFileMessageInternal(SbFileMessage inFileMessage, SbFileInfo inFileInfo, SbMultiProgressHandler inProgressHandler, SbFileMessageHandler inCompletionHandler)
        {
            SbError error = null;
            if (inFileMessage == null)
            {
                error = SbErrorCodeExtension.CreateInvalidParameterError("Message");
            }
            else if (inFileMessage.ChannelUrl != _url)
            {
                error = new SbError(SbErrorCode.InvalidParameter, SbErrorCodeExtension.DescriptionDefines.MESSAGE_DOESNT_BELONG_TO_CHANNEL);
            }
            else if (string.IsNullOrEmpty(inFileMessage.PlainUrl) && (inFileInfo == null || inFileInfo.IsExists() == false))
            {
                error = new SbError(SbErrorCode.InvalidParameter, "The file url or file data");
            }
            else if (inFileMessage.IsResendable() == false)
            {
                error = new SbError(SbErrorCode.InvalidParameter, SbErrorCodeExtension.DescriptionDefines.NOT_RESEND_ABLE);
            }

            if (error != null)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                inFileMessage?.SetErrorCode(error.ErrorCode);
                return inFileMessage;
            }

            SbFileMessageCreateParams fileMessageCreateParams = new SbFileMessageCreateParams(inFileMessage);
            if (inFileInfo != null)
                fileMessageCreateParams.File = inFileInfo;

            return SendFileMessageInternal(fileMessageCreateParams, inFileMessage?.RequestId, inProgressHandler, inCompletionHandler);
        }

        private SbFileMessage CopyFileMessageInternal(SbFileMessage inFileMessage, SbBaseChannel inToTargetChannel, SbFileMessageHandler inCompletionHandler)
        {
            SbError error = null;
            if (inFileMessage == null || inFileMessage.MessageId <= SbBaseMessage.INVALID_MESSAGE_ID_MIN)
            {
                error = SbErrorCodeExtension.CreateInvalidParameterError("Message");
            }
            else if (inFileMessage.ChannelUrl != _url)
            {
                error = new SbError(SbErrorCode.InvalidParameter, SbErrorCodeExtension.DescriptionDefines.MESSAGE_DOESNT_BELONG_TO_CHANNEL);
            }
            else if (inFileMessage.SendingStatus != SbSendingStatus.Succeeded)
            {
                error = new SbError(SbErrorCode.InvalidParameter, SbErrorCodeExtension.DescriptionDefines.NEED_SUCCEEDED_MESSAGE);
            }

            if (error != null)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return null;
            }

            SbFileMessageCreateParams fileMessageCreateParams = new SbFileMessageCreateParams(inFileMessage)
            {
                ParentMessageId = SbBaseMessage.INVALID_MESSAGE_ID_MIN
            };

            SbFileMessage pendingMessage = inToTargetChannel.SendFileMessageInternal(fileMessageCreateParams, inRequestId: null, inProgressHandler: null, inCompletionHandler);
            if (pendingMessage != null)
            {
                pendingMessage.ClearAndCopyThumbnails(inFileMessage?.Thumbnails);
            }

            return pendingMessage;
        }
    }
}