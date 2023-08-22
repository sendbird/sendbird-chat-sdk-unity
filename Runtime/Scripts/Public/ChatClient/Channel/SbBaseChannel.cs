// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.IO;

namespace Sendbird.Chat
{
    /// <summary>
    /// Objects representing a channel.
    /// </summary>
    /// @since 4.0.0
    public abstract partial class SbBaseChannel
    {
        /// <summary>
        /// The unique channel URL.
        /// </summary>
        /// @since 4.0.0
        public string Url => _url;
        
        /// @since 4.0.0
        public abstract SbChannelType ChannelType { get; }

        /// <summary>
        /// All locally cached metadata as a map. Cached metadata is updated under following circumstances:
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyDictionary<string, string> CachedMetaData => _cachedMetaDataByKey;

        /// <summary>
        /// The cover image URL.
        /// </summary>
        /// @since 4.0.0
        public string CoverUrl => _coverUrl;

        /// <summary>
        /// The creation time of the channel.
        /// </summary>
        /// @since 4.0.0
        public long CreatedAt => _createdAt;

        /// <summary>
        /// The channel data.
        /// </summary>
        /// @since 4.0.0
        public string Data => _data;

        /// <summary>
        /// Whether the channel is ephemeral.
        /// </summary>
        /// @since 4.0.0
        public bool IsEphemeral => _isEphemeral;

        /// <summary>
        /// Whether the channel is frozen.
        /// </summary>
        /// @since 4.0.0
        public bool IsFrozen { get => _isFrozen; internal set => _isFrozen = value; }

        /// <summary>
        /// The topic or name of the channel.
        /// </summary>
        /// @since 4.0.0
        public string Name => _name;

        /// <summary>
        /// The custom type of the channel.
        /// </summary>
        /// @since 4.0.0
        public string CustomType => _customType;

        /// <summary>
        /// Whether the instance is SbGroupChannel type.
        /// </summary>
        /// @since 4.0.0
        public bool IsGroupChannel => ChannelType == SbChannelType.Group;

        /// <summary>
        /// Whether the instance is SbOpenChannel type.
        /// </summary>
        /// @since 4.0.0
        public bool IsOpenChannel => ChannelType == SbChannelType.Open;

        /// <summary>
        /// Sends a user message.
        /// </summary>
        /// <param name="inMessage"></param>
        /// <param name="inCompletionHandler"></param>
        /// <returns>Returns a temporary user message being sent to the Sendbird server. The message has a request ID instead of a message ID. The request status of the message is pending. If you try to send a message with an invalid parameter, the returned message is a user message with no properties. You can perform a validation of pending message by checking for the existence of the request ID.</returns>
        /// @since 4.0.0
        public SbUserMessage SendUserMessage(string inMessage, SbUserMessageHandler inCompletionHandler)
        {
            SbUserMessageCreateParams userMessageCreateParams = new SbUserMessageCreateParams(inMessage);
            return SendUserMessageInternal(userMessageCreateParams, inRequestId: null, inCompletionHandler);
        }

        /// <summary>
        /// Sends a string message of params.
        /// </summary>
        /// <param name="inUserMessageCreateParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// <returns>Returns a temporary user message being sent to the Sendbird server. The message has a request ID instead of a message ID. The request status of the message is pending. If you try to send a message with an invalid parameter, the returned message is a user message with no properties. You can perform a validation of pending message by checking for the existence of the request ID.</returns>
        /// @since 4.0.0
        public SbUserMessage SendUserMessage(SbUserMessageCreateParams inUserMessageCreateParams, SbUserMessageHandler inCompletionHandler)
        {
            return SendUserMessageInternal(inUserMessageCreateParams, inRequestId: null, inCompletionHandler);
        }

        /// <summary>
        /// Attempts to resend a failed user message received by the failure callback. Only failed message MUST be passed, not a succeeded message or a pending message.
        /// </summary>
        /// <param name="inUserMessage"></param>
        /// <param name="inCompletionHandler"></param>
        /// <returns>Returns a temporary user message being sent to the Sendbird server.</returns>
        /// @since 4.0.0
        public SbUserMessage ResendUserMessage(SbUserMessage inUserMessage, SbUserMessageHandler inCompletionHandler)
        {
            return ResendUserMessageInternal(inUserMessage, inCompletionHandler);
        }

