// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GroupChannelInviteApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("user_ids")] internal List<string> userIds;
                [JsonProperty("inviter_id")] internal string inviterId;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, List<string> inUserIds, string inInviterId, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Group)}/{inChannelUrl}/invite";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    userIds = inUserIds,
                    inviterId = inInviterId
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