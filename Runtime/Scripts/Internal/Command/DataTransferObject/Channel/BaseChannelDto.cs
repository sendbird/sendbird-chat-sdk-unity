//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    internal abstract class BaseChannelDto
    {
        internal string channelUrl;
        internal string name;
        internal string coverUrl;
        internal UserDto createdByUserDto;
        internal long createdAt;
        internal string data;
        internal bool isEphemeral;
        internal bool isFrozen;
        internal long lastSyncedChangeLogTimeStamp;
        internal Dictionary<string, string> metaData;
        internal string customType;

        internal virtual bool TryReadSubclassField(JsonTextReader inReader, string inPropName)
        {
            return false;
        }

        internal static void ReadBaseFields(JsonTextReader inReader, BaseChannelDto inDto)
        {
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();

                if (inDto.TryReadSubclassField(inReader, propName))
                    continue;

                switch (propName)
                {
                    case "channel_url": inDto.channelUrl = JsonStreamingHelper.ReadString(inReader); break;
                    case "name": inDto.name = JsonStreamingHelper.ReadString(inReader); break;
                    case "cover_url": inDto.coverUrl = JsonStreamingHelper.ReadString(inReader); break;
                    case "created_by": inDto.createdByUserDto = UserDto.ReadUserDtoFromJson(inReader); break;
                    case "created_at": inDto.createdAt = JsonStreamingHelper.ReadLong(inReader); break;
                    case "data": inDto.data = JsonStreamingHelper.ReadString(inReader); break;
                    case "is_ephemeral": inDto.isEphemeral = JsonStreamingHelper.ReadBool(inReader); break;
                    case "freeze": inDto.isFrozen = JsonStreamingHelper.ReadBool(inReader); break;
                    case "last_synced_changelog_ts": inDto.lastSyncedChangeLogTimeStamp = JsonStreamingHelper.ReadLong(inReader); break;
                    case "metadata": inDto.metaData = JsonStreamingHelper.ReadStringDictionary(inReader); break;
                    case "custom_type": inDto.customType = JsonStreamingHelper.ReadString(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }
        }

        internal void WriteBaseFields(JsonTextWriter inWriter)
        {
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "channel_url", channelUrl);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "name", name);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "cover_url", coverUrl);
            if (createdByUserDto != null)
            {
                inWriter.WritePropertyName("created_by");
                createdByUserDto.WriteToJson(inWriter);
            }
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "created_at", createdAt);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "data", data);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "is_ephemeral", isEphemeral);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "freeze", isFrozen);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "last_synced_changelog_ts", lastSyncedChangeLogTimeStamp);
            JsonStreamingHelper.WriteStringDictionary(inWriter, "metadata", metaData);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "custom_type", customType);
        }

        internal static BaseChannelDto ToChannelDtoByChannelType(JObject inJObject, SbChannelType inChannelType)
        {
            if (inJObject == null)
                return null;

            string jsonString = inJObject.ToString(Formatting.None);
            return ReadFromJsonString(jsonString, inChannelType);
        }

        internal static BaseChannelDto ReadFromJsonString(string inJsonString, SbChannelType inChannelType)
        {
            if (string.IsNullOrEmpty(inJsonString))
                return null;

            if (inChannelType == SbChannelType.Group)
                return GroupChannelDto.DeserializeFromJson(inJsonString);

            if (inChannelType == SbChannelType.Open)
                return OpenChannelDto.DeserializeFromJson(inJsonString);

            if (inChannelType == SbChannelType.Feed)
                return null;

            return null;
        }
    }
}