        /// <summary>
        /// Updates a user message. The text message, data, custom type from user message params can be updated.
        /// </summary>
        /// <param name="inMessageId"></param>
        /// <param name="inParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void UpdateUserMessage(long inMessageId, SbUserMessageUpdateParams inParams, SbUserMessageHandler inCompletionHandler)
        {
            UpdateUserMessageInternal(inMessageId, inParams, inCompletionHandler);
        }

        /// <summary>
        /// Copies a user message to the target channel.
        /// </summary>
        /// <param name="inUserMessage"></param>
        /// <param name="inToTargetChannel"></param>
        /// <param name="inCompletionHandler"></param>
        /// <returns>Returns the temporary user message with a request ID. It doesn’t have a message ID. If there is any error, nil could be returned.</returns>
        /// @since 4.0.0
        public SbUserMessage CopyUserMessage(SbUserMessage inUserMessage, SbBaseChannel inToTargetChannel, SbUserMessageHandler inCompletionHandler)
        {
            return CopyUserMessageInternal(inUserMessage, inToTargetChannel, inCompletionHandler);
        }

        /// <summary>
        /// Requests to translate the text message into the target languages.
        /// </summary>
        /// <param name="inUserMessage"></param>
        /// <param name="inTargetLanguages"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void TranslateUserMessage(SbUserMessage inUserMessage, List<string> inTargetLanguages, SbUserMessageHandler inCompletionHandler)
        {
            TranslateUserMessageInternal(inUserMessage, inTargetLanguages, inCompletionHandler);
        }

        /// <summary>
        /// Sends a file message with file or file URL of params with progress. If the params has a binary file, it will upload data to Sendbird storage. If not, the params has a file url, it will send a message with file url.
        /// </summary>
        /// <param name="inFileMessageCreateParams">The instance of SbFileMessageCreateParams that can has parameters related with file.</param>
        /// <param name="inProgressHandler">The handler block to be used to monitor progression. inRequestId is a unique identifier for the message send request. inBytesSent is the number of bytes sent since this method was called. inTotalBytesSent is the total number of bytes sent so far. inTotalBytesExpectedToSend is the expected length of the body data. These parameters are the same to the declaration of</param>
        /// <param name="inCompletionHandler">The handler block to be executed after every message was sent. This block has no return value and takes an error. If the sending message can’t start at all because of the connection issue, the error isn’t null.</param>
        /// <returns>Returns a temporary file message being sent to the Sendbird server. The message has a request ID instead of a message ID. The request status of the message is pending. If you try to send a message with an invalid parameter, the returned message is a user message with no properties. You can perform a validation of pending message by checking for the existence of the request ID.</returns>
        /// @since 4.0.0
        public SbFileMessage SendFileMessage(SbFileMessageCreateParams inFileMessageCreateParams, SbMultiProgressHandler inProgressHandler, SbFileMessageHandler inCompletionHandler)
        {
            return SendFileMessageInternal(inFileMessageCreateParams, inRequestId: null, inProgressHandler, inCompletionHandler);
        }

        /// <summary>
        /// Attempts to resend a failed file message received by the failure callback.
        /// </summary>
        /// <param name="inFileMessage">A message to send. A failed message is passed to the callback when fails to send a message</param>
        /// <param name="inFileInfo"></param>
        /// <param name="inProgressHandler">The handler block to be used to monitor progression. inRequestId is a unique identifier for the message send request. inBytesSent is the number of bytes sent since this method was called. inTotalBytesSent is the total number of bytes sent so far. inTotalBytesExpectedToSend is the expected length of the body data. These parameters are the same to the declaration of</param>
        /// <param name="inCompletionHandler">The handler block to be executed after the message is sent. This block has no return value and takes two arguments. One is a file message. If the message is successfully sent, the complete message instance is delivered. If the message fails to be sent, a failed message based on the pending message is delivered except invalid parameter error. If failed because of invalid parameter, message is nil. Another factor is errors. If the message fails to be sent, a message error is dispatched.</param>
        /// <returns>Returns a temporary file message being sent to the Sendbird server.</returns>
        /// @since 4.0.0
        public SbFileMessage ResendFileMessage(SbFileMessage inFileMessage, SbFileInfo inFileInfo, SbMultiProgressHandler inProgressHandler, SbFileMessageHandler inCompletionHandler)
        {
            return ResendFileMessageInternal(inFileMessage, inFileInfo, inProgressHandler, inCompletionHandler);
        }

