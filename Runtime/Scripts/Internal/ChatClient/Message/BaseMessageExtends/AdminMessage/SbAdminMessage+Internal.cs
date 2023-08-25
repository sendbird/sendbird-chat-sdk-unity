// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbAdminMessage
    {
        internal SbAdminMessage(AdminMessageDto inFileMessageDto, SendbirdChatMainContext inChatMainContext)
            : base(inFileMessageDto, inChatMainContext) { }
    }
}