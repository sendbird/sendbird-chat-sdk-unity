// 
//  Copyright (c) 2023 Sendbird, Inc.
// 


using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Class holding unread message count information of current user.
    /// </summary>
    /// @since 4.0.0
    public partial class SbUnreadMessageCount
    {
        /// <summary>
        /// Gets the subscribed total number of unread message of all GroupChannels the current user has joined.
        /// </summary>
        /// @since 4.0.0
        public int GroupChannelCount => _groupChannelCount;

        /// <summary>
        /// Gets the subscribed total number of unread message of all FeedChannels the current user has joined.
        /// </summary>
        /// @since 4.0.0
        public int FeedChannelCount => _feedChannelCount;

        /// <summary>
        /// Gets the total number of unread message with subscribed custom type.
        /// </summary>
        /// @since 4.0.0
        public int TotalCountByCustomTypes => _totalCountByCustomTypes;

        /// <summary>
        /// Gets the number of unread message with subscribed custom type.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyDictionary<string, int> CustomTypeUnreadCountMap => _customTypeUnreadCountMap;

        /// <summary>
        /// Gets unread message count for specific customType.
        /// </summary>
        /// <param name="inCustomType"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public int GetUnreadCount(string inCustomType)
        {
            return GetUnreadCountInternal(inCustomType);
        }
    }
}