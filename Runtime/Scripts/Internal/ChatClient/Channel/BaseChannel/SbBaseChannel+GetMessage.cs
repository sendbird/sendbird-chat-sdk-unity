// 
//  Copyright (c) 2023 Sendbird, Inc.
// 


using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbBaseChannel
    {
        internal void GetMessagesByTimestampInternal(long inTimestamp, SbMessageListParams inParams, SbMessageListHandler inCompletionHandler)
        {
            GetMessagesInternal(SbBaseMessage.INVALID_MESSAGE_ID_MIN, inTimestamp, inParams, inCompletionHandler);
        }

        private void GetMessagesByMessageIdInternal(long inMessageId, SbMessageListParams inParams, SbMessageListHandler inCompletionHandler)
        {
            GetMessagesInternal(inMessageId, inTimestamp: 0, inParams, inCompletionHandler);
        }

        private void GetMessagesInternal(long inMessageId, long inTimestamp, SbMessageListParams inParams, SbMessageListHandler inCompletionHandler)
        {
            if (inParams == null)
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("MessageListParams");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return;
            }

            void OnResponse(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
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
                    resultMessages = new List<SbBaseMessage>(queryResponse.BaseMessageDtos.Count);
                    foreach (BaseMessageDto messageDto in queryResponse.BaseMessageDtos)
                    {
                        SbBaseMessage message = messageDto.CreateMessageInstance(chatMainContextRef);
                        resultMessages.Add(message);
                    }
                }

                inCompletionHandler?.Invoke(resultMessages, null);
            }

            MessageListQueryApiCommand.Request.Params requestParams = new MessageListQueryApiCommand.Request.Params()
            {
                prevLimit = inParams.PreviousResultSize,
                nextLimit = inParams.NextResultSize,
                inclusive = inParams.IsInclusive,
                reverse = inParams.Reverse,
                messageTypeFilter = inParams.MessageTypeFilter,
                customTypes = inParams.CustomTypes,
                senderUserIds = inParams.SenderUserIds,
                parentMessageId = SbBaseMessage.INVALID_MESSAGE_ID_MIN,
                showSubChannelMessagesOnly = inParams.ShowSubChannelMessagesOnly,
                includeMetaArray = inParams.IncludeMetaArray,
                includeReactions = inParams.IncludeReactions,
                includeThreadInfo = inParams.IncludeThreadInfo,
                includeParentMessageInfo = inParams.IncludeParentMessageInfo,
                replyType = inParams.ReplyType
            };

            if (SbBaseMessage.INVALID_MESSAGE_ID_MIN < inMessageId)
            {
                requestParams.messageId = inMessageId;
            }
            else
            {
                requestParams.messageTimestamp = inTimestamp;
            }

            MessageListQueryApiCommand.Request request = new MessageListQueryApiCommand.Request(ChannelType, _url, requestParams, OnResponse);
            chatMainContextRef.CommandRouter.RequestApiCommand(request);
        }

        internal static void GetMessage(SendbirdChatMainContext inChatMainContext, SbMessageRetrievalParams inParams, SbBaseMessageHandler inCompletionHandler)
        {
            if (inChatMainContext == null)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, SbErrorCodeExtension.INVALID_INITIALIZATION_ERROR));
                return;
            }

            if (inParams == null || inParams.MessageId <= SbBaseMessage.INVALID_MESSAGE_ID_MIN || string.IsNullOrEmpty(inParams.ChannelUrl))
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("MessageRetrievalParams");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                if (inResponse is GetMessageApiCommand.Response response && response.BaseMessageDto != null)
                {
                    SbBaseMessage message = response.BaseMessageDto.CreateMessageInstance(inChatMainContext);
                    inCompletionHandler?.Invoke(message, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            GetMessageApiCommand.Request apiCommand = new GetMessageApiCommand.Request(inParams, OnCompletionHandler);

            inChatMainContext.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}