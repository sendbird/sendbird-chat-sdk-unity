//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GroupChannelMemberCountDto
    {
        internal string channelUrl;
        internal long timestamp;
        internal int joinedMemberCount;
        internal int memberCount;

        internal static GroupChannelMemberCountDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            GroupChannelMemberCountDto dto = new GroupChannelMemberCountDto();
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
                    case "joined_member_count": dto.joinedMemberCount = JsonStreamingHelper.ReadInt(inReader); break;
                    case "member_count": dto.memberCount = JsonStreamingHelper.ReadInt(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }

        internal static List<GroupChannelMemberCountDto> ReadListFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<GroupChannelMemberCountDto> list = new List<GroupChannelMemberCountDto>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                GroupChannelMemberCountDto dto = ReadFromJson(inReader);
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
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "joined_member_count", joinedMemberCount);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "member_count", memberCount);
            inWriter.WriteEndObject();
        }
    }
}
