// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Report category.
    /// </summary>
    /// @since 4.0.0
    public enum SbReportCategory
    {
        /// <summary>
        /// Report suspicious content
        /// </summary>
        /// @since 4.0.0
        [JsonName("suspicious")] Suspicious,

        /// <summary>
        /// Report harassing content.
        /// </summary>
        /// @since 4.0.0
        [JsonName("harassing")] Harassing,

        /// <summary>
        /// Report spam content
        /// </summary>
        /// @since 4.0.0
        [JsonName("spam")] Spam,

        /// <summary>
        /// Report inappropriate content
        /// </summary>
        /// @since 4.0.0
        [JsonName("inappropriate")] Inappropriate
    }
}