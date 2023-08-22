// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Filter operators in group channels.
    /// </summary>
    /// @since 4.0.0
    public enum SbGroupChannelOperatorFilter
    {
        /// @since 4.0.0
        [JsonName("all")] All,

        /// @since 4.0.0
        [JsonName("operator")] Operator,

        /// @since 4.0.0
        [JsonName("nonoperator")] NonOperator
    }
}