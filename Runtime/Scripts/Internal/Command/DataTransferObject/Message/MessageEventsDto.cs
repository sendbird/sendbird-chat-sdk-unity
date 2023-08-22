// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class MessageEventsDto
    {
#pragma warning disable CS0649
        [JsonProperty("send_push_notification")] internal readonly string sendPushNotification;
        [JsonProperty("update_unread_count")] internal readonly bool updateUnreadCount;
        [JsonProperty("update_mention_count")] internal readonly bool updateMentionCount;
        [JsonProperty("update_last_message")] internal readonly bool updateLastMessage;
#pragma warning restore CS0649
    }
}