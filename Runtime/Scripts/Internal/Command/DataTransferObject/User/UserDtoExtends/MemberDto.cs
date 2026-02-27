//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class MemberDto : UserDto
    {
        internal bool? isBlockedByMe;
        internal bool? isBlockingMe;
        internal bool? isMuted;
        internal string role;
        internal string state;

        internal override bool TryReadSubclassField(JsonTextReader inReader, string inPropName)
        {
            switch (inPropName)
            {
                case "is_blocked_by_me": isBlockedByMe = JsonStreamingHelper.ReadNullableBool(inReader); return true;
                case "is_blocking_me": isBlockingMe = JsonStreamingHelper.ReadNullableBool(inReader); return true;
                case "is_muted": isMuted = JsonStreamingHelper.ReadNullableBool(inReader); return true;
                case "role": role = JsonStreamingHelper.ReadString(inReader); return true;
                case "state": state = JsonStreamingHelper.ReadString(inReader); return true;
                default: return false;
            }
        }

        internal static MemberDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            MemberDto dto = new MemberDto();
            ReadFields(inReader, dto);
            return dto;
        }

        internal static MemberDto ReadFromJsonString(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadFromJson);
        }

        internal static List<MemberDto> ReadListFromJsonString(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadListFromJson);
        }

        internal static List<MemberDto> ReadListFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<MemberDto> list = new List<MemberDto>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                MemberDto dto = ReadFromJson(inReader);
                if (dto != null)
                    list.Add(dto);
            }

            return list;
        }

        internal override void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            WriteFields(inWriter);
            JsonStreamingHelper.WriteNullableBool(inWriter, "is_blocked_by_me", isBlockedByMe);
            JsonStreamingHelper.WriteNullableBool(inWriter, "is_blocking_me", isBlockingMe);
            JsonStreamingHelper.WriteNullableBool(inWriter, "is_muted", isMuted);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "role", role);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "state", state);
            inWriter.WriteEndObject();
        }
    }
}
