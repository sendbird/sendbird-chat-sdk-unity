// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetChannelInvitationPreferenceApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, ResultHandler inResultHandler)
            {
                Url = $"{USERS_PREFIX_URL}/{inUserId}/channel_invitation_preference";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
            }
        }
        
        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("auto_accept")] internal bool autoAccept;
        }
    }
}