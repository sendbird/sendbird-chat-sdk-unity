//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class FileMessageDto : BaseMessageDto
    {
        internal sealed class FileDto
        {
            internal string fileUrl;
            internal string fileName;
            internal string mimeType;
            internal int fileSize;
            internal bool requireAuth;
            internal string data;
            internal string custom;

            internal static FileDto ReadFromJson(JsonTextReader inReader)
            {
                if (inReader.TokenType == JsonToken.Null)
                    return null;

                if (inReader.TokenType != JsonToken.StartObject)
                    return null;

                FileDto dto = new FileDto();
                while (inReader.Read())
                {
                    if (inReader.TokenType == JsonToken.EndObject)
                        break;

                    string propName = inReader.Value as string;
                    inReader.Read();
                    switch (propName)
                    {
                        case "url": dto.fileUrl = JsonStreamingHelper.ReadString(inReader); break;
                        case "name": dto.fileName = JsonStreamingHelper.ReadString(inReader); break;
                        case "type": dto.mimeType = JsonStreamingHelper.ReadString(inReader); break;
                        case "size": dto.fileSize = JsonStreamingHelper.ReadInt(inReader); break;
                        case "require_auth": dto.requireAuth = JsonStreamingHelper.ReadBool(inReader); break;
                        case "data": dto.data = JsonStreamingHelper.ReadString(inReader); break;
                        case "custom": dto.custom = JsonStreamingHelper.ReadString(inReader); break;
                        default: JsonStreamingHelper.SkipValue(inReader); break;
                    }
                }

                return dto;
            }
        }

        private string _fileUrl;
        private string _fileName;
        private string _mimeType;
        private int _fileSize;
        private bool _requireAuth;
        internal List<ThumbnailDto> thumbnailDtos;
        private FileDto _file;

        internal string FileUrl { get; private set; }
        internal string FileName { get; private set; }
        internal string MimeType { get; private set; }
        internal int FileSize { get; private set; }
        internal bool RequireAuth { get; private set; }

        internal override bool TryReadSubclassField(JsonTextReader inReader, string inPropName)
        {
            switch (inPropName)
            {
                case "url": _fileUrl = JsonStreamingHelper.ReadString(inReader); return true;
                case "name": _fileName = JsonStreamingHelper.ReadString(inReader); return true;
                case "type": _mimeType = JsonStreamingHelper.ReadString(inReader); return true;
                case "size": _fileSize = JsonStreamingHelper.ReadInt(inReader); return true;
                case "require_auth": _requireAuth = JsonStreamingHelper.ReadBool(inReader); return true;
                case "thumbnails": thumbnailDtos = ThumbnailDto.ReadListFromJson(inReader); return true;
                case "file": _file = FileDto.ReadFromJson(inReader); return true;
                default: return false;
            }
        }

        internal override void PostDeserialize()
        {
            base.PostDeserialize();

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
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadFromJson);
        }

        internal static FileMessageDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            FileMessageDto dto = new FileMessageDto();
            ReadFields(inReader, dto);
            return dto;
        }

        internal override void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            WriteBaseFields(inWriter);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "url", _fileUrl);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "name", _fileName);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "type", _mimeType);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "size", _fileSize);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "require_auth", _requireAuth);
            if (thumbnailDtos != null)
            {
                inWriter.WritePropertyName("thumbnails");
                inWriter.WriteStartArray();
                foreach (ThumbnailDto thumbnailDto in thumbnailDtos)
                {
                    thumbnailDto.WriteToJson(inWriter);
                }
                inWriter.WriteEndArray();
            }
            inWriter.WriteEndObject();
        }
    }
}
