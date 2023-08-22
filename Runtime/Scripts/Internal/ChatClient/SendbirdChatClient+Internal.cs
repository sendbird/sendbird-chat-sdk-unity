// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SendbirdChatClient : ISendbirdChatClient
    {
        private readonly SendbirdChatMain _sendbirdChatMain;

        internal SendbirdChatMain ChatMain => _sendbirdChatMain;
    }
}