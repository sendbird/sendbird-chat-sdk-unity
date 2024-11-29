// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class RegisterPushTokenApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            [Serializable]
            private struct ApnsPayload
            {
#pragma warning disable CS0649
                [JsonProperty("apns_device_token")] internal string deviceToken;
                [JsonProperty("is_unique")] internal bool isUnique;
                [JsonProperty("always_push")] internal bool alwaysPush;
#pragma warning restore CS0649
            }

            [Serializable]
            private struct GcmPayload
            {
#pragma warning disable CS0649
                [JsonProperty("gcm_reg_token")] internal string deviceToken;
                [JsonProperty("is_unique")] internal bool isUnique;
                [JsonProperty("always_push")] internal bool alwaysPush;
#pragma warning restore CS0649
            }

            [Serializable]
            private struct HuaweiPayload
            {
#pragma warning disable CS0649
                [JsonProperty("huawei_device_token")] internal string deviceToken;
                [JsonProperty("is_unique")] internal bool isUnique;
                [JsonProperty("always_push")] internal bool alwaysPush;
#pragma warning restore CS0649
            }

            internal Request(string inUserId, SbPushTokenType inTokenType, string inToken, bool inIsUnique, bool inAlwaysPush, bool inInternal, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                if (inInternal)
                {
                    Url = $"{USERS_INTERNAL_PREFIX_URL}/{inUserId}/push/{inTokenType.ToJsonName()}";
                }
                else
                {
                    Url = $"{USERS_PREFIX_URL}/{inUserId}/push/{inTokenType.ToJsonName()}";
                }

                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                if (inTokenType == SbPushTokenType.Apns)
                {
                    ApnsPayload tempPayload = new ApnsPayload
                    {
                        deviceToken = inToken,
                        isUnique = inIsUnique,
                        alwaysPush = inAlwaysPush
                    };

                    ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
                }
                else if (inTokenType == SbPushTokenType.Fcm)
                {
                    GcmPayload tempPayload = new GcmPayload
                    {
                        deviceToken = inToken,
                        isUnique = inIsUnique,
                        alwaysPush = inAlwaysPush
                    };

                    ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
                }
                else if (inTokenType == SbPushTokenType.Hms)
                {
                    HuaweiPayload tempPayload = new HuaweiPayload
                    {
                        deviceToken = inToken,
                        isUnique = inIsUnique,
                        alwaysPush = inAlwaysPush
                    };

                    ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
                }
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("device_token_last_deleted_at")] internal readonly long deviceTokenLastDeletedAt;
        }
    }
}