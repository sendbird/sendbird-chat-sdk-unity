// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UpdateUserInfoApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PutRequest
        {
            private const string NICKNAME = "nickname";
            private const string PROFILE_URL = "profile_url";
            private const string PROFILE_FILE = "profile_file";
            private const string PREFERRED_LANGUAGES = "preferred_languages";

            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty(NICKNAME)] internal string nickname;
                [JsonProperty(PROFILE_URL)] internal string profileUrl;
                [JsonProperty(PREFERRED_LANGUAGES)] internal List<string> preferredLanguages;
#pragma warning restore CS0649
            }

            internal Request(string inUserId, SbUserUpdateParams inParams, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                if (inParams.ProfileImageFile != null && inParams.ProfileImageFile.IsExists())
                {
                    AddMultipartFormSectionDataIfNotNullOrEmpty(NICKNAME, inParams.NickName);
                    AddMultipartFormSectionFileIfExists(PROFILE_FILE, inParams.ProfileImageFile.GetName(), inParams.ProfileImageFile.FullPath);
                }
                else
                {
                    Payload tempPayload = new Payload
                    {
                        nickname = inParams.NickName,
                        profileUrl = inParams.ProfileImageUrl
                    };
                    ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
                }
            }

            internal Request(string inUserId, List<string> inPreferredLanguages, ResultHandler inResultHandler)
            {
                Url = $"{USERS_PREFIX_URL}/{inUserId}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    preferredLanguages = inPreferredLanguages
                };
                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal UserDto UserDto { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                UserDto = UserDto.DeserializeFromJson(inJsonString);
            }
        }
    }
}