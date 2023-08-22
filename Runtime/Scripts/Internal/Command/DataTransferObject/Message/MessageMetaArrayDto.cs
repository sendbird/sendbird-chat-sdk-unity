// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class MessageMetaArrayDto
    {
#pragma warning disable CS0649
        [JsonProperty("key")] internal readonly string key;
        [JsonProperty("value")] internal readonly List<string> value;
#pragma warning restore CS0649

        internal MessageMetaArrayDto(string inKey, List<string> inValue)
        {
            key = inKey;
            value = inValue;
            if (value == null)
                value = new List<string>();
        }
    }
}