        /// <summary>
        /// Updates a file message. The data, custom type from file message params can be updated.
        /// </summary>
        /// <param name="inMessageId">The message ID of the message to be updated.</param>
        /// <param name="inParams">The file message params that has updating fields.</param>
        /// <param name="inCompletionHandler">The handler block to be executed after update.</param>
        /// @since 4.0.0
        public void UpdateFileMessage(long inMessageId, SbFileMessageUpdateParams inParams, SbFileMessageHandler inCompletionHandler)
        {
            UpdateFileMessageInternal(inMessageId, inParams, inCompletionHandler);
        }

        /// <summary>
        /// Copies a file message to the target channel.
        /// </summary>
        /// <param name="inFileMessage">File message object.</param>
        /// <param name="inToTargetChannel">Target channel object.</param>
        /// <param name="inCompletionHandler">The handler block to execute. The fileMessage is a user message which is returned from the Sendbird server. The message has a message ID.</param>
        /// <returns></returns>
        /// @since 4.0.0
        public SbFileMessage CopyFileMessage(SbFileMessage inFileMessage, SbBaseChannel inToTargetChannel, SbFileMessageHandler inCompletionHandler)
        {
            return CopyFileMessageInternal(inFileMessage, inToTargetChannel, inCompletionHandler);
        }
        
        /// <summary>
        /// Cancels an ongoing FileMessage upload.
        /// </summary>
        /// <param name="inRequestId"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public bool CancelUploadingFileMessage(string inRequestId)
        {
            return CancelUploadingFileMessageInternal(inRequestId);
        }

        /// <summary>
        /// Deletes a message with message ID.
        /// </summary>
        /// <param name="inMessageId"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void DeleteMessage(long inMessageId, SbErrorHandler inCompletionHandler)
        {
            DeleteMessageInternal(inMessageId, inCompletionHandler);
        }

        /// <summary>
        /// Retrieves previous or next messages based on the timestamp in a specific channel. The result is passed to handler as list.
        /// </summary>
        /// <param name="inTimestamp"></param>
        /// <param name="inParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetMessagesByTimestamp(long inTimestamp, SbMessageListParams inParams, SbMessageListHandler inCompletionHandler)
        {
            GetMessagesByTimestampInternal(inTimestamp, inParams, inCompletionHandler);
        }

        /// <summary>
        /// Retrieves previous or next messages based on the message ID in a specific channel. The result is passed to handler as list.
        /// </summary>
        /// <param name="inMessageId"></param>
        /// <param name="inParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetMessagesByMessageId(long inMessageId, SbMessageListParams inParams, SbMessageListHandler inCompletionHandler)
        {
            GetMessagesByMessageIdInternal(inMessageId, inParams, inCompletionHandler);
        }

        /// <summary>
        /// Creates meta data. This can be used to customize the channel.
        /// </summary>
        /// <param name="inMetaData"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void CreateMetaData(Dictionary<string, string> inMetaData, SbMetaDataHandler inCompletionHandler)
        {
            CreateMetaDataInternal(inMetaData, inCompletionHandler);
        }

        /// <summary>
        /// Gets the meta data for the channel.
        /// </summary>
        /// <param name="inKeys"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetMetaData(List<string> inKeys, SbMetaDataHandler inCompletionHandler)
        {
            GetMetaDataInternal(inKeys, inCompletionHandler);
        }

        /// <summary>
        /// Gets all meta data for the channel.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetAllMetaData(SbMetaDataHandler inCompletionHandler)
        {
            GetAllMetaDataInternal(inCompletionHandler);
        }

        /// <summary>
        /// Updates the meta data for the channel.
        /// </summary>
        /// <param name="inMetaData"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void UpdateMetaData(Dictionary<string, string> inMetaData, SbMetaDataHandler inCompletionHandler)
        {
            UpdateMetaDataInternal(inMetaData, inCompletionHandler);
        }

        /// <summary>
        /// Deletes meta data with key for the channel.
        /// </summary>
        /// <param name="inKey"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void DeleteMetaData(string inKey, SbErrorHandler inCompletionHandler)
        {
            DeleteMetaDataInternal(inKey, inCompletionHandler);
        }

        /// <summary>
        /// Deletes all meta data for the channel.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void DeleteAllMetaData(SbErrorHandler inCompletionHandler)
        {
            DeleteAllMetaDataInternal(inCompletionHandler);
        }

