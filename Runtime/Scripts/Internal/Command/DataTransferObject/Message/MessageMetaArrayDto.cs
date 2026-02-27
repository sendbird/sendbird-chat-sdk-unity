//
//  Copyright (c) 2023 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class MessageMetaArrayDto
    {
        internal string key;
        internal List<string> value;

        internal MessageMetaArrayDto() { }

        internal MessageMetaArrayDto(string inKey, List<string> inValue)
        {
            key = inKey;
            value = inValue;
            if (value == null)
                value = new List<string>();
        }

        internal static MessageMetaArrayDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            MessageMetaArrayDto dto = new MessageMetaArrayDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "key": dto.key = JsonStreamingHelper.ReadString(inReader); break;
                    case "value": dto.value = JsonStreamingHelper.ReadStringList(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }

        internal static List<MessageMetaArrayDto> ReadListFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<MessageMetaArrayDto> list = new List<MessageMetaArrayDto>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                MessageMetaArrayDto dto = ReadFromJson(inReader);
                if (dto != null)
                    list.Add(dto);
            }

            return list;
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "key", key);
            JsonStreamingHelper.WriteStringList(inWriter, "value", value);
            inWriter.WriteEndObject();
        }

        internal static void WriteListToJson(JsonTextWriter inWriter, string inName, List<MessageMetaArrayDto> inList)
        {
            if (inList == null)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteStartArray();
            foreach (MessageMetaArrayDto dto in inList)
            {
                dto.WriteToJson(inWriter);
            }
            inWriter.WriteEndArray();
        }
    }
}
