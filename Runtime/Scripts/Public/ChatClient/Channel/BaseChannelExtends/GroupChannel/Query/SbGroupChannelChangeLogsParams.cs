// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a group channel change logs params.
    /// </summary>
    /// @since 4.0.0
    public class SbGroupChannelChangeLogsParams
    {
        /// <summary>
        /// GroupChannel custom types filter. If this is null, the changelogs of all channels will be returned. Defaults to null.
        /// </summary>
        /// @since 4.0.0
        public List<string> CustomTypes { get; set; } = null;

        /// <summary>
        /// to include empty channels or not (channels without messages). 
        /// </summary>
        /// @since 4.0.0
        public bool IncludeEmpty { get; set; } = true;

        /// <summary>
        /// to include frozen channels or not. 
        /// </summary>
        /// @since 4.0.0
        public bool IncludeFrozen { get; set; } = true;
    }
}