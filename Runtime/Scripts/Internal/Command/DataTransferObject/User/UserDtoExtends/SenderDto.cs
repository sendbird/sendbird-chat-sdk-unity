//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class SenderDto : UserDto
    {
        internal bool? isBlockedByMe;
        internal string role;

        internal override bool TryReadSubclassField(JsonTextReader inReader, string inPropName)
        {
            switch (inPropName)
            {
                case "is_blocked_by_me": isBlockedByMe = JsonStreamingHelper.ReadNullableBool(inReader); return true;
                case "role": role = JsonStreamingHelper.ReadString(inReader); return true;
                default: return false;
            }
        }

        internal static SenderDto ReadFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            SenderDto dto = new SenderDto();
            ReadFields(inReader, dto);
            return dto;
        }

        internal override void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            WriteFields(inWriter);
            JsonStreamingHelper.WriteNullableBool(inWriter, "is_blocked_by_me", isBlockedByMe);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "role", role);
            inWriter.WriteEndObject();
        }
    }
}
