// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

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

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal string token;
            internal List<MemberDto> memberDtos;

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (string.IsNullOrEmpty(inJsonString))
                    return;

                using (JsonTextReader reader = JsonStreamingPool.CreateReader(inJsonString))
                {
                    reader.Read();
                    if (reader.TokenType != JsonToken.StartObject)
                        return;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.EndObject)
                            break;

                        string propName = reader.Value as string;
                        reader.Read();
                        switch (propName)
                        {
                            case "next": token = JsonStreamingHelper.ReadString(reader); break;
                            case "members": memberDtos = MemberDto.ReadListFromJson(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }
        }
    }
}