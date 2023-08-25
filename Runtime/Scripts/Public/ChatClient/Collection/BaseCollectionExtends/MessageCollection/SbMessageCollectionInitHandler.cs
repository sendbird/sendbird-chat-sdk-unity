// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// An handler used in MessageCollection.initialize.
    /// </summary>
    /// @since 4.0.0
    public class SbMessageCollectionInitHandler
    {
        /// @since 4.0.0
        public delegate void ApiResultDelegate(IReadOnlyList<SbBaseMessage> inMessages, SbError inError);

        /// <summary>
        /// This will give message lists loaded from the api.
        /// </summary>
        /// @since 4.0.0
        public ApiResultDelegate OnApiResult { get; set; }
    }
}