// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UserDto
    {
#pragma warning disable CS0649
        [JsonProperty("user_id")] private readonly string _userId;
        [JsonProperty("guest_id")] private readonly string _guestId;
        [JsonProperty("friend_discovery_key")] internal readonly string friendDiscoveryKey;
        [JsonProperty("friend_name")] internal readonly string friendName;
        [JsonProperty("image")] internal readonly string imageUrl;
        [JsonProperty("is_active")] internal readonly bool? isActive;
        [JsonProperty("is_online")] internal readonly bool? isOnline;
        [JsonProperty("last_seen_at")] internal readonly long? lastSeenAt;
        [JsonProperty("metadata")] internal readonly Dictionary<string, string> metaData;
        [JsonProperty("name")] internal readonly string name;
        [JsonProperty("nickname")] internal readonly string nickName;
        [JsonProperty("preferred_languages")] internal readonly List<string> preferredLanguages;
        [JsonProperty("profile_url")] internal readonly string profileUrl;
        [JsonProperty("require_auth_for_profile_image")] public readonly bool? requireAuthForProfileImage;
#pragma warning restore CS0649

        internal string UserId { get; private set; }

        internal UserDto() { }

        internal UserDto(SbUser inUser)
        {
            friendDiscoveryKey = inUser.FriendDiscoveryKey;
            friendName = inUser.FriendName;
            isActive = inUser.IsActive;
            isOnline = inUser.ConnectionStatus == SbUserConnectionStatus.Online;
            lastSeenAt = inUser.LastSeenAt;

            if (inUser.MetaData != null && 0 < inUser.MetaData.Count)
            {
                metaData = new Dictionary<string, string>(inUser.MetaData.Count);
                foreach (KeyValuePair<string, string> keyValuePair in inUser.MetaData)
                {
                    metaData.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            nickName = inUser.Nickname;

            if (inUser.PreferredLanguages != null && 0 < inUser.PreferredLanguages.Count)
                preferredLanguages = new List<string>(inUser.PreferredLanguages);

            profileUrl = inUser.ProfileUrl;
            _userId = inUser.UserId;
            requireAuthForProfileImage = inUser.RequireAuthForProfileImage;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            UserId = !string.IsNullOrEmpty(_userId) ? _userId : _guestId;
            UserId = UserId ?? string.Empty;
        }

        internal static UserDto DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<UserDto>(inJsonString);
        }
    }
}