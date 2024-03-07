// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Net;

namespace Sendbird.Chat
{
    internal sealed class UnpinMessageApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inChannelUrl, SbChannelType inChannelType, long inMessageId, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/messages/{inMessageId}/pin";
                resultHandler = inResultHandler;
            }
        }
    }
}