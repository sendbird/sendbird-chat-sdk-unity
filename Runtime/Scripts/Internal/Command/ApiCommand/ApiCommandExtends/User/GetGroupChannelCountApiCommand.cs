// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetGroupChannelCountApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, SbMyMemberStateFilter inState, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/group_channel_count";

                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
                
                InsertQueryParamIfNotNullOrEmpty("state", inState.ToJsonName());
            }
        }

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal int groupChannelCount;

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
                            case "group_channel_count": groupChannelCount = JsonStreamingHelper.ReadInt(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }
        }
    }
}