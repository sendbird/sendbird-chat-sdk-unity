// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class JoinGroupChannelApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PutRequest
        {

            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("user_id")] internal string userId;
                [JsonProperty("access_code")] internal string accessCode;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, string inUserId, string inAccessCode, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Group)}/{inChannelUrl}/join";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    userId = inUserId,
                    accessCode = inAccessCode
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal GroupChannelDto GroupChannelDto { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                GroupChannelDto = GroupChannelDto.DeserializeFromJson(inJsonString);
            }
        }
    }
}