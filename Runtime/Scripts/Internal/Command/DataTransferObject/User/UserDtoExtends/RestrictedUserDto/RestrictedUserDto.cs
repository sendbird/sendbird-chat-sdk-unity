//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class RestrictedUserDto : UserDto
    {
        private string _description;
        private string _mutedDescription;
        private long? _endAt;
        private long? _mutedEndAt;
        private long? _remainingDuration;
        private string _restrictionType;

        internal string Description { get; private set; }
        internal long EndAt { get; private set; }
        internal long RemainingDuration { get; private set; }
        internal SbRestrictionType RestrictionType { get; set; } = SbRestrictionType.Muted;

        internal override bool TryReadSubclassField(JsonTextReader inReader, string inPropName)
        {
            switch (inPropName)
            {
                case "description": _description = JsonStreamingHelper.ReadString(inReader); return true;
                case "muted_description": _mutedDescription = JsonStreamingHelper.ReadString(inReader); return true;
                case "end_at": _endAt = JsonStreamingHelper.ReadNullableLong(inReader); return true;
                case "muted_end_at": _mutedEndAt = JsonStreamingHelper.ReadNullableLong(inReader); return true;
                case "remaining_duration": _remainingDuration = JsonStreamingHelper.ReadNullableLong(inReader); return true;
                case "restriction_type": _restrictionType = JsonStreamingHelper.ReadString(inReader); return true;
                default: return false;
            }
        }

        internal override void PostDeserialize()
        {
            base.PostDeserialize();

            if (string.IsNullOrEmpty(_restrictionType) == false)
                RestrictionType = SbRestrictionTypeExtension.JsonNameToType(_restrictionType);

            if (string.IsNullOrEmpty(_description) == false)
            {
                Description = _description;
            }
            else
            {
                Description = _mutedDescription;
            }

            if (_endAt != null)
            {
                EndAt = _endAt.Value;
            }
            else if (_mutedEndAt != null)
            {
                EndAt = _mutedEndAt.Value;
            }

            if (_remainingDuration != null)
            {
                RemainingDuration = _remainingDuration.Value;
            }
            else
            {
                RemainingDuration = -1;
            }
        }

        internal static RestrictedUserDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            RestrictedUserDto dto = new RestrictedUserDto();
            ReadFields(inReader, dto);
            return dto;
        }

        internal static RestrictedUserDto ReadFromJsonString(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadFromJson);
        }

        internal static List<RestrictedUserDto> ReadListFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<RestrictedUserDto> list = new List<RestrictedUserDto>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                RestrictedUserDto dto = ReadFromJson(inReader);
                if (dto != null)
                    list.Add(dto);
            }

            return list;
        }

        internal override void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            WriteFields(inWriter);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "description", _description);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "muted_description", _mutedDescription);
            JsonStreamingHelper.WriteNullableLong(inWriter, "end_at", _endAt);
            JsonStreamingHelper.WriteNullableLong(inWriter, "muted_end_at", _mutedEndAt);
            JsonStreamingHelper.WriteNullableLong(inWriter, "remaining_duration", _remainingDuration);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "restriction_type", _restrictionType);
            inWriter.WriteEndObject();
        }
    }
}
