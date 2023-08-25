// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents an detailed information of a SbRestrictedUser or SbMember's restriction.
    /// </summary>
    /// @since 4.0.0
    public partial class SbRestrictionInfo
    {
        /// <summary>
        /// Description of the restriction.
        /// </summary>
        /// @since 4.0.0
        public string Description => _description;

        /// <summary>
        /// End time of the restriction.
        /// </summary>
        /// @since 4.0.0
        public long EndAt => _endAt;

        /// <summary>
        /// The remaining duration in ms.
        /// </summary>
        /// @since 4.0.0
        public long RemainingDuration => _remainingDuration;

        /// <summary>
        /// SbRestrictionType of the current user.
        /// </summary>
        /// @since 4.0.0
        public SbRestrictionType RestrictionType => _restrictionType;
    }
}