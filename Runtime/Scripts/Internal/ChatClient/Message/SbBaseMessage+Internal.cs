// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Linq;

namespace Sendbird.Chat
{
    public abstract partial class SbBaseMessage
    {
        internal const long INVALID_MESSAGE_ID_MIN = 0;
        private readonly string _requestId;
        private readonly long _messageId = INVALID_MESSAGE_ID_MIN;
        private readonly SbChannelType _channelType;
        private readonly string _channelUrl;
        private readonly long _createdAt;
        private readonly long _updatedAt;
        private readonly string _customType;
        private readonly string _data;
        private SbErrorCode _errorCode;
        private readonly bool _isFeedChannel;
        private readonly bool _isReplyToChannel;
        private readonly bool _isSilent;
        private readonly string _mentionedMessageTemplate;
        private readonly SbMentionType _mentionType;
        protected string message;
        private readonly int _messageSurvivalSeconds;
        private readonly long _parentMessageId = INVALID_MESSAGE_ID_MIN;
        private readonly List<SbMessageMetaArray> _metaArrays = new List<SbMessageMetaArray>();
        private readonly SbAppleCriticalAlertOptions _appleCriticalAlertOptions;
        private readonly Dictionary<string, string> _extendedMessage = new Dictionary<string, string>();
        private readonly List<SbUser> _mentionedUsers = new List<SbUser>();
        private readonly SbBaseMessageCreateParams _messageCreateParams;
        private readonly SbOgMetaData _ogMetaData;
        private readonly SbBaseMessage _parentMessage;
        private SbThreadInfo _threadInfo;
        private readonly SbSender _sender = null;
        private SbSendingStatus _sendingStatus;
        private protected bool isOperatorMessage;
        private bool _isAutoResendRegistered;
        private readonly List<SbReaction> _reactions = new List<SbReaction>();
        private protected readonly SendbirdChatMainContext chatMainContextRef = null;

