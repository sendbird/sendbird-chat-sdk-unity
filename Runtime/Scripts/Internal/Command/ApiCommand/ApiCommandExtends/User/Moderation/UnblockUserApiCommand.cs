// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UnblockUserApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inBlockerUserId, string inTargetUserId, ResultHandler inResultHandler)
            {
                Url = $"{USERS_PREFIX_URL}/{inBlockerUserId}/block/{inTargetUserId}";
                
                resultHandler = inResultHandler;
            }
        }
    }
}