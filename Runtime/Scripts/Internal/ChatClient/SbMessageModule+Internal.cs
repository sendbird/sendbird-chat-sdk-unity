// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbMessageModule
    {
        private readonly SendbirdChatMain _chatMainRef;

        internal SbMessageModule(SendbirdChatMain inChatMain)
        {
            _chatMainRef = inChatMain;
        }
    }
}