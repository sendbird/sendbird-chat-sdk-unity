// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a group channel.
    /// </summary>
    /// @since 4.0.0
    public partial class SbGroupChannel : SbBaseChannel
    {
        /// <summary>
        /// SbChannelType
        /// </summary>
        /// @since 4.0.0
        public override SbChannelType ChannelType => SbChannelType.Group;

        /// <summary>
        /// The SbGroupChannelHiddenState of this channel.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelHiddenState HiddenState { get => _hiddenState; internal set => _hiddenState = value; }

        /// <summary>
        /// My invitation timestamp.
        /// </summary>
        /// @since 4.0.0
        public long InvitedAt { get => _invitedAt; internal set => _invitedAt = value; }

        /// <summary>
        /// The inviter of the current User to this channel.
        /// </summary>
        /// @since 4.0.0
        public SbUser Inviter => _inviter;

        /// <summary>
        /// Whether this channel requires access code to join. This value is valid only if this channel is a public GroupChannel.
        /// </summary>
        /// @since 4.0.0
        public bool IsAccessCodeRequired { get => _isAccessCodeRequired; internal set => _isAccessCodeRequired = value; }

        /// <summary>
        /// Checks whether this channel is a broadcast GroupChannel.
        /// </summary>
        /// @since 4.0.0
        public bool IsBroadcast => _isBroadcast;

        /// <summary>
        /// Checks whether this channel is discoverable in the result of PublicGroupChannelListQuery. If it is false, it will not appear on the result of PublicGroupChannelListQuery.
        /// </summary>
        /// @since 4.0.0
        public bool IsDiscoverable => _isDiscoverable;

        /// <summary>
        /// Checks whether this channel is a distinct GroupChannel. For a distinct GroupChannel, later when you create GroupChannel with same User and IsDistinct flag being true (refer to createChannel), the channel URL does not change, which means the messages between Users remain at the channel. If the channel is not distinct one, a new GroupChannel is created even though Users are same. As a result, you get a totally new channel URL (the old channel still remains), which means the Users start new conversation.
        /// </summary>
        /// @since 4.0.0
        public bool IsDistinct => _isDistinct;

        /// <summary>
        /// Checks whether this channel is an exclusive GroupChannel.
        /// </summary>
        /// @since 4.0.0
        public bool IsExclusive => _isExclusive;

        /// <summary>
        /// Checks whether this channel is hidden.
        /// </summary>
        /// @since 4.0.0
        public bool IsHidden { get => _isHidden; internal set => _isHidden = value; }

        /// <summary>
        /// Checks whether this channel is a public GroupChannel.
        /// </summary>
        /// @since 4.0.0
        public bool IsPublic => _isPublic;

        /// <summary>
        /// Checks whether this channel is a super GroupChannel.
        /// </summary>
        /// @since 4.0.0
        public bool IsSuper { get => _isSuper; internal set => _isSuper = value; }

        /// <summary>
        /// Whether one or more members are typing.
        /// </summary>
        /// @since 4.0.0
        public bool IsTyping => IsTypingInternal();

        /// <summary>
        /// The timestamp when the current user joined.
        /// </summary>
        /// @since 4.0.0
        public long JoinedAt { get => _joinedAt; internal set => _joinedAt = value; }

        /// <summary>
        /// The total member count for this channel.
        /// </summary>
        /// @since 4.0.0
        public int MemberCount => _memberCount;

        /// <summary>
        /// The total joined member count for this channel.
        /// </summary>
        /// @since 4.0.0
        public int JoinedMemberCount => _joinedMemberCount;

        /// <summary>
        /// The last message of the channel.
        /// </summary>
        /// @since 4.0.0
        public SbBaseMessage LastMessage { get => _lastMessage; internal set => _lastMessage = value; }

        /// <summary>
        /// The last message among channel's pinned messages.
        /// </summary>
        /// @since 4.0.0
        public SbBaseMessage LastPinnedMessage { get => _lastPinnedMessage; internal set => _lastPinnedMessage = value; }

        /// <summary>
        /// 
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<SbMember> Members => _members;

        /// <summary>
        /// The local timestamp of when this channel has been used in a SbMessageCollection.
        /// </summary>
        /// @since 4.0.0
        public long MessageCollectionLastAccessedAt => _messageCollectionLastAccessedAt;

        /// <summary>
        /// This property is set when resetMyHistory and hide is called.
        /// </summary>
        /// @since 4.0.0
        public long MessageOffsetTimestamp => _messageOffsetTimestamp;

        /// <summary>
        /// The message survival seconds in this channel.
        /// </summary>
        /// @since 4.0.0
        public int MessageSurvivalSeconds => _messageSurvivalSeconds;

        /// <summary>
        /// Checks whether unread message count is enabled for this channel. This count preference can be set by SetMyCountPreference.
        /// </summary>
        /// @since 4.0.0
        public SbCountPreference MyCountPreference => _myCountPreference;

        /// <summary>
        /// Current user's last read receipt timestamp in channel.
        /// </summary>
        /// @since 4.0.0
        public long MyLastRead { get => _myLastRead; internal set => _myLastRead = value; }

        /// <summary>
        /// My member state.
        /// </summary>
        /// @since 4.0.0
        public SbMemberState MyMemberState { get => _myMemberState; internal set => _myMemberState = value; }

        /// <summary>
        /// My muted state in this channel.
        /// </summary>
        /// @since 4.0.0
        public SbMutedState MyMutedState => _myMutedState;

        /// <summary>
        /// My push trigger option. The push trigger setting can be updated by SetMyPushTriggerOption.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelPushTriggerOption MyPushTriggerOption => _myPushTriggerOption;

        /// <summary>
        /// My Role in this channel.
        /// </summary>
        /// @since 4.0.0
        public SbRole MyRole { get => _myRole; internal set => _myRole = value; }

        /// <summary>
        /// The pinned message ids of the channel.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<long> PinnedMessageIds => _pinnedMessageIds;

        /// <summary>
        /// The typing user list.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<SbUser> TypingUsers => _typingUsers;

        /// <summary>
        /// The unread mention count for this channel for the current User.
        /// </summary>
        /// @since 4.0.0
        public int UnreadMentionCount { get => _unreadMentionCount; internal set => _unreadMentionCount = value; }

        /// <summary>
        /// The unread message count for this channel for the current User.
        /// </summary>
        /// @since 4.0.0
        public int UnreadMessageCount { get => _unreadMessageCount; internal set => _unreadMessageCount = value; }

        /// <summary>
        /// Deletes this GroupChannel. Note that only operators of a channel are able to delete it or else, an error will be returned to the handler.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Delete(SbErrorHandler inCompletionHandler)
        {
            DeleteInternal(inCompletionHandler);
        }

        /// <summary>
        /// Joins this channel if this channel is public.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Join(SbErrorHandler inCompletionHandler)
        {
            JoinInternal(null, inCompletionHandler);
        }

        /// <summary>
        /// Joins this channel if this channel is public.
        /// </summary>
        /// <param name="inAccessCode"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Join(string inAccessCode, SbErrorHandler inCompletionHandler)
        {
            JoinInternal(inAccessCode, inCompletionHandler);
        }

        /// <summary>
        /// Leaves this channel.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Leave(SbErrorHandler inCompletionHandler)
        {
            LeaveInternal(false, inCompletionHandler);
        }

        /// <summary>
        /// Leaves this channel.
        /// </summary>
        /// <param name="inShouldRemoveOperatorStatus"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Leave(bool inShouldRemoveOperatorStatus, SbErrorHandler inCompletionHandler)
        {
            LeaveInternal(inShouldRemoveOperatorStatus, inCompletionHandler);
        }

        /// <summary>
        /// Update this channel with GroupChannelUpdateParams.
        /// </summary>
        /// <param name="inChannelUpdateParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void UpdateChannel(SbGroupChannelUpdateParams inChannelUpdateParams, SbGroupChannelCallbackHandler inCompletionHandler)
        {
            UpdateChannelInternal(inChannelUpdateParams, inCompletionHandler);
        }

        /// <summary>
        /// Refreshes all the data of this channel.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Refresh(SbErrorHandler inCompletionHandler)
        {
            RefreshInternal(inCompletionHandler);
        }

        /// <summary>
        /// Hides this channel from the current User's GroupChannel` list. When a new message is received from the channel, it appears again.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Hide(SbErrorHandler inCompletionHandler)
        {
            HideInternal(false, true, inCompletionHandler);
        }

        /// <summary>
        /// Hides this channel from the current User's GroupChannel list. When a new message is received from the channel, it appears again.
        /// </summary>
        /// <param name="inHidePreviousMessages"></param>
        /// <param name="inAllowAutoUnhide"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Hide(bool inHidePreviousMessages, bool inAllowAutoUnhide, SbErrorHandler inCompletionHandler)
        {
            HideInternal(inHidePreviousMessages, inAllowAutoUnhide, inCompletionHandler);
        }

        /// <summary>
        /// Unhide this channel from the current User's GroupChannel list.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Unhide(SbErrorHandler inCompletionHandler)
        {
            UnhideInternal(inCompletionHandler);
        }

        /// <summary>
        /// Freeze this GroupChannel.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Freeze(SbErrorHandler inCompletionHandler)
        {
            FreezeInternal(inFreeze: true, inCompletionHandler);
        }

        /// <summary>
        /// Unfreeze this GroupChannel.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Unfreeze(SbErrorHandler inCompletionHandler)
        {
            FreezeInternal(inFreeze: false, inCompletionHandler);
        }

        /// <summary>
        /// Invites Users top this channel.
        /// </summary>
        /// <param name="inUserIds"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Invite(List<string> inUserIds, SbErrorHandler inCompletionHandler)
        {
            InviteInternal(inUserIds, inCompletionHandler);
        }

        /// <summary>
        /// Accepts the invitation sent to the current User. After the acceptance, the User will be joined to this GroupChannel.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void AcceptInvitation(SbErrorHandler inCompletionHandler)
        {
            AcceptInvitationInternal(null, inCompletionHandler);
        }

        /// <summary>
        /// Accepts the invitation sent to the current User. After the acceptance, the User will be joined to this GroupChannel.
        /// </summary>
        /// <param name="inAccessCode"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void AcceptInvitation(string inAccessCode, SbErrorHandler inCompletionHandler)
        {
            AcceptInvitationInternal(inAccessCode, inCompletionHandler);
        }

        /// <summary>
        /// Declines the invitation sent to the current User.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void DeclineInvitation(SbErrorHandler inCompletionHandler)
        {
            DeclineInvitationInternal(inCompletionHandler);
        }

        /// <summary>
        /// Sends start typing event.
        /// </summary>
        /// @since 4.0.0
        public void StartTyping()
        {
            StartTypingInternal();
        }

        /// <summary>
        /// Sends end typing event.
        /// </summary>
        /// @since 4.0.0
        public void EndTyping()
        {
            EndTypingInternal();
        }

        /// <summary>
        /// Creates a query instance to get members.
        /// </summary>
        /// <param name="inParams"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public SbMemberListQuery CreateMemberListQuery(SbMemberListQueryParams inParams)
        {
            return CreateMemberListQueryInternal(inParams);
        }

        /// <summary>
        /// Resets the chat history of this channel for the current User. After this call, the messages created before the call will not be loaded.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void ResetMyHistory(SbErrorHandler inCompletionHandler)
        {
            ResetMyHistoryInternal(inCompletionHandler);
        }

        /// <summary>
        /// Pins a message to the channel.
        /// </summary>
        /// <param name="inMessageId"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void PinMessage(long inMessageId, SbErrorHandler inCompletionHandler)
        {
            PinMessageInternal(inMessageId, inCompletionHandler);
        }

        /// <summary>
        /// Removes the message from the channel's pinned messages.
        /// </summary>
        /// <param name="inMessageId"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void UnpinMessage(long inMessageId, SbErrorHandler inCompletionHandler)
        {
            UnpinMessageInternal(inMessageId, inCompletionHandler);
        }

        /// <summary>
        /// Gets the member list who have read the given message.
        /// </summary>
        /// <param name="inMessage"></param>
        /// <param name="inIncludeAllMembers"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public IReadOnlyList<SbMember> GetReadMembers(SbBaseMessage inMessage, bool inIncludeAllMembers)
        {
            return GetReadMembersInternal(inMessage, inIncludeAllMembers);
        }

        /// <summary>
        /// Returns the number of member's that haven't read the given message.
        /// </summary>
        /// <param name="inMessage"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public int GetUnreadMemberCount(SbBaseMessage inMessage)
        {
            return GetUnreadMemberCountInternal(inMessage);
        }

        /// <summary>
        /// Sends mark as read to this channel with a CompletionHandler.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void MarkAsRead(SbErrorHandler inCompletionHandler)
        {
            MarkAsReadInternal(inCompletionHandler);
        }

        /// <summary>
        /// Creates MessageCollection instance with the params.
        /// </summary>
        /// <param name="inParams"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public SbMessageCollection CreateMessageCollection(SbMessageCollectionCreateParams inParams)
        {
            return CreateMessageCollectionInternal(inParams);
        }
    }
}