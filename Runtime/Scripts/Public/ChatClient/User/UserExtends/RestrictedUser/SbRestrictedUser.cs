// 
//  Copyright (c) 2022 Sendbird, Inc.
// 


namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a user who has been restricted in a channel
    /// </summary>
    /// @since 4.0.0
    public partial class SbRestrictedUser : SbUser
    {
        /// <summary>
        /// Restriction info for this user
        /// </summary>
        /// @since 4.0.0
        public SbRestrictionInfo RestrictionInfo => _restrictionInfo;
    }
}