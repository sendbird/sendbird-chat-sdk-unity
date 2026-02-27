//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class SendFileMessageApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            private class Payload : SendMessageApiCommandAbstract.Payload
            {
                internal string fileUrl;
                internal string fileName;
                internal string mimeType;
                internal int? fileSize;
                internal List<ThumbnailDto> thumbnails;
                internal bool requireAuth;

                internal Payload(string inChannelUrl, string inRequestId, string inUserId, SbFileMessageCreateParams inParams)
                    : base(WsCommandType.FileMessage.ToJsonName(), inRequestId, inChannelUrl, inUserId, inParams) { }

                protected override void WriteFields(JsonTextWriter inWriter)
                {
                    base.WriteFields(inWriter);
                    JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "url", fileUrl);
                    JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "name", fileName);
                    JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "type", mimeType);
                    JsonStreamingHelper.WriteNullableInt(inWriter, "size", fileSize);
                    if (thumbnails != null)
                    {
                        inWriter.WritePropertyName("thumbnails");
                        inWriter.WriteStartArray();
                        foreach (ThumbnailDto thumbnailDto in thumbnails)
                        {
                            thumbnailDto.WriteToJson(inWriter);
                        }
                        inWriter.WriteEndArray();
                    }
                    JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "require_auth", requireAuth);
                }
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

                ContentBody = payload.ToJsonString();
            }
        }

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
