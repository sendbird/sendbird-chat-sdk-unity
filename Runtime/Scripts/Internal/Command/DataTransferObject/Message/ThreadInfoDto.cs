//
//  Copyright (c) 2023 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class ThreadInfoDto
    {
        internal List<UserDto> mostRepliedUsers;
        internal long lastRepliedAt;
        internal long updatedAt;
        internal int replyCount;

        internal static ThreadInfoDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            ThreadInfoDto dto = new ThreadInfoDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "most_replies": dto.mostRepliedUsers = UserDto.ReadUserDtoListFromJson(inReader); break;
                    case "last_replied_at": dto.lastRepliedAt = JsonStreamingHelper.ReadLong(inReader); break;
                    case "updated_at": dto.updatedAt = JsonStreamingHelper.ReadLong(inReader); break;
                    case "reply_count": dto.replyCount = JsonStreamingHelper.ReadInt(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }

        internal static ThreadInfoDto ReadFromJsonString(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadFromJson);
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            UserDto.WriteListToJson(inWriter, "most_replies", mostRepliedUsers);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "last_replied_at", lastRepliedAt);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "updated_at", updatedAt);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "reply_count", replyCount);
            inWriter.WriteEndObject();
        }
    }
}
