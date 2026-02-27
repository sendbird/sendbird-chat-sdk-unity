//
//  Copyright (c) 2023 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class MessageEventsDto
    {
        internal string sendPushNotification;
        internal bool updateUnreadCount;
        internal bool updateMentionCount;
        internal bool updateLastMessage;

        internal static MessageEventsDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            MessageEventsDto dto = new MessageEventsDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "send_push_notification": dto.sendPushNotification = JsonStreamingHelper.ReadString(inReader); break;
                    case "update_unread_count": dto.updateUnreadCount = JsonStreamingHelper.ReadBool(inReader); break;
                    case "update_mention_count": dto.updateMentionCount = JsonStreamingHelper.ReadBool(inReader); break;
                    case "update_last_message": dto.updateLastMessage = JsonStreamingHelper.ReadBool(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "send_push_notification", sendPushNotification);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "update_unread_count", updateUnreadCount);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "update_mention_count", updateMentionCount);
            JsonStreamingHelper.WritePropertyIfNotDefault(inWriter, "update_last_message", updateLastMessage);
            inWriter.WriteEndObject();
        }
    }
}
