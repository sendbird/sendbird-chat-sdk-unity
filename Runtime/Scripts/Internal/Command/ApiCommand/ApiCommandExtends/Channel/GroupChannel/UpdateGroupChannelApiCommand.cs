// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UpdateGroupChannelApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PutRequest
        {
            private const string ACCESS_CODE = "access_code";
            private const string COVER_FILE = "cover_file";
            private const string COVER_URL = "cover_url";
            private const string CUSTOM_TYPE = "custom_type";
            private const string DATA = "data";
            private const string IS_DISCOVERABLE = "is_discoverable";
            private const string IS_DISTINCT = "is_distinct";
            private const string IS_PUBLIC = "is_public";
            private const string MESSAGE_SURVIVAL_SECONDS = "message_survival_seconds";
            private const string NAME = "name";
            private const string OPERATOR_IDS = "operator_ids";
            private const string STRICT = "strict";

            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty(ACCESS_CODE)] internal string accessCode;
                [JsonProperty(COVER_URL)] internal string coverUrl;
                [JsonProperty(CUSTOM_TYPE)] internal string customType;
                [JsonProperty(DATA)] internal string data;
                [JsonProperty(IS_DISCOVERABLE)] internal bool? isDiscoverable;
                [JsonProperty(IS_DISTINCT)] internal bool isDistinct;
                [JsonProperty(IS_PUBLIC)] internal bool isPublic;
                [JsonProperty(MESSAGE_SURVIVAL_SECONDS)] internal int messageSurvivalSeconds;
                [JsonProperty(NAME)] internal string name;
                [JsonProperty(OPERATOR_IDS)] internal List<string> operatorIds;
                [JsonProperty(STRICT)] internal bool strict;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, SbGroupChannelUpdateParams inParams, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Group)}/{inChannelUrl}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                if (inParams.CoverImage != null && inParams.CoverImage.IsExists())
                {
                    AddMultipartFormSectionDataIfNotNullOrEmpty(ACCESS_CODE, inParams.AccessCode);
                    AddMultipartFormSectionFileIfExists(COVER_FILE, inParams.CoverImage.GetName(), inParams.CoverImage.FullPath);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(COVER_URL, inParams.CoverUrl);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(CUSTOM_TYPE, inParams.CustomType);
                    AddMultipartFormSectionData(IS_DISTINCT, inParams.IsDistinct);
                    AddMultipartFormSectionData(IS_PUBLIC, inParams.IsPublic);
                    
                    if( inParams.IsPublic)
                        AddMultipartFormSectionData(IS_DISCOVERABLE, inParams.IsDiscoverable);
                    
                    AddMultipartFormSectionData(MESSAGE_SURVIVAL_SECONDS, inParams.MessageSurvivalSeconds);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(NAME, inParams.Name);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(OPERATOR_IDS, inParams.OperatorUserIds);
                }
                else
                {
                    Payload tempPayload = new Payload
                    {
                        accessCode = inParams.AccessCode,
                        coverUrl = inParams.CoverUrl,
                        customType = inParams.CustomType,
                        data = inParams.Data,
                        isDistinct = inParams.IsDistinct,
                        isPublic = inParams.IsPublic,
                        messageSurvivalSeconds = inParams.MessageSurvivalSeconds,
                        name = inParams.Name,
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