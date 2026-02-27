// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetSnoozePeriodApiCommand
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
            internal bool snoozeEnabled;
            internal long snoozeStartTimestamp;
            internal long snoozeEndTimestamp;

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
                            case "snooze_enabled": snoozeEnabled = JsonStreamingHelper.ReadBool(reader); break;
                            case "snooze_start_ts": snoozeStartTimestamp = JsonStreamingHelper.ReadLong(reader); break;
                            case "snooze_end_ts": snoozeEndTimestamp = JsonStreamingHelper.ReadLong(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }
        }
    }
}