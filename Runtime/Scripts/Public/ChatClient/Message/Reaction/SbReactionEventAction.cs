// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The ReactionEvent action state.
    /// </summary>
    /// @since 4.0.0
    public enum SbReactionEventAction
    {
        /// @since 4.0.0
        [JsonName("delete")] Delete,
        
        /// @since 4.0.0
        [JsonName("add")] Add,
    }
}