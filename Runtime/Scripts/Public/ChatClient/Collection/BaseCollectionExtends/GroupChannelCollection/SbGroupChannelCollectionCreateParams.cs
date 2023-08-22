// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// A params to create the GroupChannelCollection object.
    /// </summary>
    /// @since 4.0.0
    public class SbGroupChannelCollectionCreateParams
    {
        /// @since 4.0.0
        public SbGroupChannelListQuery Query { get; set; }

        /// @since 4.0.0
        public SbGroupChannelCollectionHandler GroupChannelCollectionHandler { get; set; }

        /// @since 4.0.0
        public SbGroupChannelCollectionCreateParams(SbGroupChannelListQuery inGroupChannelListQuery, SbGroupChannelCollectionHandler inGroupChannelCollectionHandler = null)
        {
            Query = inGroupChannelListQuery;
            GroupChannelCollectionHandler = inGroupChannelCollectionHandler;
        }
    }
}