// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a thread info of a message.
    /// </summary>
    /// @since 4.0.0
    public partial class SbThreadInfo
    {
        /// <summary>
        /// The total number of replies in a specific thread. A value of 0 indicates there is no reply in the thread.
        /// </summary>
        /// @since 4.0.0
        public int ReplyCount => _replyCount;

        /// <summary>
        /// The time that the last reply was created, in Unix milliseconds format. A value of 0 indicates there is no reply in the thread.
        /// </summary>
        /// @since 4.0.0
        public long LastRepliedAt => _lastRepliedAt;

        /// <summary>
        /// Users who left a reply in the thread, based on the time the reply was added.
        /// </summary>
        /// @since 4.0.0
        public List<SbUser> MostRepliedUsers => _mostRepliedUsers;
    }
}