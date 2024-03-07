// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UpdateOpenChannelApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PutRequest
        {
            private const string NAME = "name";
            private const string COVER_URL = "cover_url";
            private const string COVER_FILE = "cover_file";
            private const string DATA = "data";
            private const string CUSTOM_TYPE = "custom_type";
            private const string OPERATOR_IDS = "operator_ids";

            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty(NAME)] internal string name;
                [JsonProperty(COVER_URL)] internal string coverUrl;
                [JsonProperty(DATA)] internal string data;
                [JsonProperty(CUSTOM_TYPE)] internal string customType;
                [JsonProperty(OPERATOR_IDS)] internal List<string> operatorIds;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, SbOpenChannelUpdateParams inParams, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Open)}/{inChannelUrl}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                if (inParams.CoverImage != null && inParams.CoverImage.IsExists())
                {
                    AddMultipartFormSectionDataIfNotNullOrEmpty(NAME, inParams.Name);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(DATA, inParams.Data);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(CUSTOM_TYPE, inParams.CustomType);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(OPERATOR_IDS, inParams.OperatorUserIds);
                    AddMultipartFormSectionFileIfExists(COVER_FILE, inParams.CoverImage.GetName(), inParams.CoverImage.FullPath);
                }
                else
                {
                    Payload tempPayload = new Payload
                    {
                        name = inParams.Name,
                        coverUrl = inParams.CoverUrl,
                        data = inParams.Data,
                        customType = inParams.CustomType,
                        operatorIds = inParams.OperatorUserIds,
                    };
                    ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
                }
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal OpenChannelDto OpenChannelDto { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                OpenChannelDto = OpenChannelDto.DeserializeFromJson(inJsonString);
            }
        }
    }
}