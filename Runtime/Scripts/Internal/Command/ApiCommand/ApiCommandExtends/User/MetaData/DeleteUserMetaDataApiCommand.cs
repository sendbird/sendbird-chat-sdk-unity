// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal sealed class DeleteUserMetaDataApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inUserId, string inKey, ResultHandler inResultHandler)
            {
                Url = $"{USERS_PREFIX_URL}/{inUserId}/metadata/{inKey}";
                resultHandler = inResultHandler;
            }
        }
    }
}