        private protected SbBaseMessage(SbBaseMessage inOtherMessage)
        {
            if (inOtherMessage == null)
                return;

            _channelType = inOtherMessage._channelType;
            _channelUrl = inOtherMessage._channelUrl;
            _requestId = inOtherMessage._requestId;
            _sender = inOtherMessage._sender;
            _messageId = inOtherMessage._messageId;
            message = inOtherMessage.message;
            _messageSurvivalSeconds = inOtherMessage._messageSurvivalSeconds;
            _parentMessage = inOtherMessage._parentMessage;
            _parentMessageId = inOtherMessage._parentMessageId;
            _createdAt = inOtherMessage._createdAt;
            _updatedAt = inOtherMessage._updatedAt;
            _customType = inOtherMessage._customType;
            _data = inOtherMessage._data;
            _errorCode = inOtherMessage._errorCode;
            _isFeedChannel = inOtherMessage._isFeedChannel;
            isOperatorMessage = inOtherMessage.isOperatorMessage;
            _isReplyToChannel = inOtherMessage._isReplyToChannel;
            _isSilent = inOtherMessage._isSilent;
            _mentionedMessageTemplate = inOtherMessage._mentionedMessageTemplate;
            _mentionType = inOtherMessage._mentionType;
            _sendingStatus = inOtherMessage._sendingStatus;
            _appleCriticalAlertOptions = inOtherMessage._appleCriticalAlertOptions?.Clone();
            _messageCreateParams = inOtherMessage._messageCreateParams?.Clone();
            _ogMetaData = inOtherMessage._ogMetaData?.Clone();
            _threadInfo = inOtherMessage._threadInfo?.Clone();

            if (inOtherMessage._mentionedUsers != null && 0 < inOtherMessage._mentionedUsers.Count)
                _mentionedUsers.AddRange(inOtherMessage._mentionedUsers);

            if (inOtherMessage._reactions != null && 0 < inOtherMessage._reactions.Count)
                _reactions.AddRange(inOtherMessage._reactions);

            if (inOtherMessage._metaArrays != null && 0 < inOtherMessage._metaArrays.Count)
                _metaArrays.AddRange(inOtherMessage._metaArrays);

            if (inOtherMessage._extendedMessage != null && 0 < inOtherMessage._extendedMessage.Count)
            {
                foreach (KeyValuePair<string, string> keyValuePair in inOtherMessage._extendedMessage)
                {
                    _extendedMessage.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        private protected SbBaseMessage(BaseMessageDto inBaseMessageDto, SendbirdChatMainContext inChatMainContext)
        {
            if (inBaseMessageDto == null)
                return;

            chatMainContextRef = inChatMainContext;
            _requestId = inBaseMessageDto.MessageCreatedRequestId;
            _channelType = inBaseMessageDto.ChannelType;
            _channelUrl = inBaseMessageDto.channelUrl;

            if (inBaseMessageDto.senderDto != null)
                _sender = new SbSender(inBaseMessageDto.senderDto, chatMainContextRef);

            _messageId = inBaseMessageDto.MessageId;
            message = inBaseMessageDto.message;
            _messageSurvivalSeconds = inBaseMessageDto.messageSurvivalSeconds;
            if (inBaseMessageDto.ParentMessageInfoDto != null)
                _parentMessage = inBaseMessageDto.ParentMessageInfoDto.CreateMessageInstance(chatMainContextRef);

            _parentMessageId = inBaseMessageDto.parentMessageId ?? INVALID_MESSAGE_ID_MIN;
            _createdAt = inBaseMessageDto.CreatedAt;
            _updatedAt = inBaseMessageDto.updatedAt;
            _customType = inBaseMessageDto.customType;
            _data = inBaseMessageDto.Data;
            _errorCode = (SbErrorCode)inBaseMessageDto.errorCode;
            isOperatorMessage = inBaseMessageDto.isOperatorMessage;
            _isReplyToChannel = inBaseMessageDto.isReplyToChannel;
            _isSilent = inBaseMessageDto.silent;
            _isAutoResendRegistered = inBaseMessageDto.autoResendRegistered;
            _mentionedMessageTemplate = inBaseMessageDto.mentionedMessageTemplate;
            if (inBaseMessageDto.mentionedUserDtos != null && 0 < inBaseMessageDto.mentionedUserDtos.Count)
            {
                _mentionedUsers = new List<SbUser>(inBaseMessageDto.mentionedUserDtos.Count);

                foreach (UserDto mentionedUser in inBaseMessageDto.mentionedUserDtos)
                {
                    SbUser user = new SbUser(mentionedUser, chatMainContextRef);
                    _mentionedUsers.Add(user);
                }
            }

            _mentionType = inBaseMessageDto.MentionType;
            _sendingStatus = inBaseMessageDto.RequestState;
            _extendedMessage = inBaseMessageDto.extendedMessage;

            if (inBaseMessageDto.MetaArrayDtos != null && 0 < inBaseMessageDto.MetaArrayDtos.Count)
            {
                _metaArrays = new List<SbMessageMetaArray>(inBaseMessageDto.MetaArrayDtos.Count);
                foreach (MessageMetaArrayDto metaArrayDto in inBaseMessageDto.MetaArrayDtos)
                {
                    SbMessageMetaArray messageMetaArray = new SbMessageMetaArray(metaArrayDto);
                    _metaArrays.Add(messageMetaArray);
                }
            }

            var reactionDtos = inBaseMessageDto.ReactionDtos;
            if (reactionDtos != null && 0 < reactionDtos.Count)
            {
                _reactions = new List<SbReaction>(reactionDtos.Count);
                foreach (ReactionDto reactionDto in reactionDtos)
                {
                    SbReaction reaction = new SbReaction(reactionDto);
                    _reactions.Add(reaction);
                }
            }

            if (inBaseMessageDto.OgMetaDataDto != null)
                _ogMetaData = new SbOgMetaData(inBaseMessageDto.OgMetaDataDto);

            if (inBaseMessageDto.appleCriticalAlertOptionsDto != null)
                _appleCriticalAlertOptions = new SbAppleCriticalAlertOptions(inBaseMessageDto.appleCriticalAlertOptionsDto);

            if (inBaseMessageDto.ThreadInfoDto != null)
                _threadInfo = new SbThreadInfo(inBaseMessageDto.ThreadInfoDto, chatMainContextRef);
        }

        private protected SbBaseMessage(SbBaseMessageCreateParams inBaseMessageCreateParams, SendbirdChatMainContext inChatMainContext,
                                        SbSender inSender, SbBaseChannel inBaseChannel, string inRequestId)
        {
            chatMainContextRef = inChatMainContext;
            _sender = inSender;
            _createdAt = TimeUtil.GetCurrentUnixTimeMilliseconds();
            _requestId = string.IsNullOrEmpty(inRequestId) ? NetworkUtil.GenerateReqId() : inRequestId;

            if (inBaseMessageCreateParams != null)
            {
                _messageCreateParams = inBaseMessageCreateParams.Clone();
                _appleCriticalAlertOptions = inBaseMessageCreateParams.AppleCriticalAlertOptions;
                _customType = inBaseMessageCreateParams.CustomType;
                _data = inBaseMessageCreateParams.Data;

                _mentionType = inBaseMessageCreateParams.MentionType;

                if (inBaseMessageCreateParams.MetaArrays != null && 0 < inBaseMessageCreateParams.MetaArrays.Count)
                    _metaArrays = new List<SbMessageMetaArray>(inBaseMessageCreateParams.MetaArrays);

                _parentMessageId = inBaseMessageCreateParams.ParentMessageId;
                _isReplyToChannel = inBaseMessageCreateParams.ReplyToChannel;
            }

            if (inBaseChannel != null)
            {
                _channelUrl = inBaseChannel.Url;
                _channelType = inBaseChannel.ChannelType;
            }
        }

        internal void SetSendingStatus(SbSendingStatus inSendingStatus)
        {
            _sendingStatus = inSendingStatus;
        }

        internal void SetErrorCode(SbErrorCode inErrorCode)
        {
            _errorCode = inErrorCode;
        }

        protected virtual SbBaseMessage Clone()
        {
            return null;
        }

        internal SbBaseMessage CloneWithFailedStatus(SbErrorCode inErrorCode)
        {
            SbBaseMessage clonedMessage = Clone();
            if (clonedMessage != null)
            {
                clonedMessage.SetSendingStatus(SbSendingStatus.Failed);
                clonedMessage.SetErrorCode(inErrorCode);
            }

            return clonedMessage;
        }

        private bool ApplyThreadInfoUpdateEventInternal(SbThreadInfoUpdateEvent inThreadInfoUpdateEvent)
        {
            if (inThreadInfoUpdateEvent == null || inThreadInfoUpdateEvent.TargetMessageId != _messageId || inThreadInfoUpdateEvent.ThreadInfo == null)
                return false;

            if (_threadInfo != null)
                return _threadInfo.Merge(inThreadInfoUpdateEvent.ThreadInfo);

            _threadInfo = new SbThreadInfo(inThreadInfoUpdateEvent.ThreadInfo);
            return true;
        }

        private bool ApplyReactionEventInternal(SbReactionEvent inReactionEvent)
        {
            if (inReactionEvent == null || _messageId != inReactionEvent.MessageId)
                return false;

            SbReaction foundReaction = _reactions.Find(inReaction => inReaction.Key == inReactionEvent.Key);
            if (foundReaction != null)
            {
                foundReaction.ApplyReactionEvent(inReactionEvent);
                if (foundReaction.GetUserCount() <= 0)
                {
                    _reactions.Remove(foundReaction);
                }

                return true;
            }

            if (inReactionEvent.Operation == SbReactionEventAction.Add)
            {
                SbReaction reaction = new SbReaction(inReactionEvent.Key, inReactionEvent.UserId, inReactionEvent.UpdatedAt);
                _reactions.Add(reaction);
                return true;
            }

            return false;
        }

        internal bool IsMentionedToMe(string inCurrentUserId)
        {
            if (string.IsNullOrEmpty(inCurrentUserId) || _sender?.UserId == inCurrentUserId)
                return false;

            if (_mentionType == SbMentionType.Channel)
                return true;

            if (_mentionType == SbMentionType.Users && _mentionedUsers != null)
            {
                return _mentionedUsers.Any(inUser => inUser.UserId == inCurrentUserId);
            }

            return false;
        }

        private bool IsResendableInternal()
        {
            if (_sendingStatus != SbSendingStatus.Failed && _sendingStatus != SbSendingStatus.Canceled)
                return false;

            if (SbBaseMessage.INVALID_MESSAGE_ID_MIN < _messageId)
                return false;

            if (_errorCode == SbErrorCode.ConnectionRequired ||
                _errorCode == SbErrorCode.NetworkError ||
                _errorCode == SbErrorCode.AckTimeout ||
                _errorCode == SbErrorCode.WebSocketConnectionClosed ||
                _errorCode == SbErrorCode.WebSocketConnectionFailed ||
                _errorCode == SbErrorCode.FileUploadCancelFailed ||
                _errorCode == SbErrorCode.FileUploadCanceled ||
                _errorCode == SbErrorCode.InternalError ||
                _errorCode == SbErrorCode.RateLimitExceeded ||
                _errorCode == SbErrorCode.SocketTooManyMessages ||
                _errorCode == SbErrorCode.PendingError)
                return true;

            return false;
        }

        private void GetThreadedMessagesByTimestampInternal(long inTimestamp, SbThreadedMessageListParams inParams, SbThreadedMessageListHandler inCompletionHandler)
        {
            if (inParams == null)
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("ThreadedMessageListParams");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, null, error));
                return;
            }

            void OnResponse(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, null, inError);
                    return;
                }

