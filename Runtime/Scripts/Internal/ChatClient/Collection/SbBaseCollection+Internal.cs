// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbBaseCollection
    {
        private protected readonly SendbirdChatMainContext chatMainContextRef = null;

        private protected SbBaseCollection(SendbirdChatMainContext inChatMainContext)
        {
            chatMainContextRef = inChatMainContext;
        }

        private protected virtual void DisposeInternal()
        {
            chatMainContextRef.CollectionManager.RemoveCollection(this);
        }
    }
}