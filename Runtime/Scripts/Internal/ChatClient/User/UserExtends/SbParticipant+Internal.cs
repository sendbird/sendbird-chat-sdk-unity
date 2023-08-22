// 
//  Copyright (c) 2022 Sendbird, Inc.
// 


namespace Sendbird.Chat
{
    public partial class SbParticipant
    {
        private bool _isMuted;

        internal SbParticipant(SbUser inUser, SendbirdChatMainContext inChatMainContext) : base(inUser, inChatMainContext) { }

        internal SbParticipant(ParticipantDto inParticipantDto, SendbirdChatMainContext inChatMainContext) : base(inParticipantDto, inChatMainContext)
        {
            if (inParticipantDto == null)
            {
                Logger.Warning(Logger.CategoryType.User, "SbParticipant::SbParticipant() ParticipantDto is null.");
                return;
            }

            _isMuted = inParticipantDto.isMuted ?? false;
        }

        private protected override void OnUpdateFromDto(UserDto inUserDto)
        {
            if (!(inUserDto is ParticipantDto participantDto))
                return;

            _isMuted = participantDto.isMuted ?? _isMuted;
        }
    }
}