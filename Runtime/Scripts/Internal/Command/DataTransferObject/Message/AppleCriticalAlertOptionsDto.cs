//
//  Copyright (c) 2023 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class AppleCriticalAlertOptionsDto
    {
        internal string name;
        internal double volume;

        internal AppleCriticalAlertOptionsDto() { }

        internal AppleCriticalAlertOptionsDto(SbAppleCriticalAlertOptions inSbAppleCriticalAlertOptions)
        {
            name = inSbAppleCriticalAlertOptions.Name;
            volume = inSbAppleCriticalAlertOptions.Volume;
        }

        internal static AppleCriticalAlertOptionsDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            AppleCriticalAlertOptionsDto dto = new AppleCriticalAlertOptionsDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "critical_sound": dto.name = JsonStreamingHelper.ReadString(inReader); break;
                    case "volume": dto.volume = JsonStreamingHelper.ReadDouble(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "critical_sound", name);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "volume", volume);
            inWriter.WriteEndObject();
        }
    }
}
