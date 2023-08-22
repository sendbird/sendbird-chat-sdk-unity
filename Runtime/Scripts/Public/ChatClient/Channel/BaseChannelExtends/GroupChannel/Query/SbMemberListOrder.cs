// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The order type for member list query.
    /// </summary>
    /// @since 4.0.0
    public enum SbMemberListOrder
    {
        /// @since 4.0.0
        [JsonName("member_nickname_alphabetical")] NicknameAlphabetical,
        
        /// @since 4.0.0
        [JsonName("operator_then_member_alphabetical")] OperatorThenMemberNicknameAlphabetical
    }
}