// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents messages type filter to be used when messages list are read. User for SbUserMessage, File for SbFileMessage and Admin for SbAdminMessage.
    /// </summary>
    /// @since 4.0.0
    public enum SbMessageTypeFilter
    {
        /// @since 4.0.0
        [JsonName("")] All,

        /// @since 4.0.0
        [JsonName("MESG")] User,

        /// @since 4.0.0
        [JsonName("FILE")] File,

        /// @since 4.0.0
        [JsonName("ADMM")] Admin,
    }
}