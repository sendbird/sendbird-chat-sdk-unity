// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbGroupChannel : IGroupChannelComparable
    {
        internal const long TYPING_INVALIDATE_TIME = 10;
        internal const long INVALID_MESSAGE_OFFSET_TIMESTAMP = 0;

        private SbGroupChannelHiddenState _hiddenState = SbGroupChannelHiddenState.Unhidden;
        private long _invitedAt;
        private SbUser _inviter;
        private bool _isAccessCodeRequired;
        private bool _isBroadcast;
        private bool _isChatNotification;
        private bool _isDiscoverable;
        private bool _isDistinct;
        private bool _isExclusive;
        private bool _isHidden;
        private bool _isPublic;
        private bool _isSuper;
        private long _joinedAt;
        private SbBaseMessage _lastMessage;
        private readonly List<SbMember> _members = new List<SbMember>();
        private long _messageCollectionLastAccessedAt;
        private long _messageOffsetTimestamp = INVALID_MESSAGE_OFFSET_TIMESTAMP;
        private int _messageSurvivalSeconds;
        private SbCountPreference _myCountPreference;
        private long _myLastRead;
        private SbMemberState _myMemberState;
        private SbMutedState _myMutedState;
        private SbGroupChannelPushTriggerOption _myPushTriggerOption;
        private SbRole _myRole;
        private readonly List<long> _pinnedMessageIds = new List<long>();
        private SbBaseMessage _lastPinnedMessage;
        internal long PinnedMessageUpdatedAt { get; set; }
        private int _unreadMentionCount;
        private int _unreadMessageCount;
        private int _memberCount;
        private int _joinedMemberCount;
        private long _memberCountUpdatedAt = 0;
        private readonly Dictionary<string, long> _readReceiptsByUserId = new Dictionary<string, long>();
        private readonly Dictionary<string, long> _deliveryReceiptsByUserId = new Dictionary<string, long>();

        private long _startTypingLastSentAtMs = 0;
        private long _endTypingLastSentAtMs = 0;
        private readonly List<SbUser> _typingUsers = new List<SbUser>();
        private readonly Dictionary<string, long> _typingUserTimestampsByUserId = new Dictionary<string, long>();


        internal SbGroupChannel(string inChannelUrl, SendbirdChatMainContext inChatMainContext) : base(inChannelUrl, inChatMainContext) { }

        private protected override void OnResetFromChannelDto(BaseChannelDto inBaseChannelDto)
        {
            if (!(inBaseChannelDto is GroupChannelDto groupChannelDto))
            {
                Logger.Warning(Logger.CategoryType.Channel, "SbGroupChannel::OnResetFromChannelDto() Command object is null.");
                return;
            }

            _hiddenState = groupChannelDto.HiddenState;
            _invitedAt = groupChannelDto.invitedAt;

            if (groupChannelDto.inviter != null)
                _inviter = new SbUser(groupChannelDto.inviter, chatMainContextRef);

            _isAccessCodeRequired = groupChannelDto.isAccessCodeRequired;
            _isBroadcast = groupChannelDto.isBroadcast;
            _isChatNotification = groupChannelDto.isChatNotification;
            _isDiscoverable = groupChannelDto.isDiscoverable;
            _isDistinct = groupChannelDto.isDistinct;
            _isExclusive = groupChannelDto.isExclusive;
            _isHidden = groupChannelDto.isHidden;
            _isPublic = groupChannelDto.isPublic;
            _isSuper = groupChannelDto.isSuper;
            _joinedAt = groupChannelDto.joinedTs;

            if (groupChannelDto.LastMessage != null)
                _lastMessage = groupChannelDto.LastMessage.CreateMessageInstance(chatMainContextRef);

            if (groupChannelDto.LatestPinnedMessage != null)
                _lastPinnedMessage = groupChannelDto.LatestPinnedMessage.CreateMessageInstance(chatMainContextRef);

            SetMessageOffsetTimestamp(groupChannelDto.tsMessageOffset);
            _messageSurvivalSeconds = groupChannelDto.messageSurvivalSeconds;
            _myCountPreference = groupChannelDto.CountPreference;
            _myLastRead = groupChannelDto.userLastRead;
            _myMemberState = groupChannelDto.MemberState;
            _myMutedState = groupChannelDto.MutedState;
            _myPushTriggerOption = groupChannelDto.GroupChannelPushTriggerOption;
            _myRole = groupChannelDto.MyRole;
            _unreadMentionCount = groupChannelDto.unreadMentionCount;
            _unreadMessageCount = groupChannelDto.unreadMessageCount;
            SetPinnedMessageIds(groupChannelDto.pinnedMessageIds);

            ResetMembersFromChannelDto(groupChannelDto);
        }

        internal void ResetMembersFromChannelDto(GroupChannelDto inGroupChannelDto)
        {
            if (inGroupChannelDto == null)
                return;

            _members.Clear();

            if (inGroupChannelDto.members != null && 0 < inGroupChannelDto.members.Count)
            {
                foreach (MemberDto memberDto in inGroupChannelDto.members)
                {
                    if (memberDto == null)
                        continue;

                    SbMember member = new SbMember(memberDto, chatMainContextRef);
                    _members.Add(member);
                }
            }

            _joinedMemberCount = inGroupChannelDto.joinedMemberCount;
            _memberCount = inGroupChannelDto.memberCount;
        }

        protected override SbRole GetCurrentUserRole()
        {
            return _myRole;
        }

        private void JoinInternal(string inAccessCode, SbErrorHandler inCompletionHandler)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is JoinGroupChannelApiCommand.Response joinGroupChannelResponse && joinGroupChannelResponse.GroupChannelDto != null)
                {
                    OnResetFromChannelDto(joinGroupChannelResponse.GroupChannelDto);
                }

                inCompletionHandler?.Invoke(inError);
            }

            JoinGroupChannelApiCommand.Request apiCommand = new JoinGroupChannelApiCommand.Request(Url, chatMainContextRef.CurrentUserId, inAccessCode, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void LeaveInternal(bool inShouldRemoveOperatorStatus = false, SbErrorHandler inCompletionHandler = null)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    _joinedAt = 0;
                    _invitedAt = 0;
                    chatMainContextRef.GroupChannelManager.RemoveCachedChannelIfContains(Url);
                }

                inCompletionHandler?.Invoke(inError);
            }

            LeaveGroupChannelApiCommand.Request apiCommand = new LeaveGroupChannelApiCommand.Request(Url, chatMainContextRef.CurrentUserId, inShouldRemoveOperatorStatus, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void RefreshInternal(SbErrorHandler inCompletionHandler)
        {
            void OnCompletionHandler(SbGroupChannel inChannel, bool inCache, SbError inError)
            {
                inCompletionHandler?.Invoke(inError);
            }

            chatMainContextRef.GroupChannelManager.GetChannel(Url, inIsInternal: false, inIsForceRefresh: true, OnCompletionHandler);
        }

        internal SbMember AddMember(MemberDto inMemberDto, long inTimestamp)
        {
            if (inMemberDto == null)
                return null;

            SbMember member = FindMember(inMemberDto.UserId);
            if (member != null)
            {
                SbMemberState oldMemberState = member.MemberState;
                member.UpdateFromDto(inMemberDto);
                if (oldMemberState == SbMemberState.Joined)
                    member.MemberState = oldMemberState;
            }
            else
            {
                member = new SbMember(inMemberDto, chatMainContextRef);
                _members.Add(member);
            }

            UpdateReadReceipt(inMemberDto.UserId, inTimestamp);
            UpdateDeliveryReceipt(inMemberDto.UserId, inTimestamp);

            _memberCount = _members.Count;
            return member;
        }

        internal void RemoveMember(string inUserId)
        {
            SbMember foundMember = _members.Find(inListMember => inListMember.UserId == inUserId);
            if (foundMember != null)
            {
                _members.Remove(foundMember);
                _memberCount = _members.Count;
            }
        }

        internal SbMember FindMember(string inUserId)
        {
            if (string.IsNullOrEmpty(inUserId))
                return null;

            return _members.Find(inListMember => inListMember.UserId == inUserId);
        }

        internal void UpdateMembers(List<UserDto> inUserDtos)
        {
            if (inUserDtos == null || inUserDtos.Count <= 0)
                return;

            foreach (UserDto userDto in inUserDtos)
            {
                UpdateMember(userDto);
            }
        }

        internal void UpdateMember(UserDto inUserDto)
        {
            if (inUserDto == null)
                return;

            SbMember foundMember = FindMember(inUserDto.UserId);
            if (foundMember != null)
            {
                foundMember.UpdateFromDto(inUserDto);
                if (chatMainContextRef.CurrentUserRef != null && foundMember.UserId == chatMainContextRef.CurrentUserId)
                {
                    chatMainContextRef.CurrentUserRef.UpdateFromDto(inUserDto);
                }
            }
        }


        internal bool UpdateMemberCount(ChannelEventDataAbstract inChannelEventData)
        {
            if (inChannelEventData == null || inChannelEventData.timestamp < _memberCountUpdatedAt)
                return false;

            if (inChannelEventData.memberCount == null || inChannelEventData.joinedMemberCount == null)
                return false;

            if (inChannelEventData.memberCount != null)
                _memberCount = inChannelEventData.memberCount.Value;

            if (inChannelEventData.joinedMemberCount != null)
                _joinedMemberCount = inChannelEventData.joinedMemberCount.Value;

            _memberCountUpdatedAt = inChannelEventData.timestamp;
            return true;
        }

        internal bool UpdateMemberCount(GroupChannelMemberCountDto inGroupChannelMemberCountDto)
        {
            if (inGroupChannelMemberCountDto == null || inGroupChannelMemberCountDto.timestamp < _memberCountUpdatedAt)
                return false;

            _memberCount = inGroupChannelMemberCountDto.memberCount;
            _joinedMemberCount = inGroupChannelMemberCountDto.joinedMemberCount;
            _memberCountUpdatedAt = inGroupChannelMemberCountDto.timestamp;
            return true;
        }

        internal void UpdateJoinedMemberCount()
        {
            _joinedMemberCount = 0;
            foreach (SbMember member in _members)
            {
                if (member.MemberState == SbMemberState.Joined)
                    _joinedMemberCount++;
            }
        }

        internal void UpdateReadReceipt(string inUserId, long inTimestamp)
        {
            if (string.IsNullOrEmpty(inUserId))
                return;

            if (_readReceiptsByUserId.TryGetValue(inUserId, out long oldTimestamp))
            {
                if (oldTimestamp < inTimestamp)
                    _readReceiptsByUserId[inUserId] = inTimestamp;
            }
            else
            {
                _readReceiptsByUserId.Add(inUserId, inTimestamp);
            }
        }

        internal long GetReadReceiptTimestamp(string inUserId)
        {
            if (string.IsNullOrEmpty(inUserId) == false && _readReceiptsByUserId.TryGetValue(inUserId, out long receipt))
                return receipt;

            return 0;
        }

        internal void RemoveReadReceipt(string inUserId)
        {
            if (string.IsNullOrEmpty(inUserId))
                return;

            _readReceiptsByUserId.RemoveIfContains(inUserId);
        }

        internal void UpdateDeliveryReceipts(Dictionary<string, long> inDeliveryReceipts)
        {
            if (inDeliveryReceipts == null || inDeliveryReceipts.Count <= 0)
                return;

            foreach (KeyValuePair<string, long> keyValuePair in inDeliveryReceipts)
            {
                UpdateDeliveryReceipt(keyValuePair.Key, keyValuePair.Value);
            }
        }

        internal void UpdateDeliveryReceipt(string inUserId, long inTimestamp)
        {
            if (string.IsNullOrEmpty(inUserId))
                return;

            if (_deliveryReceiptsByUserId.TryGetValue(inUserId, out long oldTimestamp))
            {
                if (oldTimestamp < inTimestamp)
                    _deliveryReceiptsByUserId[inUserId] = inTimestamp;
            }
            else
            {
                _deliveryReceiptsByUserId.Add(inUserId, inTimestamp);
            }
        }

        internal void RemoveDeliveryReceipt(string inUserId)
        {
            if (string.IsNullOrEmpty(inUserId))
                return;

            _deliveryReceiptsByUserId.RemoveIfContains(inUserId);
        }

        internal void SetPinnedMessageIds(List<long> inPinnedMessageIds)
        {
            _pinnedMessageIds.Clear();
            if (inPinnedMessageIds != null)
                _pinnedMessageIds.AddRange(inPinnedMessageIds);
        }

        internal bool IsMyUnreadMessageCountEnabled()
        {
            return _myCountPreference == SbCountPreference.All || _myCountPreference == SbCountPreference.UnreadMessageCountOnly;
        }

        internal bool IsMyUnreadMentionCountEnabled()
        {
            return _myCountPreference == SbCountPreference.All || _myCountPreference == SbCountPreference.UnreadMentionCountOnly;
        }

        internal void IncreaseUnreadMessageCountIfEnabled()
        {
            if (IsMyUnreadMessageCountEnabled())
            {
                _unreadMessageCount += 1;
                if (_isSuper && chatMainContextRef.MaxUnreadCountOnSuperGroup < _unreadMessageCount)
                {
                    _unreadMessageCount = chatMainContextRef.MaxUnreadCountOnSuperGroup;
                }
            }
            else
            {
                _unreadMessageCount = 0;
            }
        }

        internal void IncreaseUnreadMentionCountIfEnabled()
        {
            if (IsMyUnreadMentionCountEnabled())
            {
                _unreadMentionCount += 1;
            }
            else
            {
                _unreadMentionCount = 0;
            }
        }

        internal void ClearAllUnreadCount()
        {
            _unreadMentionCount = 0;
            _unreadMessageCount = 0;
        }

        internal bool UpdateUnreadCount(SbBaseMessage inMessage)
        {
            if (inMessage == null)
                return false;

            bool isFromCurrentUser = inMessage.Sender?.UserId == chatMainContextRef.CurrentUserId;

            if (inMessage.IsSilent && isFromCurrentUser == false)
                return false;

            bool updated = false;
            if (isFromCurrentUser == false)
            {
                IncreaseUnreadMessageCountIfEnabled();
                updated = true;
            }

            if (inMessage.IsMentionedToMe(chatMainContextRef.CurrentUserId))
            {
                IncreaseUnreadMentionCountIfEnabled();
                updated = true;
            }

            return updated;
        }


        internal bool ShouldUpdateLsatMessage(SbBaseMessage inMessage, bool inForceUpdateLastMessage)
        {
            if (inMessage == null)
                return false;

            bool isFromCurrentUser = inMessage.Sender?.UserId == chatMainContextRef.CurrentUserId;
            if (inMessage.IsSilent == false || inForceUpdateLastMessage || isFromCurrentUser)
            {
                if (_lastMessage == null || _lastMessage.CreatedAt < inMessage.CreatedAt)
                    return true;

                if (_lastMessage.CreatedAt == inMessage.CreatedAt && _lastMessage.MessageId == inMessage.MessageId && _lastMessage.UpdatedAt < inMessage.UpdatedAt)
                    return true;
            }

            return false;
        }

        internal bool ShouldDisableMack()
        {
            return _isSuper && !_isBroadcast && !_isExclusive && chatMainContextRef.AppInfo.DisableSuperGroupMack;
        }

        private SbMemberListQuery CreateMemberListQueryInternal(SbMemberListQueryParams inParams)
        {
            return new SbMemberListQuery(Url, inParams, chatMainContextRef);
        }

        private IReadOnlyList<SbMember> GetReadMembersInternal(SbBaseMessage inMessage, bool inIncludeAllMembers)
        {
            if (inMessage == null || inMessage is SbAdminMessage || _isSuper || chatMainContextRef.CurrentUserRef == null)
                return SbMember.EMPTY_MEMBER_LIST;

            string senderUserId = inMessage.Sender?.UserId;
            List<SbMember> resultMembers = new List<SbMember>();
            foreach (SbMember member in _members)
            {
                if (inIncludeAllMembers == false && (member.UserId == chatMainContextRef.CurrentUserId || member.UserId == senderUserId))
                    continue;

                if (_readReceiptsByUserId.TryGetValue(member.UserId, out long readReceipt) && inMessage.CreatedAt <= readReceipt)
                    resultMembers.Add(member);
            }

            return resultMembers;
        }

        private int GetUnreadMemberCountInternal(SbBaseMessage inMessage)
        {
            if (inMessage == null || inMessage is SbAdminMessage || _isSuper || chatMainContextRef.CurrentUserRef == null)
                return 0;

            string senderUserId = inMessage.Sender?.UserId;
            int unreadMemberCount = 0;
            foreach (SbMember member in _members)
            {
                if (member.UserId == chatMainContextRef.CurrentUserId || member.UserId == senderUserId || member.MemberState != SbMemberState.Joined)
                    continue;

                if (_readReceiptsByUserId.TryGetValue(member.UserId, out long readReceiptTimestamp) == false)
                    readReceiptTimestamp = 0;

                if (readReceiptTimestamp < inMessage.CreatedAt)
                    unreadMemberCount++;
            }

            return unreadMemberCount;
        }

        private void MarkAsReadInternal(SbErrorHandler inCompletionHandler)
        {
            void OnReadWsCommandAck(WsReceiveCommandAbstract inWsReceiveCommand, SbError inError)
            {
                if (inWsReceiveCommand is ReadWsReceiveCommand readWsReceiveCommand)
                {
                    UpdateReadReceipt(chatMainContextRef.CurrentUserId, readWsReceiveCommand.timestamp);
                }

                inCompletionHandler?.Invoke(inError);
            }

            string reqId = NetworkUtil.GenerateReqId();
            ReadWsSendCommand sendCommand = new ReadWsSendCommand(reqId, Url, OnReadWsCommandAck);
            chatMainContextRef.CommandRouter.SendWsCommand(sendCommand);
        }

        internal void SetMessageOffsetTimestamp(long inMessageOffsetTimestamp)
        {
            _messageOffsetTimestamp = inMessageOffsetTimestamp;
        }

        internal void SetMyMutedState(SbMutedState inMutedState)
        {
            _myMutedState = inMutedState;
        }
    }
}