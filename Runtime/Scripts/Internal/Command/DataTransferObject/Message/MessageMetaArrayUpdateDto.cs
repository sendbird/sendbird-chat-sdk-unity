//
//  Copyright (c) 2023 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class MessageMetaArrayUpdateDto
    {
        internal enum Mode
        {
            [JsonName("add")] Add,
            [JsonName("remove")] Remove
        }

        internal string mode;
        internal bool upsert;
        internal List<MessageMetaArrayDto> array;

        internal MessageMetaArrayUpdateDto(List<MessageMetaArrayDto> inArray, Mode inMode, bool inUpsert = true)
        {
            array = inArray;
            mode = inMode.ToJsonName();
            upsert = inUpsert;
        }

        internal void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "mode", mode);
            JsonStreamingHelper.WriteProperty(inWriter, "upsert", upsert);
            MessageMetaArrayDto.WriteListToJson(inWriter, "array", array);
            inWriter.WriteEndObject();
        }
    }
}
