// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class SetSnoozePeriodApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PutRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("snooze_enabled")] internal bool snoozeEnabled;
                [JsonProperty("snooze_start_ts")] internal long snoozeStartTimestamp;
                [JsonProperty("snooze_end_ts")] internal long snoozeEndTimestamp;
#pragma warning restore CS0649
            }

            internal Request(string inUserId, bool inSnoozeEnabled, long inStartTimestamp, long inEndTimestamp, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/push_preference";
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    snoozeEnabled = inSnoozeEnabled,
                    snoozeStartTimestamp = inStartTimestamp,
                    snoozeEndTimestamp = inEndTimestamp
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }
    }
}