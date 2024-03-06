// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class SetDoNotDisturbApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PutRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("do_not_disturb")] internal bool enable;
                [JsonProperty("start_hour")] internal int startHour;
                [JsonProperty("start_min")] internal int startMinute;
                [JsonProperty("end_hour")] internal int endHour;
                [JsonProperty("end_min")] internal int endMinute;
                [JsonProperty("timezone")] internal string timezone;
#pragma warning restore CS0649
            }

            internal Request(string inUserId, bool inEnable, int inStartHour, int inStartMinute, int inEndHour, int inEndMinute, string inTimezone, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/push_preference";
                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    enable = inEnable,
                    startHour = inStartHour,
                    startMinute = inStartMinute,
                    endHour = inEndHour,
                    endMinute = inEndMinute,
                    timezone = inTimezone,
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }
    }
}