// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal interface IGroupChannelComparable
    {
        string GetChannelUrl();
        string GetName();
        long GetCreatedAt();
        SbBaseMessage GetLastMessage();
    }
}