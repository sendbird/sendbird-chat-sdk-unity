// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Web;

namespace Sendbird.Chat
{
    internal sealed class DeleteMetaCountersApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inChannelUrl, SbChannelType inChannelType, string inKey, ResultHandler inResultHandler)
            {
                inChannelUrl = HttpUtility.UrlEncode(inChannelUrl);
                inKey = HttpUtility.UrlEncode(inKey);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/metacounter/{inKey}";
                resultHandler = inResultHandler;
            }
        }
    }
}