// 
//  Copyright (c) 2022 Sendbird, Inc.
// 


using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbMember : SbUser
    {
        internal static readonly IReadOnlyList<SbMember> EMPTY_MEMBER_LIST = new List<SbMember>();

        private bool _isBlockedByMe;
        private bool _isBlockingMe;
        private bool _isMuted;
        private SbRole _role;
        private SbMemberState _memberState;
        private SbRestrictionInfo _restrictionInfo;

        internal SbMember(SbMember inMember, SendbirdChatMainContext inChatMainContext) : base(inMember, inChatMainContext) { }

        internal SbMember(MemberDto inMemberDto, SendbirdChatMainContext inChatMainContext) : base(inMemberDto, inChatMainContext)
        {
            if (inMemberDto == null)
            {
                Logger.Warning(Logger.CategoryType.User, "SbMember::SbMember() MemberDto is null.");
                return;
            }

            _isBlockedByMe = inMemberDto.isBlockedByMe ?? false;
            _isBlockingMe = inMemberDto.isBlockingMe ?? false;
            _isMuted = inMemberDto.isMuted ?? false;
            _role = string.IsNullOrEmpty(inMemberDto.role) == false ? SbRoleExtension.JsonNameToType(inMemberDto.role) : SbRole.None;
            _memberState = string.IsNullOrEmpty(inMemberDto.state) == false ? SbMemberStateExtension.JsonNameToType(inMemberDto.state) : SbMemberState.None;
        }

        private protected override void OnUpdateFromDto(UserDto inUserDto)
        {
            if (!(inUserDto is MemberDto memberDto))
                return;

            _isBlockedByMe = memberDto.isBlockedByMe ?? _isBlockedByMe;
            _isBlockingMe = memberDto.isBlockingMe ?? _isBlockingMe;
            _isMuted = memberDto.isMuted ?? _isMuted;
            _role = string.IsNullOrEmpty(memberDto.role) == false ? SbRoleExtension.JsonNameToType(memberDto.role) : _role;
            _memberState = string.IsNullOrEmpty(memberDto.state) ? SbMemberStateExtension.JsonNameToType(memberDto.state) : _memberState;
        }
    }
}