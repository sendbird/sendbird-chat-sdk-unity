// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GroupChannelMemberListQueryApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inToken, int inLimit, string inChannelUrl,
                             string inNicknameStartsWithFilter,
                             SbGroupChannelOperatorFilter inOperatorFilter,
                             SbGroupChannelMutedMemberFilter inMutedMemberFilter,
                             SbMemberStateFilter inMemberStateFilter,
                             SbMemberListOrder inOrder,
                             ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Group)}/{inChannelUrl}/members";

                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty("token", inToken);
                InsertQueryParamIfNotNullOrEmpty("limit", inLimit);
                InsertQueryParamIfNotNullOrEmpty("order", inOrder.ToJsonName());
                InsertQueryParamIfNotNullOrEmpty("operator_filter", inOperatorFilter.ToJsonName());
                InsertQueryParamIfNotNullOrEmpty("muted_member_filter", inMutedMemberFilter.ToJsonName());
                InsertQueryParamIfNotNullOrEmpty("member_state_filter", inMemberStateFilter.ToJsonName());
                InsertQueryParamIfNotNullOrEmpty("nickname_startswith", inNicknameStartsWithFilter);
                InsertQueryParamIfNotNullOrEmpty("show_read_receipt", true);
                InsertQueryParamIfNotNullOrEmpty("show_delivery_receipt", true);
                InsertQueryParamIfNotNullOrEmpty("show_member_is_muted", true);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("next")] internal readonly string token;
            [JsonProperty("members")] internal readonly List<MemberDto> memberDtos;
        }
    }
}