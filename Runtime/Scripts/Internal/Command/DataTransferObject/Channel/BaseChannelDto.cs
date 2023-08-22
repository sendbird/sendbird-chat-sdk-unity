// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal abstract class BaseChannelDto
    {
#pragma warning disable CS0649
        [JsonProperty("channel_url")] internal readonly string channelUrl;
        [JsonProperty("name")] internal readonly string name;
        [JsonProperty("cover_url")] internal readonly string coverUrl;
        [JsonProperty("created_by")] internal readonly UserDto createdByUserDto;
        [JsonProperty("created_at")] internal readonly long createdAt;
        [JsonProperty("data")] internal readonly string data;
        [JsonProperty("is_ephemeral")] internal readonly bool isEphemeral;
        [JsonProperty("freeze")] internal readonly bool isFrozen;
        [JsonProperty("last_synced_changelog_ts")] internal readonly long lastSyncedChangeLogTimeStamp;
        [JsonProperty("metadata")] internal readonly Dictionary<string, string> metaData;
        [JsonProperty("custom_type")] internal readonly string customType;
#pragma warning restore CS0649

        internal static BaseChannelDto ToChannelDtoByChannelType(JObject inJObject, SbChannelType inChannelType)
        {
            if (inJObject != null)
            {
                if (inChannelType == SbChannelType.Group)
                    return inJObject.ToObjectIgnoreException<GroupChannelDto>();

                if (inChannelType == SbChannelType.Open)
                    return inJObject.ToObjectIgnoreException<OpenChannelDto>();

                if (inChannelType == SbChannelType.Feed)
                    return null;
            }

            return null;
        }
    }
}