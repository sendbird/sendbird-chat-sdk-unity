// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Web;

namespace Sendbird.Chat
{
    internal sealed class DeleteMessageApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inChannelUrl, SbChannelType inChannelType, long inMessageId, ResultHandler inResultHandler)
            {
                inChannelUrl = HttpUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/messages/{inMessageId}";
                resultHandler = inResultHandler;
            }
        }
    }
}