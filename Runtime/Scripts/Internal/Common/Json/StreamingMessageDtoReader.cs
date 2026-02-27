//
//  Copyright (c) 2026 Sendbird, Inc.
//

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    /// <summary>
    /// Single-pass streaming reader that simultaneously extracts WS command fields and
    /// populates a BaseMessageDto subclass, eliminating the need for JObject intermediate DOM.
    /// </summary>
    internal static class StreamingMessageDtoReader
    {
        internal struct CommandFields
        {
            internal string reqId;
            internal string requestId;
            internal UnreadMessageCountDto unreadMessageCountDto;
            internal string oldValuesMentionType;
            internal List<string> oldValuesMentionedUserIds;
        }

        internal static CommandFields Deserialize<TDto>(string inJsonString, out TDto outDto, int inOffset = 0) where TDto : BaseMessageDto, new()
        {
            CommandFields commandFields = new CommandFields();
            outDto = default;

            if (string.IsNullOrEmpty(inJsonString))
                return commandFields;

            try
            {
                using (JsonTextReader reader = inOffset > 0
                    ? JsonStreamingPool.CreateReader(inJsonString, inOffset)
                    : JsonStreamingPool.CreateReader(inJsonString))
                {
                    reader.Read();
                    if (reader.TokenType != JsonToken.StartObject)
                        return commandFields;

                    TDto dto = new TDto();

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.EndObject)
                            break;

                        string propName = reader.Value as string;
                        reader.Read();

                        // 1. Try subclass-specific fields (UserMessageDto, FileMessageDto, etc.)
                        if (dto.TryReadSubclassField(reader, propName))
                            continue;

                        // 2. Handle fields shared between command and DTO
                        switch (propName)
                        {
                            case "req_id":
                            {
                                string value = JsonStreamingHelper.ReadString(reader);
                                commandFields.reqId = value;
                                dto.SetMessageReqId(value);
                                continue;
                            }
                            case "request_id":
                            {
                                string value = JsonStreamingHelper.ReadString(reader);
                                commandFields.requestId = value;
                                dto.SetMessageRequestId(value);
                                continue;
                            }
                            case "unread_cnt":
                                commandFields.unreadMessageCountDto = UnreadMessageCountDto.ReadFromJson(reader);
                                continue;
                            case "old_values":
                                ReadOldValues(reader, ref commandFields);
                                continue;
                        }

                        // 3. Try base message DTO fields
                        if (dto.TryReadBaseField(reader, propName))
                            continue;

                        // 4. Unknown field
                        JsonStreamingHelper.SkipValue(reader);
                    }

                    dto.PostDeserialize();
                    outDto = dto;
                }
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.Command,
                    $"StreamingMessageDtoReader.Deserialize<{typeof(TDto).Name}> failed: {exception.Message}");
                outDto = default;
            }

            return commandFields;
        }

        private static void ReadOldValues(JsonTextReader inReader, ref CommandFields inCommandFields)
        {
            if (inReader.TokenType == JsonToken.Null)
                return;

            if (inReader.TokenType != JsonToken.StartObject)
            {
                JsonStreamingHelper.SkipValue(inReader);
                return;
            }

            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "mention_type": inCommandFields.oldValuesMentionType = JsonStreamingHelper.ReadString(inReader); break;
                    case "mentioned_user_ids": inCommandFields.oldValuesMentionedUserIds = JsonStreamingHelper.ReadStringList(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }
        }
    }
}
