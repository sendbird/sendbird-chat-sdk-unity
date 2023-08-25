// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    internal sealed class CreateGroupChannelApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            private const string ACCESS_CODE = "access_code";
            private const string CHANNEL_URL = "channel_url";
            private const string COVER_FILE = "cover_file";
            private const string COVER_URL = "cover_url";
            private const string CUSTOM_TYPE = "custom_type";
            private const string DATA = "data";
            private const string IS_BROADCAST = "is_broadcast";
            private const string IS_DISCOVERABLE = "is_discoverable";
            private const string IS_DISTINCT = "is_distinct";
            private const string IS_EPHEMERAL = "is_ephemeral";
            private const string IS_EXCLUSIVE = "is_exclusive";
            private const string IS_PUBLIC = "is_public";
            private const string IS_SUPER = "is_super";
            private const string MESSAGE_SURVIVAL_SECONDS = "message_survival_seconds";
            private const string NAME = "name";
            private const string OPERATOR_IDS = "operator_ids";
            private const string STRICT = "strict";
            private const string USER_IDS = "user_ids";

            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty(ACCESS_CODE)] internal string accessCode;
                [JsonProperty(CHANNEL_URL)] internal string channelUrl;
                [JsonProperty(COVER_URL)] internal string coverUrl;
                [JsonProperty(CUSTOM_TYPE)] internal string customType;
                [JsonProperty(DATA)] internal string data;
                [JsonProperty(IS_BROADCAST)] internal bool isBroadcast;
                [JsonProperty(IS_DISCOVERABLE)] internal bool? isDiscoverable;
                [JsonProperty(IS_DISTINCT)] internal bool isDistinct;
                [JsonProperty(IS_EPHEMERAL)] internal bool isEphemeral;
                [JsonProperty(IS_EXCLUSIVE)] internal bool isExclusive;
                [JsonProperty(IS_PUBLIC)] internal bool isPublic;
                [JsonProperty(IS_SUPER)] internal bool isSuper;
                [JsonProperty(MESSAGE_SURVIVAL_SECONDS)] internal int messageSurvivalSeconds;
                [JsonProperty(NAME)] internal string name;
                [JsonProperty(OPERATOR_IDS)] internal List<string> operatorIds;
                [JsonProperty(STRICT)] internal bool strict;
                [JsonProperty(USER_IDS)] internal List<string> userIds;
#pragma warning restore CS0649
            }

            internal Request(SbGroupChannelCreateParams inParams, ResultHandler inResultHandler)
            {
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Group)}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                if (inParams.CoverImage != null && inParams.CoverImage.IsExists())
                {
                    AddMultipartFormSectionDataIfNotNullOrEmpty(ACCESS_CODE, inParams.AccessCode);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(CHANNEL_URL, inParams.ChannelUrl);
                    AddMultipartFormSectionFileIfExists(COVER_FILE, inParams.CoverImage.GetName(), inParams.CoverImage.FullPath);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(COVER_URL, inParams.CoverUrl);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(CUSTOM_TYPE, inParams.CustomType);
                    AddMultipartFormSectionData(IS_BROADCAST, inParams.IsBroadcast);
                    AddMultipartFormSectionData(IS_DISTINCT, inParams.IsDistinct);
                    AddMultipartFormSectionData(IS_EPHEMERAL, inParams.IsEphemeral);
                    AddMultipartFormSectionData(IS_EXCLUSIVE, inParams.IsExclusive);
                    AddMultipartFormSectionData(IS_PUBLIC, inParams.IsPublic);
                    
                    if( inParams.IsPublic)
                        AddMultipartFormSectionData(IS_DISCOVERABLE, inParams.IsDiscoverable);
                    
                    AddMultipartFormSectionData(IS_SUPER, inParams.IsSuper);
                    AddMultipartFormSectionData(MESSAGE_SURVIVAL_SECONDS, inParams.MessageSurvivalSeconds);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(NAME, inParams.Name);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(DATA, inParams.Data);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(OPERATOR_IDS, inParams.OperatorUserIds);
                    AddMultipartFormSectionData(STRICT, inParams.IsStrict);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(USER_IDS, inParams.UserIds);
                }
                else
                {
                    Payload tempPayload = new Payload
                    {
                        accessCode = inParams.AccessCode,
                        channelUrl = inParams.ChannelUrl,
                        coverUrl = inParams.CoverUrl,
                        customType = inParams.CustomType,
                        data = inParams.Data,
                        isBroadcast = inParams.IsBroadcast,
                        isDistinct = inParams.IsDistinct,
                        isEphemeral = inParams.IsEphemeral,
                        isExclusive = inParams.IsExclusive,
                        isPublic = inParams.IsPublic,
                        isSuper = inParams.IsSuper,
                        messageSurvivalSeconds = inParams.MessageSurvivalSeconds,
                        name = inParams.Name,
                        strict = inParams.IsStrict,
                        userIds = inParams.UserIds,
                        operatorIds = inParams.OperatorUserIds
                    };

                    if (tempPayload.isPublic)
                        tempPayload.isDiscoverable = inParams.IsDiscoverable;
                    
                    ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
                }
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal GroupChannelDto GroupChannelDto { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                GroupChannelDto = GroupChannelDto.DeserializeFromJson(inJsonString);
            }
        }
    }
}