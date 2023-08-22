// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal sealed class GetOpenChannelApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inChannelUrl, bool inIsInternal, ResultHandler inResultHandler)
            {
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Open, inIsInternal)}/{inChannelUrl}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal OpenChannelDto OpenChannelDto { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                OpenChannelDto = OpenChannelDto.DeserializeFromJson(inJsonString);
            }
        }
    }
}