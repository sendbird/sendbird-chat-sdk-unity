// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbGroupChannel
    {
        private SbMessageCollection CreateMessageCollectionInternal(SbMessageCollectionCreateParams inParams)
        {
            return chatMainContextRef.CollectionManager.CreateMessageCollection(this, inParams);
        }
    }
}