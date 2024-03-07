// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;

namespace Sendbird.Chat
{
    internal sealed class GetMessageApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(SbMessageRetrievalParams inParams, ResultHandler inResultHandler)
            {
                if (inParams == null)
                {
                    inResultHandler?.Invoke(null, SbErrorCodeExtension.INVALID_PARAMETER_ERROR, false);
                    return;
                }

                inParams.ChannelUrl = WebUtility.UrlEncode(inParams.ChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inParams.ChannelType)}/{inParams.ChannelUrl}/messages/{inParams.MessageId}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty("with_sorted_meta_array", inParams.IncludeMetaArray);
                InsertQueryParamIfNotNullOrEmpty("include_reactions", inParams.IncludeReactions);
                InsertQueryParamIfNotNullOrEmpty("include_thread_info", inParams.IncludeThreadInfo);
                InsertQueryParamIfNotNullOrEmpty("include_parent_message_info", inParams.IncludeParentMessageInfo);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal BaseMessageDto BaseMessageDto { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                BaseMessageDto = BaseMessageDto.JsonStringToMessageDto(inJsonString);
            }
        }
    }
}