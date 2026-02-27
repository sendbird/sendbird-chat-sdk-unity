//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UserMessageDto : BaseMessageDto
    {
        internal List<string> translationTargetLanguages;
        internal Dictionary<string, string> translations;
        internal List<PluginDto> plugins;
        internal PollDto poll;

        internal override bool TryReadSubclassField(JsonTextReader inReader, string inPropName)
        {
            switch (inPropName)
            {
                case "target_langs": translationTargetLanguages = JsonStreamingHelper.ReadStringList(inReader); return true;
                case "translations": translations = JsonStreamingHelper.ReadStringDictionary(inReader); return true;
                case "plugins": plugins = PluginDto.ReadListFromJson(inReader); return true;
                case "poll": poll = PollDto.ReadFromJson(inReader); return true;
                default: return false;
            }
        }

        internal override SbBaseMessage CreateMessageInstance(SendbirdChatMainContext inChatMainContext)
        {
            return new SbUserMessage(this, inChatMainContext);
        }

        internal static UserMessageDto DeserializeFromJson(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadFromJson);
        }

        internal static UserMessageDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            UserMessageDto dto = new UserMessageDto();
            ReadFields(inReader, dto);
            return dto;
        }

        internal override void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            WriteBaseFields(inWriter);
            JsonStreamingHelper.WriteStringList(inWriter, "target_langs", translationTargetLanguages);
            JsonStreamingHelper.WriteStringDictionary(inWriter, "translations", translations);
            if (plugins != null)
            {
                inWriter.WritePropertyName("plugins");
                inWriter.WriteStartArray();
                foreach (PluginDto pluginDto in plugins)
                {
                    pluginDto.WriteToJson(inWriter);
                }
                inWriter.WriteEndArray();
            }
            if (poll != null)
            {
                inWriter.WritePropertyName("poll");
                poll.WriteToJson(inWriter);
            }
            inWriter.WriteEndObject();
        }
    }
}
