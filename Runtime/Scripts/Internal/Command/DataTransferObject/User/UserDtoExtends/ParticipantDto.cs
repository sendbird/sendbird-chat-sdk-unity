//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class ParticipantDto : UserDto
    {
        internal bool? isMuted;

        internal override bool TryReadSubclassField(JsonTextReader inReader, string inPropName)
        {
            switch (inPropName)
            {
                case "is_muted": isMuted = JsonStreamingHelper.ReadNullableBool(inReader); return true;
                default: return false;
            }
        }

        internal static ParticipantDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            ParticipantDto dto = new ParticipantDto();
            ReadFields(inReader, dto);
            return dto;
        }

        internal static List<ParticipantDto> ReadListFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<ParticipantDto> list = new List<ParticipantDto>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                ParticipantDto dto = ReadFromJson(inReader);
                if (dto != null)
                    list.Add(dto);
            }

            return list;
        }

        internal override void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            WriteFields(inWriter);
            JsonStreamingHelper.WriteNullableBool(inWriter, "is_muted", isMuted);
            inWriter.WriteEndObject();
        }
    }
}
