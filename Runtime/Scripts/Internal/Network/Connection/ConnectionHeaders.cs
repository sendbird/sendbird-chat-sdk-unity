// 
//  Copyright (c) 2022 Sendbird, Inc.
// 


using System.Net;

namespace Sendbird.Chat
{
    //https://sendbird.atlassian.net/wiki/spaces/PLAT/pages/1237680151/SDK-SERVER+Header+Login+Params+Specification
    internal class ConnectionHeaders
    {
        internal class Header
        {
            public string Name { get; }
            public string Value { get; set; }

            public Header(string inName, string inValue = "", bool inApplyUrlEncode = false)
            {
                Name = inName;
                Value = inApplyUrlEncode ? WebUtility.UrlEncode(inValue) : inValue;
            }
        }

        internal class TimestampHeader
        {
            public string Name { get; }
            public string Value => TimeUtil.GetCurrentUnixTimeMilliseconds().ToString();

            public TimestampHeader(string inName)
            {
                Name = inName;
            }
        }

        internal static readonly Header USER_AGENT = new Header("User-Agent", $"{SendbirdChatMainContext.PLATFORM_NAME.ToLower()}/{SendbirdChatMainContext.SDK_VERSION}");
        internal static readonly Header SB_USER_AGENT = new Header("SB-User-Agent", $"{SendbirdChatMainContext.PLATFORM_NAME.ToLower()}/c{SendbirdChatMainContext.SDK_VERSION}/o{SendbirdChatMainContext.OS_NAME.ToLower()}");

        internal static readonly Header SB_SDK_USER_AGENT = new Header("SB-SDK-User-Agent",
                                                                       $"main_sdk_info=chat/{SendbirdChatMainContext.PLATFORM_NAME.ToLower()}/{SendbirdChatMainContext.SDK_VERSION}" +
                                                                       $"&device_os_platform={SendbirdChatMainContext.OS_NAME.ToLower()}" +
                                                                       $"&os_version={SendbirdChatMainContext.OS_VERSION}" +
                                                                       $"&platform_version={SendbirdChatMainContext.PLATFORM_VERSION}", inApplyUrlEncode: true);

        internal static readonly Header ACCEPT = new Header("Accept", MimeType.APPLICATION_JSON);
        internal static readonly TimestampHeader REQUEST_SENT_TIMESTAMP = new TimestampHeader("Request-Sent-Timestamp");
        internal const string SENDBIRD_NAME = "SendBird";
        internal const string SESSION_KEY_NAME = "Session-Key";
        internal const string SENDBIRD_WS_TOKEN = "SENDBIRD-WS-TOKEN"; // Auth Token
        internal const string SENDBIRD_WS_AUTH = "SENDBIRD-WS-AUTH";   // Session key

        internal static string BuildValueOfSendbirdName(string inAppId, string inCustomerAppVersion)
        {
            if (inAppId == null) inAppId = string.Empty;
            if (inCustomerAppVersion == null) inCustomerAppVersion = string.Empty;

            return $"{SendbirdChatMainContext.PLATFORM_NAME},{SendbirdChatMainContext.PLATFORM_VERSION},{SendbirdChatMainContext.SDK_VERSION},{inAppId},{inCustomerAppVersion}";
        }
    }
}