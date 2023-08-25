// 
//  Copyright (c) 2022 Sendbird, Inc.
// 


namespace Sendbird.Chat
{
    public partial class SbRestrictionInfo
    {
        private readonly string _description;
        private readonly long _endAt;
        private readonly long _remainingDuration;
        private readonly SbRestrictionType _restrictionType;

        internal SbRestrictionInfo(RestrictedUserDto inRestrictionUserDto)
        {
            if (inRestrictionUserDto != null)
            {
                _description = inRestrictionUserDto.Description;
                _endAt = inRestrictionUserDto.EndAt;
                _restrictionType = inRestrictionUserDto.RestrictionType;
                _remainingDuration = inRestrictionUserDto.RemainingDuration;
            }
        }
    }
}