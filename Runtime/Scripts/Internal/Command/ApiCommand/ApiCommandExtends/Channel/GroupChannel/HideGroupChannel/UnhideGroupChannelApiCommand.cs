// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Net;

namespace Sendbird.Chat
{
    internal sealed class UnhideGroupChannelApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inChannelUrl, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Group)}/{inChannelUrl}/hide";
                resultHandler = inResultHandler;
            }
        }
    }
}