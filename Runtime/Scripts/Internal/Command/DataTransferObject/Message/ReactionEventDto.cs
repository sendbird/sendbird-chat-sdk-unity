// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class ReactionEventDto
    {
#pragma warning disable CS0649
        [JsonProperty("reaction")] internal readonly string key;
        [JsonProperty("user_id")] internal readonly string userId;
        [JsonProperty("updated_at")] internal readonly long updatedAt;
        [JsonProperty("msg_id")] internal readonly long msgId;
        [JsonProperty("operation")] private readonly string _operation;
#pragma warning restore CS0649

        internal SbReactionEventAction Operation { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (string.IsNullOrEmpty(_operation) == false)
                Operation = SbReactionEventActionExtension.JsonNameToType(_operation);
        }
    }
}