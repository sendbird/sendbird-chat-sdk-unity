//
//  Copyright (c) 2023 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class ReactionDto
    {
        internal string key;
        internal List<string> userIds;
        internal long latestUpdatedAt;

        internal static ReactionDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            ReactionDto dto = new ReactionDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "key": dto.key = JsonStreamingHelper.ReadString(inReader); break;
                    case "user_ids": dto.userIds = JsonStreamingHelper.ReadStringList(inReader); break;
                    case "updated_at": dto.latestUpdatedAt = JsonStreamingHelper.ReadLong(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }

        internal static List<ReactionDto> ReadListFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<ReactionDto> list = new List<ReactionDto>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                ReactionDto dto = ReadFromJson(inReader);
                if (dto != null)
                    list.Add(dto);
            }

            return list;
        }

        internal static List<ReactionDto> ReadListFromJsonString(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadListFromJson);
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "key", key);
            JsonStreamingHelper.WriteStringList(inWriter, "user_ids", userIds);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "updated_at", latestUpdatedAt);
            inWriter.WriteEndObject();
        }
    }
}
