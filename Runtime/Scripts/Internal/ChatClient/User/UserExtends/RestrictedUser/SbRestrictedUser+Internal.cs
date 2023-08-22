// 
//  Copyright (c) 2022 Sendbird, Inc.
// 


namespace Sendbird.Chat
{
    public partial class SbRestrictedUser
    {
        private readonly SbRestrictionInfo _restrictionInfo;

        internal SbRestrictedUser(RestrictedUserDto inRestrictedUserDto, SendbirdChatMainContext inChatMainContext) : base(inRestrictedUserDto, inChatMainContext)
        {
            if (inRestrictedUserDto == null)
            {
                Logger.Warning(Logger.CategoryType.User, "SbRestrictedUser::SbRestrictedUser() RestrictedUserDto is null.");
                return;
            }

            _restrictionInfo = new SbRestrictionInfo(inRestrictedUserDto);
        }
    }
}