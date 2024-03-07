// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UnblockUserApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inBlockerUserId, string inTargetUserId, ResultHandler inResultHandler)
            {
                inBlockerUserId = WebUtility.UrlEncode(inBlockerUserId);
                inTargetUserId = WebUtility.UrlEncode(inTargetUserId);
                Url = $"{USERS_PREFIX_URL}/{inBlockerUserId}/block/{inTargetUserId}";
                
                resultHandler = inResultHandler;
            }
        }
    }
}