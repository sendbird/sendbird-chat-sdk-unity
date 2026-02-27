// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetDoNotDisturbApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/push_preference";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
            }
        }

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal bool isDoNotDisturbEnable;
            internal int startHour;
            internal int startMin;
            internal int endHour;
            internal int endMin;
            internal string timezone;

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
                            case "do_not_disturb": isDoNotDisturbEnable = JsonStreamingHelper.ReadBool(reader); break;
                            case "start_hour": startHour = JsonStreamingHelper.ReadInt(reader); break;
                            case "start_min": startMin = JsonStreamingHelper.ReadInt(reader); break;
                            case "end_hour": endHour = JsonStreamingHelper.ReadInt(reader); break;
                            case "end_min": endMin = JsonStreamingHelper.ReadInt(reader); break;
                            case "timezone": timezone = JsonStreamingHelper.ReadString(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }
        }
    }
}