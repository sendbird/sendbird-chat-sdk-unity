// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// An object contains a set of options to get total unread message count from a group channel
    /// </summary>
    /// @since 4.0.0
    public class SbGroupChannelTotalUnreadMessageCountParams
    {
        /// <summary>
        /// The array filter of channel custom types.
        /// </summary>
        /// @since 4.0.0
        public List<string> ChannelCustomTypesFilter { get; set; }

        /// <summary>
        /// The enumerator filter of super channel.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelSuperChannelFilter SuperChannelFilter { get; set; } = SbGroupChannelSuperChannelFilter.All;
    }
}