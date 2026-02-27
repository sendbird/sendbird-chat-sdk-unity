//
//  Copyright (c) 2023 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class DeletedMessageDto
    {
        internal long deletedAt;
        internal long messageId;

        internal static DeletedMessageDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            DeletedMessageDto dto = new DeletedMessageDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "deleted_at": dto.deletedAt = JsonStreamingHelper.ReadLong(inReader); break;
                    case "message_id": dto.messageId = JsonStreamingHelper.ReadLong(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }

        internal static List<DeletedMessageDto> ReadListFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<DeletedMessageDto> list = new List<DeletedMessageDto>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                DeletedMessageDto dto = ReadFromJson(inReader);
                if (dto != null)
                    list.Add(dto);
            }

            return list;
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "deleted_at", deletedAt);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "message_id", messageId);
            inWriter.WriteEndObject();
        }
    }
}