        /// <summary>
        /// Creates the meta counters for the channel.
        /// </summary>
        /// <param name="inMetaCounters"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void CreateMetaCounters(Dictionary<string, int> inMetaCounters, SbMetaCountersHandler inCompletionHandler)
        {
            CreateMetaCountersInternal(inMetaCounters, inCompletionHandler);
        }

        /// <summary>
        /// Gets the meta counters with keys for the channel.
        /// </summary>
        /// <param name="inKeys"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetMetaCounters(List<string> inKeys, SbMetaCountersHandler inCompletionHandler)
        {
            GetMetaCountersInternal(inKeys, inCompletionHandler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetAllMetaCounters(SbMetaCountersHandler inCompletionHandler)
        {
            GetAllMetaCountersInternal(inCompletionHandler);
        }

        /// <summary>
        /// Gets all meta counters for the channel.
        /// </summary>
        /// <param name="inMetaCounters"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void UpdateMetaCounters(Dictionary<string, int> inMetaCounters, SbMetaCountersHandler inCompletionHandler)
        {
            UpdateMetaCountersInternal(inMetaCounters, UpdateMetaCountersApiCommand.UpdateModeType.Set, inUpsert: true, inCompletionHandler);
        }

        /// <summary>
        /// Increases the meta counters for the channel.
        /// </summary>
        /// <param name="inMetaCounters"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void IncreaseMetaCounters(Dictionary<string, int> inMetaCounters, SbMetaCountersHandler inCompletionHandler)
        {
            UpdateMetaCountersInternal(inMetaCounters, UpdateMetaCountersApiCommand.UpdateModeType.Increase, inUpsert: false, inCompletionHandler);
        }

        /// <summary>
        /// Decreases the meta counters for the channel.
        /// </summary>
        /// <param name="inMetaCounters"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void DecreaseMetaCounters(Dictionary<string, int> inMetaCounters, SbMetaCountersHandler inCompletionHandler)
        {
            UpdateMetaCountersInternal(inMetaCounters, UpdateMetaCountersApiCommand.UpdateModeType.Decrease, inUpsert: false, inCompletionHandler);
        }

        /// <summary>
        /// Deletes the meta counters with key for the channel.
        /// </summary>
        /// <param name="inKey"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void DeleteMetaCounters(string inKey, SbErrorHandler inCompletionHandler)
        {
            DeleteMetaCountersInternal(inKey, inCompletionHandler);
        }

        /// <summary>
        /// Deletes all meta counters for the channel.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void DeleteAllMetaCounters(SbErrorHandler inCompletionHandler)
        {
            DeleteAllMetaCountersInternal(inCompletionHandler);
        }

        /// <summary>
        /// Creates a query instance to get the operator list from this channel.
        /// </summary>
        /// <param name="inParams"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public SbOperatorListQuery CreateOperatorListQuery(SbOperatorListQueryParams inParams = null)
        {
            return CreateOperatorListQueryInternal(inParams);
        }

        /// <summary>
        /// Creates previous message list query for this channel.
        /// </summary>
        /// <param name="inParams"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public SbPreviousMessageListQuery CreatePreviousMessageListQuery(SbPreviousMessageListQueryParams inParams = null)
        {
            return CreatePreviousMessageListQueryInternal(inParams);
        }

        /// <summary>
        /// Requests message change logs after given timestamp. The result is passed to handler.
        /// </summary>
        /// <param name="inTimestamp"></param>
        /// <param name="inParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetMessageChangeLogsSinceTimestamp(long inTimestamp, SbMessageChangeLogsParams inParams, SbMessageChangeLogHandler inCompletionHandler)
        {
            GetMessageChangeLogsSinceTimestampInternal(inTimestamp, inParams, inCompletionHandler);
        }

        /// <summary>
        /// Requests message change logs after given token. The result is passed to handler.
        /// </summary>
        /// <param name="inToken"></param>
        /// <param name="inParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetMessageChangeLogsSinceToken(string inToken, SbMessageChangeLogsParams inParams, SbMessageChangeLogHandler inCompletionHandler)
        {
            GetMessageChangeLogsSinceTokenInternal(inToken, inParams, inCompletionHandler);
        }

        /// <summary>
        /// Creates a query instance for banned user list of the channel.
        /// </summary>
        /// <param name="inParams">SbBannedUserListQueryParams instance.</param>
        /// <returns>The instance for the banned user list. Query only banned user list.</returns>
        /// @since 4.0.0
        public SbBannedUserListQuery CreateBannedUserListQuery(SbBannedUserListQueryParams inParams)
        {
            return CreateBannedUserListQueryInternal(inParams);
        }

        /// <summary>
        /// Creates a query instance for getting muted user list of the channel instance.
        /// </summary>
        /// <param name="inParams">SbMutedUserListQueryParams instance.</param>
        /// <returns>UserListQuery instance for the muted user list.</returns>
        /// @since 4.0.0
        public SbMutedUserListQuery CreateMutedUserListQuery(SbMutedUserListQueryParams inParams)
        {
            return CreateMutedUserListQueryInternal(inParams);
        }

        /// <summary>
        /// Bans a user for seconds. Let a user out and prevent to join again. If the user is already banned, duration will be updated from the time that was initialized.
        /// </summary>
        /// <param name="inUserId">The user ID to be banned.</param>
        /// <param name="inSeconds">Seconds of duration to be banned. Seconds should be larger than -1. If it is -1, user is banned forever. If it is 0, duration is set 10 years by default.</param>
        /// <param name="inDescription">The reason why the user was banned.</param>
        /// <param name="inCompletionHandler">The handler block to be executed after the user is banned. This block has no return value and takes an argument that is an error made when there is something wrong to ban.</param>
        /// @since 4.0.0
        public void BanUser(string inUserId, int inSeconds, string inDescription, SbErrorHandler inCompletionHandler)
        {
            BanUserInternal(inUserId, inSeconds, inDescription, inCompletionHandler);
        }

        /// <summary>
        /// Removes ban for a user.
        /// </summary>
        /// <param name="inUserId">The user ID to be removed ban.</param>
        /// <param name="inCompletionHandler">The handler block to be executed after remove ban. This block has no return value and takes an argument that is an error made when there is something wrong to remove ban.</param>
        /// @since 4.0.0
        public void UnbanUser(string inUserId, SbErrorHandler inCompletionHandler)
        {
            UnbanUserInternal(inUserId, inCompletionHandler);
        }

        /// <summary>
        /// Mutes a user for seconds. Muted user cannot send any messages to the group channel.
        /// </summary>
        /// <param name="inUserId">The user to be muted.</param>
        /// <param name="inSeconds">The user cannot send any messages for this time.</param>
        /// <param name="inDescription">The description that explains the reason why the user is muted.</param>
        /// <param name="inCompletionHandler">The handler block to be executed after mute. This block has no return value and takes an argument that is an error made when there is something wrong to mute the user.</param>
        /// @since 4.0.0
        public void MuteUser(string inUserId, int inSeconds, string inDescription, SbErrorHandler inCompletionHandler)
        {
            MuteUserInternal(inUserId, inSeconds, inDescription, inCompletionHandler);
        }

        /// <summary>
        /// Unmutes a user.
        /// </summary>
        /// <param name="inUserId">The user ID to be turned off mute.</param>
        /// <param name="inCompletionHandler">The handler block to be executed after turn off mute. This block has no return value and takes an argument that is an error made when there is something wrong to turn off mute.</param>
        /// @since 4.0.0
        public void UnmuteUser(string inUserId, SbErrorHandler inCompletionHandler)
        {
            UnmuteUserInternal(inUserId, inCompletionHandler);
        }

        /// <summary>
        /// Gets my muted information in this channel.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetMyMutedInfo(SbMuteInfoHandler inCompletionHandler)
        {
            GetMyMutedInfoInternal(inCompletionHandler);
        }

        /// <summary>
        /// Add operators to the channel. 
        /// </summary>
        /// <param name="inUserIds"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void AddOperators(List<string> inUserIds, SbErrorHandler inCompletionHandler)
        {
            AddOperatorsInternal(inUserIds, inCompletionHandler);
        }

        /// <summary>
        /// Remove operators from the channel.
        /// </summary>
        /// <param name="inUserIds"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void RemoveOperators(List<string> inUserIds, SbErrorHandler inCompletionHandler)
        {
            RemoveOperatorsInternal(inUserIds, inCompletionHandler);
        }

        /// <summary>
        /// Remove all operators from the channel. See https://docs.sendbird.com/platform/user_type#3_operator for the explanations on the operators.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void RemoveAllOperators(SbErrorHandler inCompletionHandler)
        {
            RemoveAllOperatorsInternal(inCompletionHandler);
        }

        /// <summary>
        /// Adds a reaction to a message.
        /// </summary>
        /// <param name="inMessage">The message object that the reaction will be added.</param>
        /// <param name="inKey">The reaction key to be added.</param>
        /// <param name="inCompletionHandler">The handler block to be executed. If succeeded, the SbReactionEvent will have the information of the reaction.</param>
        /// @since 4.0.0
        public void AddReaction(SbBaseMessage inMessage, string inKey, SbReactionEventHandler inCompletionHandler)
        {
            AddReactionInternal(inMessage, inKey, inCompletionHandler);
        }

        /// <summary>
        /// Deletes a reaction from a message.
        /// </summary>
        /// <param name="inMessage">The message object that has the reaction key.</param>
        /// <param name="inKey">The reaction key to be deleted.</param>
        /// <param name="inCompletionHandler">The handler block to be executed. If succeeded, the SbReactionEvent will have the information of the reaction.</param>
        /// @since 4.0.0
        public void DeleteReaction(SbBaseMessage inMessage, string inKey, SbReactionEventHandler inCompletionHandler)
        {
            DeleteReactionInternal(inMessage, inKey, inCompletionHandler);
        }

        /// <summary>
        /// Creates keys of meta array for the message.
        /// </summary>
        /// <param name="inMessage"></param>
        /// <param name="inKeys"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void CreateMessageMetaArrayKeys(SbBaseMessage inMessage, List<string> inKeys, SbBaseMessageHandler inCompletionHandler)
        {
            CreateMessageMetaArrayKeysInternal(inMessage, inKeys, inCompletionHandler);
        }

        /// <summary>
        /// Deletes keys from meta array of the message.
        /// </summary>
        /// <param name="inMessage"></param>
        /// <param name="inKeys"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void DeleteMessageMetaArrayKeys(SbBaseMessage inMessage, List<string> inKeys, SbBaseMessageHandler inCompletionHandler)
        {
            DeleteMessageMetaArrayKeysInternal(inMessage, inKeys, inCompletionHandler);
        }

        /// <summary>
        /// Adds meta array to the message.
        /// </summary>
        /// <param name="inMessage"></param>
        /// <param name="inMetaArrays"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void AddMessageMetaArrayValues(SbBaseMessage inMessage, List<SbMessageMetaArray> inMetaArrays, SbBaseMessageHandler inCompletionHandler)
        {
            AddMessageMetaArrayValuesInternal(inMessage, inMetaArrays, inCompletionHandler);
        }

        /// <summary>
        /// Removes meta array from the message.
        /// </summary>
        /// <param name="inMessage"></param>
        /// <param name="inMetaArrays"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void RemoveMessageMetaArrayValues(SbBaseMessage inMessage, List<SbMessageMetaArray> inMetaArrays, SbBaseMessageHandler inCompletionHandler)
        {
            RemoveMessageMetaArrayValuesInternal(inMessage, inMetaArrays, inCompletionHandler);
        }

        /// <summary>
        /// Reports a user in a channel of inappropriate activities.
        /// </summary>
        /// <param name="inOffendingUser"></param>
        /// <param name="inReportCategory"></param>
        /// <param name="inReportDescription"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void ReportUser(SbUser inOffendingUser, SbReportCategory inReportCategory, string inReportDescription, SbErrorHandler inCompletionHandler)
        {
            ReportUserInternal(inOffendingUser, inReportCategory, inReportDescription, inCompletionHandler);
        }

        /// <summary>
        /// Reports a malicious message in the channel
        /// </summary>
        /// <param name="inMessage"></param>
        /// <param name="inReportCategory"></param>
        /// <param name="inReportDescription"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void ReportMessage(SbBaseMessage inMessage, SbReportCategory inReportCategory, string inReportDescription, SbErrorHandler inCompletionHandler)
        {
            ReportMessageInternal(inMessage, inReportCategory, inReportDescription, inCompletionHandler);
        }

        /// <summary>
        /// Reports current channel instance of inappropriate activities.
        /// </summary>
        /// <param name="inReportCategory"></param>
        /// <param name="inReportDescription"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Report(SbReportCategory inReportCategory, string inReportDescription, SbErrorHandler inCompletionHandler)
        {
            ReportInternal(inReportCategory, inReportDescription, inCompletionHandler);
        }
    }
}