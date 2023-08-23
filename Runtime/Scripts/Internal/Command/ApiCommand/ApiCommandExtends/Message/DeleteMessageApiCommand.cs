// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal sealed class DeleteMessageApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inChannelUrl, SbChannelType inChannelType, long inMessageId, ResultHandler inResultHandler)
            {
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/messages/{inMessageId}";
                resultHandler = inResultHandler;
            }
        }
    }
}