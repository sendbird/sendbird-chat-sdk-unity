// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbBaseChannel
    {
        private void GetMessageChangeLogsSinceTimestampInternal(long inTimestamp, SbMessageChangeLogsParams inParams, SbMessageChangeLogHandler inCompletionHandler)
        {
            GetMessageChangeLogs(inToken: null, inTimestamp, inParams, inCompletionHandler);
        }

        private void GetMessageChangeLogsSinceTokenInternal(string inToken, SbMessageChangeLogsParams inParams, SbMessageChangeLogHandler inCompletionHandler)
        {
            GetMessageChangeLogs(inToken, inTimestamp: 0, inParams, inCompletionHandler);
        }

        private void GetMessageChangeLogs(string inToken, long inTimestamp, SbMessageChangeLogsParams inParams, SbMessageChangeLogHandler inCompletionHandler)
        {
            if (inParams == null)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("MessageChangeLogsParams");
                    CoroutineManager.Instance.CallOnNextFrame(
                        () => inCompletionHandler.Invoke(inUpdatedMessages: null, inDeletedMessageIds: null, inHasMore: false, inToken: null, error));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is GetMessageChangeLogsApiCommand.Response response)
                {
                    long lastSyncAt = 0;
                    List<SbBaseMessage> updatedMessage = null;
                    if (response.UpdatedBaseMessageDtos != null && 0 < response.UpdatedBaseMessageDtos.Count)
                    {
                        updatedMessage = new List<SbBaseMessage>(response.UpdatedBaseMessageDtos.Count);
                        foreach (BaseMessageDto baseMessageDto in response.UpdatedBaseMessageDtos)
                        {
                            lastSyncAt = Math.Max(lastSyncAt, baseMessageDto.CreatedAt);
                            updatedMessage.Add(baseMessageDto.CreateMessageInstance(chatMainContextRef));
                        }
                    }

                    List<long> deletedMessageIds = null;
                    if (response.deletedMessageDtos != null && 0 < response.deletedMessageDtos.Count)
                    {
                        deletedMessageIds = new List<long>(response.deletedMessageDtos.Count);
                        foreach (DeletedMessageDto deletedMessageDto in response.deletedMessageDtos)
                        {
                            lastSyncAt = Math.Max(lastSyncAt, deletedMessageDto.deletedAt);
                            deletedMessageIds.Add(deletedMessageDto.messageId);
                        }
                    }

                    if (0 < lastSyncAt && LastSyncedChangeLogsTimestamp < lastSyncAt)
                    {
                        LastSyncedChangeLogsTimestamp = lastSyncAt;
                    }

                    inCompletionHandler?.Invoke(updatedMessage, deletedMessageIds, response.hasMore, response.token, null);
                }
                else
                {
                    if (inError != null)
                        inError = SbErrorCodeExtension.MALFORMED_DATA_ERROR;

                    inCompletionHandler?.Invoke(inUpdatedMessages: null, inDeletedMessageIds: null, inHasMore: false, inToken: null, inError);
                }
            }

            GetMessageChangeLogsApiCommand.Request apiCommand = new GetMessageChangeLogsApiCommand.Request(Url, ChannelType, inToken, inTimestamp, inParams, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}