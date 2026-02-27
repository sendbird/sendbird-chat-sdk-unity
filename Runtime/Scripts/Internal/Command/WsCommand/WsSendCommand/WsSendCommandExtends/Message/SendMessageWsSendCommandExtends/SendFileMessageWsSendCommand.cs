//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class SendFileMessageWsSendCommand : SendMessageWsSendCommandAbstract
    {
        private readonly string _fileUrl;
        private readonly string _fileName;
        private readonly string _mimeType;
        private readonly int? _fileSize;
        private readonly List<ThumbnailDto> _thumbnails;
        private readonly bool _requireAuth;

        internal SendFileMessageWsSendCommand(string inReqId, string inChannelUrl, bool inRequireAuth, List<ThumbnailDto> inThumbnailDtos,
                                          SbFileMessageCreateParams inFileMessageCreateParams, AckHandler inAckHandler)
            : base(WsCommandType.FileMessage, inReqId, inChannelUrl, inFileMessageCreateParams, inAckHandler)
        {
            _fileUrl = inFileMessageCreateParams.FileUrl;
            _fileName = inFileMessageCreateParams.FileName;
            _mimeType = inFileMessageCreateParams.MimeType;
            _requireAuth = inRequireAuth;
            _thumbnails = inThumbnailDtos;

            if (0 < inFileMessageCreateParams.FileSize)
                _fileSize = inFileMessageCreateParams.FileSize;
        }

        protected override void WriteFields(JsonTextWriter inWriter)
        {
            base.WriteFields(inWriter);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "url", _fileUrl);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "name", _fileName);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "type", _mimeType);
            JsonStreamingHelper.WriteNullableInt(inWriter, "size", _fileSize);
            if (_thumbnails != null)
            {
                inWriter.WritePropertyName("thumbnails");
                inWriter.WriteStartArray();
                foreach (ThumbnailDto thumbnailDto in _thumbnails)
                {
                    thumbnailDto.WriteToJson(inWriter);
                }
                inWriter.WriteEndArray();
            }
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "require_auth", _requireAuth);
        }
    }
}
