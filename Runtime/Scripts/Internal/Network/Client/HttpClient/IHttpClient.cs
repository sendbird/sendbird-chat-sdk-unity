// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal interface IHttpClient
    {
        void SetHost(string inHostUrl);
        void Request(HttpClientRequestParamsBase inRequestParams);
        void AbortIfRequesting();
        bool IsValid();
    }
}