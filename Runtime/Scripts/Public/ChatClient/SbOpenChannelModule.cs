// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// @since 4.0.0
    public partial class SbOpenChannelModule
    {
        /// <summary>
        /// Creates a open channel with SbOpenChannelCreateParams class.
        /// </summary>
        /// <param name="inChannelCreateParams">Instance of SbOpenChannelCreateParams what has properties to create open channel.</param>
        /// <param name="inCompletionHandler">Handler block to execute. channel is the open channel instance.</param>
        /// @since 4.0.0
        public void CreateChannel(SbOpenChannelCreateParams inChannelCreateParams, SbOpenChannelCallbackHandler inCompletionHandler)
        {
            CreateChannelInternal(inChannelCreateParams, inCompletionHandler);
        }

        /// <summary>
        /// Gets an open channel instance from channel URL.
        /// </summary>
        /// <param name="inChannelUrl">The channel URL.</param>
        /// <param name="inCompletionHandler">The handler block to execute. channel is the open channel instance which has the channelURL.</param>
        /// @since 4.0.0
        public void GetChannel(string inChannelUrl, SbGetOpenChannelHandler inCompletionHandler)
        {
            GetChannelInternal(inChannelUrl, inCompletionHandler);
        }

        /// <summary>
        /// Creates a query instance for open channel list.
        /// </summary>
        /// <param name="inOpenChannelListQueryParams">The params object to change query condition.</param>
        /// <returns></returns>
        /// @since 4.0.0
        public SbOpenChannelListQuery CreateOpenChannelListQuery(SbOpenChannelListQueryParams inOpenChannelListQueryParams)
        {
            return CreateOpenChannelListQueryInternal(inOpenChannelListQueryParams);
        }

        /// <summary>
        /// Adds a channel handler. All added handlers will be notified when events occur.
        /// </summary>
        /// <param name="inIdentifier">The identifier for handler.</param>
        /// <param name="inChannelHandler"></param>
        /// @since 4.0.0
        public void AddOpenChannelHandler(string inIdentifier, SbOpenChannelHandler inChannelHandler)
        {
            AddOpenChannelHandlerInternal(inIdentifier, inChannelHandler);
        }

        /// <summary>
        /// Removes a channel handler. The deleted handler no longer be notified.
        /// </summary>
        /// <param name="inIdentifier"></param>
        /// @since 4.0.0
        public void RemoveOpenChannelHandler(string inIdentifier)
        {
            RemoveOpenChannelHandlerInternal(inIdentifier);
        }

        /// <summary>
        /// Removes all channel handlers added by AddOpenChannelHandler.
        /// </summary>
        /// @since 4.0.0
        public void RemoveAllOpenChannelHandlers()
        {
            RemoveAllOpenChannelHandlersInternal();
        }
    }
}