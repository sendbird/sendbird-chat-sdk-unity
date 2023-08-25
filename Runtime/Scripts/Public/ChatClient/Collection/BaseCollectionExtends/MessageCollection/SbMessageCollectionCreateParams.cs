// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// A params to create the SbMessageCollection.
    /// </summary>
    /// @since 4.0.0
    public class SbMessageCollectionCreateParams
    {
        /// <summary>
        /// The MessageListParams to be used in the SbMessageCollection.
        /// </summary>
        /// @since 4.0.0
        public SbMessageListParams MessageListParams { get; set; }

        /// <summary>
        /// The starting point of which to load messages from.
        /// </summary>
        /// @since 4.0.0
        public long StartingPoint { get; set; }

        /// <summary>
        /// The message collection handler to be used for this SbMessageCollection.
        /// </summary>
        /// @since 4.0.0
        public SbMessageCollectionHandler MessageCollectionHandler { get; set; }
        
        /// @since 4.0.0
        public SbMessageCollectionCreateParams(SbMessageListParams inMessageListParams, long inStartingPoint = long.MaxValue, SbMessageCollectionHandler inMessageCollectionHandler = null)
        {
            MessageListParams = inMessageListParams ?? new SbMessageListParams();
            StartingPoint = inStartingPoint;
            MessageCollectionHandler = inMessageCollectionHandler;
        }
    }
}