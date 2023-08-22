// 
//  Copyright (c) 2022 Sendbird, Inc.
// 


namespace Sendbird.Chat
{
    //https://sendbird.atlassian.net/wiki/spaces/PLAT/pages/1237680151/SDK-SERVER+Header+Login+Params+Specification
    internal class ConnectionHeaders
    {
        internal class Header
        {
            public string Name { get; }
            public string Value { get; set; }

            public Header(string inName, string inValue = "")
            {
                Name = inName;
                Value = inValue;
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

        internal static readonly Header USER_AGENT = new Header("User-Agent", $"{SendbirdChatMainContext.PLATFORM_NAME}/{SendbirdChatMainContext.SDK_VERSION}/{SendbirdChatMainContext.OS_NAME}");
        internal static readonly Header SB_USER_AGENT = new Header("SB-User-Agent", $"{SendbirdChatMainContext.PLATFORM_NAME}/c{SendbirdChatMainContext.SDK_VERSION}/o{SendbirdChatMainContext.OS_NAME}");

        internal static readonly Header SB_SDK_USER_AGENT = new Header("SB-SDK-User-Agent",
                                                                       $"main_sdk_info=chat/{SendbirdChatMainContext.PLATFORM_NAME.ToLower()}/{SendbirdChatMainContext.SDK_VERSION}" +
                                                                       $"&device_os_platform={SendbirdChatMainContext.OS_NAME.ToLower()}" +
                                                                       $"&os_version={SendbirdChatMainContext.OS_VERSION}" +
                                                                       $"&platform_version={SendbirdChatMainContext.PLATFORM_VERSION}");
        internal static readonly Header ACCEPT = new Header("Accept", MimeType.APPLICATION_JSON);
        internal static readonly TimestampHeader REQUEST_SENT_TIMESTAMP = new TimestampHeader("Request-Sent-Timestamp");
        internal const string CONTENT_TYPE_NAME = "Content-Type";
        internal const string SENDBIRD_NAME = "SendBird";
        internal const string SESSION_KEY_NAME = "Session-Key";

        internal static string BuildValueOfSendbirdName(string inAppId, string inCustomerAppVersion)
        {
            if (inAppId == null) inAppId = string.Empty;
            if (inCustomerAppVersion == null) inCustomerAppVersion = string.Empty;

            return $"Unity,{SendbirdChatMainContext.PLATFORM_VERSION},{SendbirdChatMainContext.SDK_VERSION},{inAppId},{inCustomerAppVersion}";
        }
    }
}