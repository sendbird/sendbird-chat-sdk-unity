// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Web;

namespace Sendbird.Chat
{
    internal sealed class UnbanUserApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inChannelUrl, SbChannelType inChannelType, string inUserId, ResultHandler inResultHandler)
            {
                inChannelUrl = HttpUtility.UrlEncode(inChannelUrl);
                inUserId = HttpUtility.UrlEncode(inUserId);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/ban/{inUserId}";
                resultHandler = inResultHandler;
            }
        }
    }
}