// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class MessageMetaArrayUpdateDto
    {
        internal enum Mode
        {
            [JsonName("add")] Add,
            [JsonName("remove")] Remove
        }

#pragma warning disable CS0649
        [JsonProperty("mode")] internal readonly string mode;
        [JsonProperty("upsert")] internal readonly bool upsert;
        [JsonProperty("array")] internal readonly List<MessageMetaArrayDto> array;
#pragma warning restore CS0649

        internal MessageMetaArrayUpdateDto(List<MessageMetaArrayDto> inArray, Mode inMode, bool inUpsert = true)
        {
            array = inArray;
            mode = inMode.ToJsonName();
            upsert = inUpsert;
        }
    }
}