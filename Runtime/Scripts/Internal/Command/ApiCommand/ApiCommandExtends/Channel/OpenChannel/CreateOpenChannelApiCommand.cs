// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class CreateOpenChannelApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            private const string NAME = "name";
            private const string CHANNEL_URL = "channel_url";
            private const string COVER_URL = "cover_url";
            private const string COVER_FILE = "cover_file";
            private const string DATA = "data";
            private const string CUSTOM_TYPE = "custom_type";
            private const string OPERATOR_IDS = "operator_ids";
            private const string IS_EPHEMERAL = "is_ephemeral";

            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty(NAME)] internal string name;
                [JsonProperty(CHANNEL_URL)] internal string channelUrl;
                [JsonProperty(COVER_URL)] internal string coverUrl;
                [JsonProperty(DATA)] internal string data;
                [JsonProperty(CUSTOM_TYPE)] internal string customType;
                [JsonProperty(IS_EPHEMERAL)] internal bool isEphemeral;
                [JsonProperty(OPERATOR_IDS)] internal List<string> operatorIds;
#pragma warning restore CS0649
            }

            internal Request(SbOpenChannelCreateParams inParams, ResultHandler inResultHandler)
            {
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Open)}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
                
                if (inParams.CoverImage != null && inParams.CoverImage.IsExists())
                {
                    AddMultipartFormSectionDataIfNotNullOrEmpty(NAME, inParams.Name);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(CHANNEL_URL, inParams.ChannelUrl);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(DATA, inParams.Data);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(CUSTOM_TYPE, inParams.CustomType);
                    AddMultipartFormSectionData(IS_EPHEMERAL, inParams.IsEphemeral);
                    AddMultipartFormSectionDataIfNotNullOrEmpty(OPERATOR_IDS, inParams.OperatorUserIds);
                    AddMultipartFormSectionFileIfExists(COVER_FILE, inParams.CoverImage.GetName(), inParams.CoverImage.FullPath);
                }
                else
                {
                    Payload tempPayload = new Payload
                    {
                        name = inParams.Name,
                        channelUrl = inParams.ChannelUrl,
                        coverUrl = inParams.CoverUrl,
                        data = inParams.Data,
                        customType = inParams.CustomType,
                        isEphemeral = inParams.IsEphemeral,
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