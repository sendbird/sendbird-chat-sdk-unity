//
//  Copyright (c) 2023 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    public class ThumbnailDto
    {
        internal string url;
        internal int width;
        internal int height;
        internal int realWidth;
        internal int realHeight;

        internal static ThumbnailDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            ThumbnailDto dto = new ThumbnailDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "url": dto.url = JsonStreamingHelper.ReadString(inReader); break;
                    case "width": dto.width = JsonStreamingHelper.ReadInt(inReader); break;
                    case "height": dto.height = JsonStreamingHelper.ReadInt(inReader); break;
                    case "real_width": dto.realWidth = JsonStreamingHelper.ReadInt(inReader); break;
                    case "real_height": dto.realHeight = JsonStreamingHelper.ReadInt(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }

        internal static List<ThumbnailDto> ReadListFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<ThumbnailDto> list = new List<ThumbnailDto>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                ThumbnailDto dto = ReadFromJson(inReader);
                if (dto != null)
                    list.Add(dto);
            }

            return list;
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "url", url);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "width", width);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "height", height);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "real_width", realWidth);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "real_height", realHeight);
            inWriter.WriteEndObject();
        }
    }
}
