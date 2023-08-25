// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class SendFileMessageWsSendCommand : SendMessageWsSendCommandAbstract
    {
        [JsonProperty("url")] private readonly string _fileUrl;
        [JsonProperty("name")] private readonly string _fileName;
        [JsonProperty("type")] private readonly string _mimeType;
        [JsonProperty("size")] private readonly int? _fileSize;
        [JsonProperty("thumbnails")] private readonly List<ThumbnailDto> _thumbnails;
        [JsonProperty("require_auth")] private readonly bool _requireAuth;

        internal SendFileMessageWsSendCommand(string inReqId, string inChannelUrl, bool inRequireAuth, List<ThumbnailDto> inThumbnailDtos, 
                                          SbFileMessageCreateParams inFileMessageCreateParams, AckHandler inAckHandler)
            : base(WsCommandType.FileMessage, inReqId, inChannelUrl, inFileMessageCreateParams, inAckHandler)
        {
            _fileUrl = inFileMessageCreateParams.FileUrl;
            _fileName = inFileMessageCreateParams.FileName;
            _mimeType = inFileMessageCreateParams.MimeType;
            _requireAuth = inRequireAuth;
            _thumbnails = inThumbnailDtos;

            if( 0 < inFileMessageCreateParams.FileSize)
                _fileSize = inFileMessageCreateParams.FileSize;
        }
    }
}