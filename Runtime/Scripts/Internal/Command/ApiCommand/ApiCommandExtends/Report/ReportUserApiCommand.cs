// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class ReportUserApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("channel_url")] internal string channelUrl;
                [JsonProperty("channel_type")] internal string channelType;
                [JsonProperty("report_category")] internal string reportCategory;
                [JsonProperty("report_description")] internal string reportDescription;
                [JsonProperty("reporting_user_id")] internal string reportingUserId;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, SbChannelType inChannelType, string inOffendingUserId, SbReportCategory inReportCategory,
                             string inReporterId, string inDescription, ResultHandler inResultHandler)
            {
                inOffendingUserId = WebUtility.UrlEncode(inOffendingUserId);
                Url = $"{REPORT_PREFIX_URL}/{USERS_URL_NAME}/{inOffendingUserId}";

                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    channelUrl = inChannelUrl,
                    reportCategory = inReportCategory.ToJsonName(),
                    reportDescription = inDescription,
                    reportingUserId = inReporterId,
                };

                tempPayload.channelType = inChannelType == SbChannelType.Open ? "open_channels" : "group_channels";

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }
    }
}