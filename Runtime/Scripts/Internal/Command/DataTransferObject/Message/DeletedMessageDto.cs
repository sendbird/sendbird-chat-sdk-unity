// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class DeletedMessageDto
    {
#pragma warning disable CS0649
        [JsonProperty("deleted_at")] internal readonly long deletedAt;
        [JsonProperty("message_id")] internal readonly long messageId;
#pragma warning restore CS0649
    }
}