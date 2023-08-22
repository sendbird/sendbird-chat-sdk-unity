// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Base class for messages.
    /// </summary>
    /// @since 4.0.0
    public abstract partial class SbBaseMessage
    {
        /// <summary>
        /// All SbMessageMetaArray of the message.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<SbMessageMetaArray> MetaArrays => _metaArrays;

        /// <summary>
        /// The apple critical alert options of the message.
        /// </summary>
        /// @since 4.0.0
        public SbAppleCriticalAlertOptions AppleCriticalAlertOptions => _appleCriticalAlertOptions;

        /// <summary>
        /// The SbChannelType of the channel this message belongs to.
        /// </summary>
        /// @since 4.0.0
        public SbChannelType ChannelType => _channelType;

        /// <summary>
        /// The channel URL of the channel this message belongs to.
        /// </summary>
        /// @since 4.0.0
        public string ChannelUrl => _channelUrl;

        /// <summary>
        /// The creation time of the message in milliseconds.
        /// </summary>
        /// @since 4.0.0
        public long CreatedAt => _createdAt;

        /// <summary>
        /// The custom type of the message.
        /// </summary>
        /// @since 4.0.0
        public string CustomType => _customType;

        /// <summary>
        /// The custom data of the message.
        /// </summary>
        /// @since 4.0.0
        public string Data => _data;

        /// <summary>
        /// The error code of them message if the sendingStatus is SbSendingStatus.Failed.
        /// </summary>
        /// @since 4.0.0
        public int ErrorCode => (int)_errorCode;

        /// <summary>
        /// The template for the message.
        /// </summary>
        /// @since 4.0.0
        public IDictionary<string, string> ExtendedMessage => _extendedMessage;

        /// <summary>
        /// The sending status of the message.
        /// </summary>
        /// @since 4.0.0
        public SbSendingStatus SendingStatus => _sendingStatus;

        /// <summary>
        /// Whether this message is from SbGroupChannel.
        /// </summary>
        /// @since 4.0.0
        public bool IsGroupChannel => ChannelType == SbChannelType.Group;

        /// <summary>
        /// Whether this message is from SbOpenChannel.
        /// </summary>
        /// @since 4.0.0
        public bool IsOpenChannel => ChannelType == SbChannelType.Open;

        /// <summary>
        /// Whether this message is from SbFeedChannel.
        /// </summary>
        /// @since 4.0.0
        public bool IsFeedChannel => ChannelType == SbChannelType.Feed;

        /// <summary>
        /// Whether the message was sent from an operator.
        /// </summary>
        /// @since 4.0.0
        public bool IsOperatorMessage => isOperatorMessage;

        /// <summary>
        /// Determines whether the current message is a replied message and also a message was replied to the channel.
        /// </summary>
        /// @since 4.0.0
        public bool IsReplyToChannel => _isReplyToChannel;

        /// <summary>
        /// Checks whether the message is silent or not.
        /// </summary>
        /// @since 4.0.0
        public bool IsSilent => _isSilent;

        /// <summary>
        /// The mentioned message template of the message.
        /// </summary>
        /// @since 4.0.0
        public string MentionedMessageTemplate => _mentionedMessageTemplate;

        /// <summary>
        /// 
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<SbUser> MentionedUsers => _mentionedUsers;

        /// <summary>
        /// The mentioned users of the message.
        /// </summary>
        /// @since 4.0.0
        public SbMentionType MentionType => _mentionType;

        /// <summary>
        /// The message text of the message.
        /// </summary>
        /// @since 4.0.0
        public string Message => message;

        /// <summary>
        /// The SbBaseMessageCreateParams object that used for sending this message For more details, Please refer SbUserMessage.MessageCreateParams, SbFileMessage.MessageCreateParams
        /// </summary>
        /// @since 4.0.0
        public SbBaseMessageCreateParams MessageCreateParams => _messageCreateParams;

        /// <summary>
        /// The ID of the message.
        /// </summary>
        /// @since 4.0.0
        public long MessageId => _messageId;

        /// <summary>
        /// The message's survival seconds.
        /// </summary>
        /// @since 4.0.0
        public int MessageSurvivalSeconds => _messageSurvivalSeconds;

        /// <summary>
        /// The SbOgMetaData of the message.
        /// </summary>
        /// @since 4.0.0
        public SbOgMetaData OgMetaData => _ogMetaData;

        /// <summary>
        /// 
        /// </summary>
        /// @since 4.0.0
        public SbBaseMessage ParentMessage => _parentMessage;

        /// <summary>
        /// The parent message of this message. Only NonNull if this message is a reply message. It does not contain all properties of the parent message.
        /// </summary>
        /// @since 4.0.0
        public long ParentMessageId => _parentMessageId;

        /// <summary>
        /// The reactions on the message.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<SbReaction> Reactions => _reactions;

        /// <summary>
        /// The request ID of the message.
        /// </summary>
        /// @since 4.0.0
        public string RequestId => _requestId;

        /// <summary>
        /// The thread info of the message.
        /// </summary>
        /// @since 4.0.0
        public SbThreadInfo ThreadInfo => _threadInfo;

        /// <summary>
        /// The updated time of the message in milliseconds.
        /// </summary>
        /// @since 4.0.0
        public long UpdatedAt => _updatedAt;

        /// <summary>
        /// The Sender of the message. If SendbirdChat.Options.UseMemberInfoInMessage is set true and this message belongs to a GroupChannel (not a super group channel), the sender information, such as nickname and profile url, is returned as the same with the GroupChannel.members. Otherwise, the sender information will be returned as the value of the message creation time.
        /// </summary>
        /// @since 4.0.0
        public SbSender Sender => _sender;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inReactionEvent"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public bool ApplyReactionEvent(SbReactionEvent inReactionEvent)
        {
            return ApplyReactionEventInternal(inReactionEvent);
        }

        /// <summary>
        /// Applies SbThreadInfoUpdateEvent to a message.
        /// </summary>
        /// <param name="inThreadInfoUpdateEvent"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public bool ApplyThreadInfoUpdateEvent(SbThreadInfoUpdateEvent inThreadInfoUpdateEvent)
        {
            return ApplyThreadInfoUpdateEventInternal(inThreadInfoUpdateEvent);
        }

        /// <summary>
        /// Whether the message is resendable.
        /// </summary>
        /// <returns></returns>
        /// @since 4.0.0
        public bool IsResendable()
        {
            return IsResendableInternal();
        }

        /// <summary>
        /// Retrieves the threaded replies of the current message depending on the timestamp. If the current message doesnt have replies, the result is an empty list. The result is passed to handler as list.
        /// </summary>
        /// <param name="inTimestamp"></param>
        /// <param name="inParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetThreadedMessagesByTimestamp(long inTimestamp, SbThreadedMessageListParams inParams, SbThreadedMessageListHandler inCompletionHandler)
        {
            GetThreadedMessagesByTimestampInternal(inTimestamp, inParams, inCompletionHandler);
        }
    }
}