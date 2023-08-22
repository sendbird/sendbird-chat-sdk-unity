// 
//  Copyright (c) 2022 Sendbird, Inc.
// 


namespace Sendbird.Chat
{
    public partial class SbSender
    {
        private bool _isBlockedByMe;
        private SbRole _role;

        internal SbSender(SbUser inUser, SendbirdChatMainContext inChatMainContext, SbRole inRole) : base(inUser, inChatMainContext)
        {
            _isBlockedByMe = false;
            _role = inRole;
        }

        internal SbSender(SenderDto inSenderDto, SendbirdChatMainContext inChatMainContext) : base(inSenderDto, inChatMainContext)
        {
            if (inSenderDto == null)
            {
                Logger.Warning(Logger.CategoryType.User, "SbSender::SbSender() SenderDto is null.");
                return;
            }

            _isBlockedByMe = inSenderDto.isBlockedByMe ?? false;
            _role = string.IsNullOrEmpty(inSenderDto.role) == false ? SbRoleExtension.JsonNameToType(inSenderDto.role) : SbRole.None;
        }

        private protected override void OnUpdateFromDto(UserDto inUserDto)
        {
            if (!(inUserDto is SenderDto senderDto))
                return;

            _isBlockedByMe = senderDto.isBlockedByMe ?? _isBlockedByMe;
            _role = string.IsNullOrEmpty(senderDto.role) ? SbRoleExtension.JsonNameToType(senderDto.role) : _role;
        }
    }
}