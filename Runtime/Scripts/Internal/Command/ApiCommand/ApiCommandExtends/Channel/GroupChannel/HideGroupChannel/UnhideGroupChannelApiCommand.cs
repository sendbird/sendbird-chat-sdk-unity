// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal sealed class UnhideGroupChannelApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inChannelUrl, ResultHandler inResultHandler)
            {
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Group)}/{inChannelUrl}/hide";
                resultHandler = inResultHandler;
            }
        }
    }
}