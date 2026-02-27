//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    public class AppInfoDto
    {
        internal string emojiHash;
        internal long fileUploadSizeLimit;
        internal List<string> premiumFeatures;
        internal bool useReaction;
        internal List<string> applicationAttributes;
        internal bool disableSuperGroupMack;

        internal static AppInfoDto DeserializeFromJson(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadFromJson);
        }

        private static AppInfoDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            AppInfoDto dto = new AppInfoDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "emoji_hash": dto.emojiHash = JsonStreamingHelper.ReadString(inReader); break;
                    case "file_upload_size_limit": dto.fileUploadSizeLimit = JsonStreamingHelper.ReadLong(inReader); break;
                    case "premium_feature_list": dto.premiumFeatures = JsonStreamingHelper.ReadStringList(inReader); break;
                    case "use_reaction": dto.useReaction = JsonStreamingHelper.ReadBool(inReader); break;
                    case "application_attributes": dto.applicationAttributes = JsonStreamingHelper.ReadStringList(inReader); break;
                    case "disable_supergroup_mack": dto.disableSuperGroupMack = JsonStreamingHelper.ReadBool(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }
    }
}
