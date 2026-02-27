//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    public class ReconnectionDto
    {
        internal float interval;
        internal float maxInterval;
        internal int mul;
        internal int retryCount;

        internal static ReconnectionDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            ReconnectionDto dto = new ReconnectionDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "interval": dto.interval = JsonStreamingHelper.ReadFloat(inReader); break;
                    case "max_interval": dto.maxInterval = JsonStreamingHelper.ReadFloat(inReader); break;
                    case "mul": dto.mul = JsonStreamingHelper.ReadInt(inReader); break;
                    case "retry_cnt": dto.retryCount = JsonStreamingHelper.ReadInt(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }
    }
}
