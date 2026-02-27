//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class UserDto
    {
        private string _userId;
        private string _guestId;
        internal string friendDiscoveryKey;
        internal string friendName;
        internal string imageUrl;
        internal bool? isActive;
        internal bool? isOnline;
        internal long? lastSeenAt;
        internal Dictionary<string, string> metaData;
        internal string name;
        internal string nickName;
        internal List<string> preferredLanguages;
        internal string profileUrl;
        internal bool? requireAuthForProfileImage;

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

        internal virtual void PostDeserialize()
        {
            UserId = !string.IsNullOrEmpty(_userId) ? _userId : _guestId;
            UserId = UserId ?? string.Empty;
        }

        internal virtual bool TryReadSubclassField(JsonTextReader inReader, string inPropName)
        {
            return false;
        }

        internal static void ReadFields(JsonTextReader inReader, UserDto inDto)
        {
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();

                if (inDto.TryReadSubclassField(inReader, propName))
                    continue;

                switch (propName)
                {
                    case "user_id": inDto._userId = JsonStreamingHelper.ReadString(inReader); break;
                    case "guest_id": inDto._guestId = JsonStreamingHelper.ReadString(inReader); break;
                    case "friend_discovery_key": inDto.friendDiscoveryKey = JsonStreamingHelper.ReadString(inReader); break;
                    case "friend_name": inDto.friendName = JsonStreamingHelper.ReadString(inReader); break;
                    case "image": inDto.imageUrl = JsonStreamingHelper.ReadString(inReader); break;
                    case "is_active": inDto.isActive = JsonStreamingHelper.ReadNullableBool(inReader); break;
                    case "is_online": inDto.isOnline = JsonStreamingHelper.ReadNullableBool(inReader); break;
                    case "last_seen_at": inDto.lastSeenAt = JsonStreamingHelper.ReadNullableLong(inReader); break;
                    case "metadata": inDto.metaData = JsonStreamingHelper.ReadStringDictionary(inReader); break;
                    case "name": inDto.name = JsonStreamingHelper.ReadString(inReader); break;
                    case "nickname": inDto.nickName = JsonStreamingHelper.ReadString(inReader); break;
                    case "preferred_languages": inDto.preferredLanguages = JsonStreamingHelper.ReadStringList(inReader); break;
                    case "profile_url": inDto.profileUrl = JsonStreamingHelper.ReadString(inReader); break;
                    case "require_auth_for_profile_image": inDto.requireAuthForProfileImage = JsonStreamingHelper.ReadNullableBool(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            inDto.PostDeserialize();
        }

        internal static UserDto ReadUserDtoFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            UserDto dto = new UserDto();
            ReadFields(inReader, dto);
            return dto;
        }

        internal static UserDto ReadUserDtoFromJsonString(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadUserDtoFromJson);
        }

        internal static List<UserDto> ReadUserDtoListFromJson(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<UserDto> list = new List<UserDto>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                UserDto dto = ReadUserDtoFromJson(inReader);
                if (dto != null)
                    list.Add(dto);
            }

            return list;
        }

        internal static List<UserDto> ReadUserDtoListFromJsonString(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, ReadUserDtoListFromJson);
        }

        internal static UserDto DeserializeUserDtoFromJson(string inJsonString)
        {
            return ReadUserDtoFromJsonString(inJsonString);
        }

        internal virtual void WriteToJson(JsonTextWriter inWriter)
        {
            inWriter.WriteStartObject();
            WriteFields(inWriter);
            inWriter.WriteEndObject();
        }

        internal void WriteFields(JsonTextWriter inWriter)
        {
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "user_id", _userId);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "friend_discovery_key", friendDiscoveryKey);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "friend_name", friendName);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "image", imageUrl);
            JsonStreamingHelper.WriteNullableBool(inWriter, "is_active", isActive);
            JsonStreamingHelper.WriteNullableBool(inWriter, "is_online", isOnline);
            JsonStreamingHelper.WriteNullableLong(inWriter, "last_seen_at", lastSeenAt);
            JsonStreamingHelper.WriteStringDictionary(inWriter, "metadata", metaData);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "name", name);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "nickname", nickName);
            JsonStreamingHelper.WriteStringList(inWriter, "preferred_languages", preferredLanguages);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "profile_url", profileUrl);
            JsonStreamingHelper.WriteNullableBool(inWriter, "require_auth_for_profile_image", requireAuthForProfileImage);
        }

        internal static void WriteListToJson(JsonTextWriter inWriter, string inName, List<UserDto> inList)
        {
            if (inList == null)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteStartArray();
            foreach (UserDto dto in inList)
            {
                dto.WriteToJson(inWriter);
            }
            inWriter.WriteEndArray();
        }
    }
}
