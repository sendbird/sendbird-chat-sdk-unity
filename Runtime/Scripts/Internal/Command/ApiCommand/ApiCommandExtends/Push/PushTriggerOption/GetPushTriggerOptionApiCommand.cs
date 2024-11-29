// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetPushTriggerOptionApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, ResultHandler inResultHandler)
            {
                string encodedUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{encodedUserId}/push_preference";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("push_trigger_option")] internal readonly string pushTriggerOption;

            internal SbPushTriggerOption? GroupChannelPushTriggerOptionNullable { get; private set; } = null;

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (string.IsNullOrEmpty(pushTriggerOption) == false)
                {
                    GroupChannelPushTriggerOptionNullable = SbPushTriggerOptionExtension.JsonNameToType(pushTriggerOption);
                }
            }
        }
    }
}