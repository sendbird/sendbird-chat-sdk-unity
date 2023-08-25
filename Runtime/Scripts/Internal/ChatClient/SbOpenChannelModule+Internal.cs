// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbOpenChannelModule
    {
        private readonly SendbirdChatMain _chatMainRef;
        
        internal SbOpenChannelModule(SendbirdChatMain inChatMain)
        {
            _chatMainRef = inChatMain;
        }

        private void CreateChannelInternal(SbOpenChannelCreateParams inChannelCreateParams, SbOpenChannelCallbackHandler inCompletionHandler)
        {
            _chatMainRef.ChatMainContext.OpenChannelManager.CreateChannel(inChannelCreateParams, inCompletionHandler);
        }

        private void GetChannelInternal(string inChannelUrl, SbGetOpenChannelHandler inCompletionHandler)
        {
            _chatMainRef.ChatMainContext.OpenChannelManager.GetChannel(inChannelUrl, false, false, inCompletionHandler);
        }
        
        private void AddOpenChannelHandlerInternal(string inIdentifier, SbOpenChannelHandler inChannelHandler)
        {
            _chatMainRef.ChatMainContext.OpenChannelManager.AddChannelHandler(inIdentifier, inChannelHandler);
        }

        private void RemoveOpenChannelHandlerInternal(string inIdentifier)
        {
            _chatMainRef.ChatMainContext.OpenChannelManager.RemoveChannelHandlerIfContains(inIdentifier);
        }

        private void RemoveAllOpenChannelHandlersInternal()
        {
            _chatMainRef.ChatMainContext.OpenChannelManager.RemoveAllChannelHandlers();
        }

        private SbOpenChannelListQuery CreateOpenChannelListQueryInternal(SbOpenChannelListQueryParams inOpenChannelListQueryParams)
        {
            return new SbOpenChannelListQuery(inOpenChannelListQueryParams, _chatMainRef.ChatMainContext);
        }
    }
}