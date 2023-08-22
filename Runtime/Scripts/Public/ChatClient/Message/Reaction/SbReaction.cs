// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Objects representing a reaction.
    /// </summary>
    /// @since 4.0.0
    public partial class SbReaction
    {
        /// <summary>
        /// The key of the reaction.
        /// </summary>
        /// @since 4.0.0
        public string Key => _key;

        /// <summary>
        /// The updated time of the reaction in milliseconds.
        /// </summary>
        /// @since 4.0.0
        public long UpdatedAt => _updatedAt;

        /// <summary>
        /// User ids.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> UserIds => _userIds;
    }
}