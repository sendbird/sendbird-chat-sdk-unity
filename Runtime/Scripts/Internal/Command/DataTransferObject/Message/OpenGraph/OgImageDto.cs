//
//  Copyright (c) 2023 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class OgImageDto
    {
        internal string url;
        internal string secureUrl;
        internal string type;
        internal string alt;
        internal int width;
        internal int height;

        internal static OgImageDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            OgImageDto dto = new OgImageDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "url": dto.url = JsonStreamingHelper.ReadString(inReader); break;
                    case "secure_url": dto.secureUrl = JsonStreamingHelper.ReadString(inReader); break;
                    case "type": dto.type = JsonStreamingHelper.ReadString(inReader); break;
                    case "alt": dto.alt = JsonStreamingHelper.ReadString(inReader); break;
                    case "width": dto.width = JsonStreamingHelper.ReadInt(inReader); break;
                    case "height": dto.height = JsonStreamingHelper.ReadInt(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "url", url);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "secure_url", secureUrl);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "type", type);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "alt", alt);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "width", width);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "height", height);
            inWriter.WriteEndObject();
        }
    }
}
