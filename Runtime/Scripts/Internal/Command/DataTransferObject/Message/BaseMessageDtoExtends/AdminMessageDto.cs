// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal sealed class AdminMessageDto : BaseMessageDto
    {
#pragma warning disable CS0649
        // [JsonProperty("target_langs")] internal readonly List<string> translationTargetLanguages;
        // [JsonProperty("translations")] internal readonly Dictionary<string, string> translations;
        // [JsonProperty("plugins")] internal readonly List<PluginDto> plugins;
        // [JsonProperty("poll")] internal readonly PollDto poll;
#pragma warning restore CS0649

        internal override SbBaseMessage CreateMessageInstance(SendbirdChatMainContext inChatMainContext)
        {
            return new SbAdminMessage(this, inChatMainContext);
        }

        internal static AdminMessageDto DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<AdminMessageDto>(inJsonString);
        }

        internal static AdminMessageDto DeserializeFromJson(JObject inJObject)
        {
            return inJObject.ToObjectIgnoreException<AdminMessageDto>();
        }
    }
}