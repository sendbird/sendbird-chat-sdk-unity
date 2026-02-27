//
//  Copyright (c) 2023 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class PollDto
    {
        internal static PollDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
            {
                JsonStreamingHelper.SkipValue(inReader);
                return null;
            }

            PollDto dto = new PollDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                // Skip property name
                inReader.Read();
                JsonStreamingHelper.SkipValue(inReader);
            }

            return dto;
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            inWriter.WriteEndObject();
        }
    }
}
