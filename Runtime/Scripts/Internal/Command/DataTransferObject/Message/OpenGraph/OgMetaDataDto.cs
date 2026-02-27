//
//  Copyright (c) 2023 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class OgMetaDataDto
    {
        internal string title;
        internal string url;
        internal string description;
        internal OgImageDto image;

        internal static OgMetaDataDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            OgMetaDataDto dto = new OgMetaDataDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "og:title": dto.title = JsonStreamingHelper.ReadString(inReader); break;
                    case "og:url": dto.url = JsonStreamingHelper.ReadString(inReader); break;
                    case "og:description": dto.description = JsonStreamingHelper.ReadString(inReader); break;
                    case "og:image": dto.image = OgImageDto.ReadFromJson(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }

        internal static OgMetaDataDto ReadFromJsonString(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadFromJson);
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "og:title", title);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "og:url", url);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "og:description", description);
            if (image != null)
            {
                inWriter.WritePropertyName("og:image");
                image.WriteToJson(inWriter);
            }
            inWriter.WriteEndObject();
        }
    }
}
