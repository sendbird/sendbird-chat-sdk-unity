//
//  Copyright (c) 2023 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class ReactionEventDto
    {
        internal string key;
        internal string userId;
        internal long updatedAt;
        internal long msgId;
        private string _operation;

        internal SbReactionEventAction Operation { get; private set; }

        internal static ReactionEventDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            ReactionEventDto dto = new ReactionEventDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "reaction": dto.key = JsonStreamingHelper.ReadString(inReader); break;
                    case "user_id": dto.userId = JsonStreamingHelper.ReadString(inReader); break;
                    case "updated_at": dto.updatedAt = JsonStreamingHelper.ReadLong(inReader); break;
                    case "msg_id": dto.msgId = JsonStreamingHelper.ReadLong(inReader); break;
                    case "operation": dto._operation = JsonStreamingHelper.ReadString(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            dto.PostDeserialize();
            return dto;
        }

        internal static ReactionEventDto ReadFromJsonString(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadFromJson);
        }

        private void PostDeserialize()
        {
            if (string.IsNullOrEmpty(_operation) == false)
                Operation = SbReactionEventActionExtension.JsonNameToType(_operation);
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "reaction", key);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "user_id", userId);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "updated_at", updatedAt);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "msg_id", msgId);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "operation", _operation);
            inWriter.WriteEndObject();
        }
    }
}
