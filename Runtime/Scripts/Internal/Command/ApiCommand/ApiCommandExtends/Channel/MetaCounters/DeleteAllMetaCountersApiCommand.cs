// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Net;

namespace Sendbird.Chat
{
    internal sealed class DeleteAllMetaCountersApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inChannelUrl, SbChannelType inChannelType, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/metacounter";
                resultHandler = inResultHandler;
            }
        }
    }
}