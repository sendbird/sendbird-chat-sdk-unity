// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetMyPushTriggerOptionApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, string inChannelUrl, ResultHandler inResultHandler)
            {
                string encodedUserId = WebUtility.UrlEncode(inUserId);
                string encodedChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                Url = $"{USERS_PREFIX_URL}/{encodedUserId}/push_preference/{encodedChannelUrl}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("push_trigger_option")] internal readonly string pushTriggerOption;

            internal SbGroupChannelPushTriggerOption? GroupChannelPushTriggerOption { get; private set; } = null;

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