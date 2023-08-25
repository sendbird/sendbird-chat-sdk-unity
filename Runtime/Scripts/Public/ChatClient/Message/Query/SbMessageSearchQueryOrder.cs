// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// The order in which the query result will be based on.
    /// </summary>
    /// @since 4.0.0
    public enum SbMessageSearchQueryOrder
    {
        /// <summary>
        /// Score query returns the result as by their matching score.
        /// </summary>
        /// @since 4.0.0
        [JsonName("score")] Score = 0,

        /// <summary>
        /// Timestamp query returns the result as by SbBaseMessage's timestamp.
        /// </summary>
        /// @since 4.0.0
        [JsonName("ts")] Timestamp
    }
}