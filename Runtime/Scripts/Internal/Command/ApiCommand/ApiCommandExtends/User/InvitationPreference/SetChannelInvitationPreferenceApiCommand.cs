// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class SetChannelInvitationPreferenceApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PutRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("auto_accept")] internal bool autoAccept;
#pragma warning restore CS0649
            }

            internal Request(string inUserId, bool inAutoAccept, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/channel_invitation_preference";
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    autoAccept = inAutoAccept
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }
    }
}