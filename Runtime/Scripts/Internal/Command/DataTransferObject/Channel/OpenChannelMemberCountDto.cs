//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class OpenChannelMemberCountDto
    {
        internal string channelUrl;
        internal long timestamp;
        internal int participantCount;

        internal static OpenChannelMemberCountDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            OpenChannelMemberCountDto dto = new OpenChannelMemberCountDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "channel_url": dto.channelUrl = JsonStreamingHelper.ReadString(inReader); break;
                    case "ts": dto.timestamp = JsonStreamingHelper.ReadLong(inReader); break;
                    case "participant_count": dto.participantCount = JsonStreamingHelper.ReadInt(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }

        internal static List<OpenChannelMemberCountDto> ReadListFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<OpenChannelMemberCountDto> list = new List<OpenChannelMemberCountDto>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                OpenChannelMemberCountDto dto = ReadFromJson(inReader);
                if (dto != null)
                    list.Add(dto);
            }

            return list;
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "channel_url", channelUrl);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "ts", timestamp);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "participant_count", participantCount);
            inWriter.WriteEndObject();
        }
    }
}
