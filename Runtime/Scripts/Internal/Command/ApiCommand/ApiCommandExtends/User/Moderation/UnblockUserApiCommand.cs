// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Net;

namespace Sendbird.Chat
{
    internal sealed class UnblockUserApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inBlockerUserId, string inTargetUserId, ResultHandler inResultHandler)
            {
                string encodedBlockerUserId = WebUtility.UrlEncode(inBlockerUserId);
                string encodedTargetUserId = WebUtility.UrlEncode(inTargetUserId);
                Url = $"{USERS_PREFIX_URL}/{encodedBlockerUserId}/block/{encodedTargetUserId}";
                
                resultHandler = inResultHandler;
            }
        }
    }
}