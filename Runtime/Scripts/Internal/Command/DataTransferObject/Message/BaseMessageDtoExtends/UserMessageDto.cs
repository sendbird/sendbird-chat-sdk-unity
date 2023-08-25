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
    internal sealed class UserMessageDto : BaseMessageDto
    {
#pragma warning disable CS0649
        [JsonProperty("target_langs")] internal readonly List<string> translationTargetLanguages;
        [JsonProperty("translations")] internal readonly Dictionary<string, string> translations;
        [JsonProperty("plugins")] internal readonly List<PluginDto> plugins;
        [JsonProperty("poll")] internal readonly PollDto poll;
#pragma warning restore CS0649

        internal override SbBaseMessage CreateMessageInstance(SendbirdChatMainContext inChatMainContext)
        {
            return new SbUserMessage(this, inChatMainContext);
        }

        internal static UserMessageDto DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<UserMessageDto>(inJsonString);
        }

        internal static UserMessageDto DeserializeFromJson(JObject inJObject)
        {
            return inJObject.ToObjectIgnoreException<UserMessageDto>();
        }
    }
}