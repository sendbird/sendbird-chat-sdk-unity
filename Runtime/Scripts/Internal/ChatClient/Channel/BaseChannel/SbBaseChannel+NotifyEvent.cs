// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbBaseChannel
    {
        private void NotifyMessageSent(SbBaseMessage inMessage)
        {
            if (inMessage != null)
            {
                chatMainContextRef.CollectionManager.OnMessageSent(this, inMessage);
            }
        }

        private void NotifyMessagePending(SbBaseMessage inMessage)
        {
            if (inMessage != null)
            {
                chatMainContextRef.CollectionManager.OnMessagePending(this, inMessage);
            }
        }

        private void NotifyMessageFailed(SbBaseMessage inMessage)
        {
            if (inMessage != null)
            {
                chatMainContextRef.CollectionManager.OnMessageFailed(this, inMessage);
            }
        }

        private void NotifyMessageCanceled(SbBaseMessage inMessage)
        {
            if (inMessage != null)
            {
                chatMainContextRef.CollectionManager.OnMessageCanceled(this, inMessage);
            }
        }
    }
}