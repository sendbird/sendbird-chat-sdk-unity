// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Web;

namespace Sendbird.Chat
{
    internal sealed class GetGroupChannelApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inChannelUrl, bool inIsInternal, ResultHandler inResultHandler)
            {
                inChannelUrl = HttpUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Group, inIsInternal)}/{inChannelUrl}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty("member", true.ToString());
                InsertQueryParamIfNotNullOrEmpty("show_read_receipt", true.ToString());
                InsertQueryParamIfNotNullOrEmpty("show_delivery_receipt", true.ToString());
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