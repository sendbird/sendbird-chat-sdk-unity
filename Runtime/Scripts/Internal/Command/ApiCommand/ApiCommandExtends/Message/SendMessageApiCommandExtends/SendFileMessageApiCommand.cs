// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class SendFileMessageApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            [Serializable]
            private class Payload : SendMessageApiCommandAbstract.Payload
            {
#pragma warning disable CS0649
                [JsonProperty("url")] internal string fileUrl;
                [JsonProperty("name")] internal string fileName;
                [JsonProperty("type")] internal string mimeType;
                [JsonProperty("size")] internal int? fileSize;
                [JsonProperty("thumbnails")] internal List<ThumbnailDto> thumbnails;
                [JsonProperty("require_auth")] internal bool requireAuth;
#pragma warning restore CS0649
                internal Payload(string inChannelUrl, string inRequestId, string inUserId, SbFileMessageCreateParams inParams)
                    : base(WsCommandType.FileMessage.ToJsonName(), inRequestId, inChannelUrl, inUserId, inParams) { }
            }

            internal Request(string inRequestId, string inChannelUrl, SbChannelType inChannelType, string inUserId, SbFileMessageCreateParams inParams, bool inRequireAuth, List<ThumbnailDto> inThumbnailDtos, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/messages";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                Payload payload = new Payload(inChannelUrl, inRequestId, inUserId, inParams);
                {
                    if (inParams != null)
                    {
                        payload.fileUrl = inParams.FileUrl;
                        payload.fileName = inParams.FileName;
                        payload.fileSize = inParams.FileSize;
                        payload.mimeType = inParams.MimeType;
                        payload.requireAuth = inRequireAuth;
                        payload.thumbnails = inThumbnailDtos;
                    }
                }

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(payload);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal FileMessageDto FileMessageDtoDto { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                FileMessageDtoDto = FileMessageDto.DeserializeFromJson(inJsonString);
            }
        }
    }
}