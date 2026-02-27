//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class AdminMessageDto : BaseMessageDto
    {
        internal override bool TryReadSubclassField(JsonTextReader inReader, string inPropName)
        {
            return false;
        }

        internal override SbBaseMessage CreateMessageInstance(SendbirdChatMainContext inChatMainContext)
        {
            return new SbAdminMessage(this, inChatMainContext);
        }

        internal static AdminMessageDto DeserializeFromJson(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadFromJson);
        }

        internal static AdminMessageDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            AdminMessageDto dto = new AdminMessageDto();
            ReadFields(inReader, dto);
            return dto;
        }

        internal override void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            WriteBaseFields(inWriter);
            inWriter.WriteEndObject();
        }
    }
}
