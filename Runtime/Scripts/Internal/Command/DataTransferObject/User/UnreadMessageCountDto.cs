//
//  Copyright (c) 2023 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class UnreadMessageCountDto
    {
        internal long timeStamp;
        internal int groupChannelCount;
        internal int feedChannelCount;
        internal Dictionary<string, int> customTypes;

        internal static UnreadMessageCountDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            UnreadMessageCountDto dto = new UnreadMessageCountDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "ts": dto.timeStamp = JsonStreamingHelper.ReadLong(inReader); break;
                    case "all": dto.groupChannelCount = JsonStreamingHelper.ReadInt(inReader); break;
                    case "feed": dto.feedChannelCount = JsonStreamingHelper.ReadInt(inReader); break;
                    case "custom_types": dto.customTypes = JsonStreamingHelper.ReadStringIntDictionary(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "ts", timeStamp);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "all", groupChannelCount);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "feed", feedChannelCount);
            JsonStreamingHelper.WriteStringIntDictionary(inWriter, "custom_types", customTypes);
            inWriter.WriteEndObject();
        }
    }
}
