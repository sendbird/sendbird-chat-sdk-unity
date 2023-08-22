// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal sealed class FileMessageDto : BaseMessageDto
    {
        [Serializable]
        internal sealed class FileDto
        {
            [JsonProperty("url")] internal readonly string fileUrl = null;
            [JsonProperty("name")] internal readonly string fileName = null;
            [JsonProperty("type")] internal readonly string mimeType = null;
            [JsonProperty("size")] internal readonly int fileSize = 0;
            [JsonProperty("require_auth")] internal readonly bool requireAuth = false;
            [JsonProperty("data")] internal readonly string data = null;
            [JsonProperty("custom")] internal readonly string custom = null;
        }

        [JsonProperty("url")] private readonly string _fileUrl = null;
        [JsonProperty("name")] private readonly string _fileName = null;
        [JsonProperty("type")] private readonly string _mimeType = null;
        [JsonProperty("size")] private readonly int _fileSize = 0;
        [JsonProperty("require_auth")] private readonly bool _requireAuth = false;
        [JsonProperty("thumbnails")] internal readonly List<ThumbnailDto> thumbnailDtos = null;
        [JsonProperty("file")] private readonly FileDto _file = null;

        internal string FileUrl { get; private set; } = null;
        internal string FileName { get; private set; } = null;
        internal string MimeType { get; private set; } = null;
        internal int FileSize { get; private set; } = 0;
        internal bool RequireAuth { get; private set; } = false;
        
        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (_file != null)
            {
                FileUrl = _file.fileUrl;
                FileName = _file.fileName;
                MimeType = _file.mimeType;
                FileSize = _file.fileSize;
                RequireAuth = _file.requireAuth;
                base.Data = string.IsNullOrEmpty(_file.data) == false ? _file.data : _file.custom;
            }
            else
            {
                FileUrl = _fileUrl;
                FileName = _fileName;
                MimeType = _mimeType;
                FileSize = _fileSize;
                RequireAuth = _requireAuth;
            }
        }

        internal override SbBaseMessage CreateMessageInstance(SendbirdChatMainContext inChatMainContext)
        {
            return new SbFileMessage(this, inChatMainContext);
        }

        internal static FileMessageDto DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<FileMessageDto>(inJsonString);
        }

        internal static FileMessageDto DeserializeFromJson(JObject inJObject)
        {
            return inJObject.ToObjectIgnoreException<FileMessageDto>();
        }
    }
}