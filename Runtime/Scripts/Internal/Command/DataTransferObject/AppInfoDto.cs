// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    public class AppInfoDto
    {
#pragma warning disable CS0649
        [JsonProperty("emoji_hash")] internal readonly string emojiHash;
        [JsonProperty("file_upload_size_limit")] internal readonly long fileUploadSizeLimit;
        [JsonProperty("premium_feature_list")] internal readonly List<string> premiumFeatures;
        [JsonProperty("use_reaction")] internal readonly bool useReaction;
        [JsonProperty("application_attributes")] internal readonly List<string> applicationAttributes;
        [JsonProperty("disable_supergroup_mack")] internal readonly bool disableSuperGroupMack;
        // [JsonProperty("concurrent_call_limit")] internal readonly string concurrentCallLimit;
        // [JsonProperty("back_off_delay")] internal readonly string backOffDelay;
#pragma warning restore CS0649
        
        internal static AppInfoDto DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<AppInfoDto>(inJsonString);
        }
    }
}