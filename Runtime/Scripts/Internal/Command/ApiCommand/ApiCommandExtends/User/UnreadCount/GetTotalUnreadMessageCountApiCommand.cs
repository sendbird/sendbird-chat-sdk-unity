// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetTotalUnreadMessageCountApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, List<string> inCustomTypes, SbGroupChannelSuperChannelFilter inSuperChannelFilter, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/unread_message_count";

                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamWithListIfNotNullOrEmpty("custom_types", inCustomTypes);
                InsertQueryParamIfNotNullOrEmpty("super_mode", inSuperChannelFilter.ToJsonName());
                InsertQueryParamIfNotNullOrEmpty("include_feed_channel", false);
            }
        }

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal int groupChannelUnreadCount;
            internal int feedChannelUnreadCount;

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
                            case "unread_count": groupChannelUnreadCount = JsonStreamingHelper.ReadInt(reader); break;
                            case "unread_feed_count": feedChannelUnreadCount = JsonStreamingHelper.ReadInt(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }
        }
    }
}