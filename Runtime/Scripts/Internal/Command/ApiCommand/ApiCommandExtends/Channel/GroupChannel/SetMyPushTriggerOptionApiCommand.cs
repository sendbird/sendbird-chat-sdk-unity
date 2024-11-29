// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class SetMyPushTriggerOptionApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PutRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("push_trigger_option")] internal string pushTriggerOption;
#pragma warning restore CS0649
            }

            internal Request(string inUserId, string inChannelUrl, SbGroupChannelPushTriggerOption inGroupChannelPushTriggerOption, ResultHandler inResultHandler)
            {
                string encodedUserId = WebUtility.UrlEncode(inUserId);
                string encodedChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{USERS_PREFIX_URL}/{encodedUserId}/push_preference/{encodedChannelUrl}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    pushTriggerOption = inGroupChannelPushTriggerOption.ToJsonName()
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("push_trigger_option")] internal readonly string pushTriggerOption;

            internal SbGroupChannelPushTriggerOption GroupChannelPushTriggerOption { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (string.IsNullOrEmpty(pushTriggerOption) == false)
                {
                    GroupChannelPushTriggerOption = SbGroupChannelPushTriggerOptionExtension.JsonNameToType(pushTriggerOption);
                }
            }
        }
    }
}