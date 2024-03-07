// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Net;

namespace Sendbird.Chat
{
    internal sealed class DeleteAllUserMetaDataApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inUserId, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/metadata";
                resultHandler = inResultHandler;
            }
        }
    }
}