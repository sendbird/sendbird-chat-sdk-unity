// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class ReportMessageApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            [Serializable]
            private struct Payload
            {
#pragma warning disable CS0649
                [JsonProperty("report_category")] internal string reportCategory;
                [JsonProperty("report_description")] internal string reportDescription;
                [JsonProperty("reporting_user_id")] internal string reportingUserId;
                [JsonProperty("offending_user_id")] internal string offendingUserId;
#pragma warning restore CS0649
            }

            internal Request(string inChannelUrl, SbChannelType inChannelType, string inOffendingUserId, long inMessageId,
                             SbReportCategory inReportCategory, string inReporterId, string inDescription, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                
                if (inChannelType == SbChannelType.Open)
                {
                    Url = $"{REPORT_PREFIX_URL}/{OPEN_CHANNELS_URL_NAME}/{inChannelUrl}/messages/{inMessageId}";    
                }
                else
                {
                    Url = $"{REPORT_PREFIX_URL}/{GROUP_CHANNELS_URL_NAME}/{inChannelUrl}/messages/{inMessageId}";    
                }
                

                resultHandler = inResultHandler;

                Payload tempPayload = new Payload
                {
                    reportCategory = inReportCategory.ToJsonName(),
                    reportDescription = inDescription,
                    reportingUserId = inReporterId,
                    offendingUserId = inOffendingUserId
                };

                ContentBody = NewtonsoftJsonExtension.SerializeObjectIgnoreException(tempPayload);
            }
        }
    }
}