                if (!(inResponse is MessageListQueryApiCommand.Response queryResponse))
                {
                    inCompletionHandler?.Invoke(null, null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
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

                inCompletionHandler?.Invoke(this, resultMessages, null);
            }

            MessageListQueryApiCommand.Request.Params requestParams = new MessageListQueryApiCommand.Request.Params()
            {
                messageTimestamp = inTimestamp,
                messageId = SbBaseMessage.INVALID_MESSAGE_ID_MIN,
                prevLimit = inParams.PreviousResultSize,
                nextLimit = inParams.NextResultSize,
                inclusive = inParams.IsInclusive,
                reverse = inParams.Reverse,
                messageTypeFilter = inParams.MessageTypeFilter,
                customTypes = inParams.CustomTypes,
                senderUserIds = inParams.SenderUserIds,
                parentMessageId = _messageId,
                showSubChannelMessagesOnly = false,
                includeMetaArray = inParams.IncludeMetaArray,
                includeReactions = inParams.IncludeReactions,
                includeThreadInfo = true,
                includeParentMessageInfo = inParams.IncludeParentMessageInfo,
                replyType = SbReplyType.All
            };

            MessageListQueryApiCommand.Request request = new MessageListQueryApiCommand.Request(_channelType, _channelUrl, requestParams, OnResponse);
            chatMainContextRef.CommandRouter.RequestApiCommand(request);
        }
    }
}