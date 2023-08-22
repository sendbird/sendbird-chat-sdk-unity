// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Text;

namespace Sendbird.Chat
{
    internal class QueryHttpClientRequestParams : HttpClientRequestParamsBase
    {
        internal QueryHttpClientRequestParams(string inUrl,
                                              HttpMethodType inHttpMethodType,
                                              string inContentBody,
                                              Dictionary<string, string> inCustomHeaders = null,
                                              ResultHandler inResultHandler = null,
                                              ProgressHandler inProgressHandler = null,
                                              Dictionary<string, string> inQueryParams = null,
                                              Dictionary<string, IReadOnlyList<string>> inQueryParamsWithList = null)
            : base(inUrl, inHttpMethodType, inContentBody, inCustomHeaders, inResultHandler, inProgressHandler)
        {
            if (inQueryParams == null && inQueryParamsWithList == null)
                return;

            StringBuilder urlStringBuilder = new StringBuilder();
            urlStringBuilder.Append(inUrl);
            urlStringBuilder.Append("?");

            bool isFirstQuery = true;
            if (inQueryParams != null && 0 < inQueryParams.Count)
            {
                foreach (KeyValuePair<string, string> keyValuePair in inQueryParams)
                {
                    if (isFirstQuery)
                    {
                        isFirstQuery = false;
                    }
                    else
                    {
                        urlStringBuilder.Append("&");
                    }

                    urlStringBuilder.Append($"{keyValuePair.Key}={keyValuePair.Value}");
                }
            }

            if (inQueryParamsWithList != null && 0 < inQueryParamsWithList.Count)
            {
                foreach (KeyValuePair<string, IReadOnlyList<string>> keyValuePair in inQueryParamsWithList)
                {
                    if (isFirstQuery)
                    {
                        isFirstQuery = false;
                    }
                    else
                    {
                        urlStringBuilder.Append("&");
                    }

                    urlStringBuilder.Append($"{keyValuePair.Key}=");

                    bool isFirstValueOfList = true;
                    foreach (string valueOfList in keyValuePair.Value)
                    {
                        if (isFirstValueOfList)
                        {
                            isFirstValueOfList = false;
                        }
                        else
                        {
                            urlStringBuilder.Append(",");
                        }

                        urlStringBuilder.Append(valueOfList);
                    }
                }
            }

            Url = urlStringBuilder.ToString();
        }
    }
}