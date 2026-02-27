//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class OpenChannelDto : BaseChannelDto
    {
        internal List<UserDto> operatorUserDtos;
        internal int participantCount;
        internal long maxLengthMessage;
        internal bool isDynamicPartitioned;

        internal override bool TryReadSubclassField(JsonTextReader inReader, string inPropName)
        {
            switch (inPropName)
            {
                case "operators": operatorUserDtos = UserDto.ReadUserDtoListFromJson(inReader); return true;
                case "participant_count": participantCount = JsonStreamingHelper.ReadInt(inReader); return true;
                case "max_length_message": maxLengthMessage = JsonStreamingHelper.ReadLong(inReader); return true;
                case "is_dynamic_partitioned": isDynamicPartitioned = JsonStreamingHelper.ReadBool(inReader); return true;
                default: return false;
            }
        }

        internal static List<OpenChannelDto> ReadListFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<OpenChannelDto> list = new List<OpenChannelDto>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                OpenChannelDto dto = ReadFromJson(inReader);
                if (dto != null)
                    list.Add(dto);
            }

            return list;
        }

        internal static OpenChannelDto DeserializeFromJson(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadFromJson);
        }

        internal static OpenChannelDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            OpenChannelDto dto = new OpenChannelDto();
            ReadBaseFields(inReader, dto);
            return dto;
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            WriteBaseFields(inWriter);
            UserDto.WriteListToJson(inWriter, "operators", operatorUserDtos);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "participant_count", participantCount);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "max_length_message", maxLengthMessage);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_dynamic_partitioned", isDynamicPartitioned);
            inWriter.WriteEndObject();
        }
    }
}
