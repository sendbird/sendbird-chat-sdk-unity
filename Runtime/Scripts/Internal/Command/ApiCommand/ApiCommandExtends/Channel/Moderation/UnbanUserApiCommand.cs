// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Net;

namespace Sendbird.Chat
{
    internal sealed class UnbanUserApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inChannelUrl, SbChannelType inChannelType, string inUserId, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/ban/{inUserId}";
                resultHandler = inResultHandler;
            }
        }
    }
}