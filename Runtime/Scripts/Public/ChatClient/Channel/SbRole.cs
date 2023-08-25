// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The role of a Member or a Sender.
    /// </summary>
    /// @since 4.0.0
    public enum SbRole
    {
        /// @since 4.0.0
        [JsonName("none")] None,
        
        /// @since 4.0.0
        [JsonName("operator")] Operator,
    }
}