//
//  Copyright (c) 2023 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class PluginDto
    {
        internal string type;
        internal string vendor;
        internal Dictionary<string, string> detail;

        internal static PluginDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            PluginDto dto = new PluginDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "type": dto.type = JsonStreamingHelper.ReadString(inReader); break;
                    case "vendor": dto.vendor = JsonStreamingHelper.ReadString(inReader); break;
                    case "detail": dto.detail = JsonStreamingHelper.ReadStringDictionary(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }

        internal static List<PluginDto> ReadListFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<PluginDto> list = new List<PluginDto>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                PluginDto dto = ReadFromJson(inReader);
                if (dto != null)
                    list.Add(dto);
            }

            return list;
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "type", type);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "vendor", vendor);
            JsonStreamingHelper.WriteStringDictionary(inWriter, "detail", detail);
            inWriter.WriteEndObject();
        }
    }
}
