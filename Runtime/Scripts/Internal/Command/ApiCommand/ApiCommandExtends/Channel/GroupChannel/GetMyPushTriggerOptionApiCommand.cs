// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

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

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal string pushTriggerOption;

            internal SbGroupChannelPushTriggerOption? GroupChannelPushTriggerOption { get; private set; } = null;

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (string.IsNullOrEmpty(inJsonString))
                    return;

                using (JsonTextReader reader = JsonStreamingPool.CreateReader(inJsonString))
                {
                    reader.Read();
                    if (reader.TokenType != JsonToken.StartObject)
                        return;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.EndObject)
                            break;

                        string propName = reader.Value as string;
                        reader.Read();
                        switch (propName)
                        {
                            case "push_trigger_option": pushTriggerOption = JsonStreamingHelper.ReadString(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(pushTriggerOption) == false)
                {
                    GroupChannelPushTriggerOption = SbGroupChannelPushTriggerOptionExtension.JsonNameToType(pushTriggerOption);
                }
            }
        }
    }
}