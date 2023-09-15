// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Web;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UnblockUserApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inBlockerUserId, string inTargetUserId, ResultHandler inResultHandler)
            {
                inBlockerUserId = HttpUtility.UrlEncode(inBlockerUserId);
                inTargetUserId = HttpUtility.UrlEncode(inTargetUserId);
                Url = $"{USERS_PREFIX_URL}/{inBlockerUserId}/block/{inTargetUserId}";
                
                resultHandler = inResultHandler;
            }
        }
    }
}