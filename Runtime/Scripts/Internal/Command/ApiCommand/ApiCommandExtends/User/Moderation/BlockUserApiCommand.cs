// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class BlockUserApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("target_id")] internal string targetUserId;
#pragma warning restore CS0649
            }

            internal Request(string inBlockerUserId, string inTargetUserId, ResultHandler inResultHandler)
            {
                string encodedBlockerUserId = WebUtility.UrlEncode(inBlockerUserId);
                Url = $"{USERS_PREFIX_URL}/{encodedBlockerUserId}/block";

                resultHandler = inResultHandler;
                ResponseType = typeof(Response);

                Payload tempPayload = new Payload
                {
                    targetUserId = inTargetUserId
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }
        
        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal UserDto UserDto { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                UserDto = UserDto.DeserializeFromJson(inJsonString);
            }
        }
    }
}