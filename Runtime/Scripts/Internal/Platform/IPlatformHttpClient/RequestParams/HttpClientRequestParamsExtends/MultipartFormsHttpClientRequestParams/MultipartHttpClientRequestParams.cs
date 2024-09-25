// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal class MultipartHttpClientRequestParams : HttpClientRequestParamsBase
    {
        internal string BoundaryOfMultipartForms { get; } = null;
        internal List<HttpMultipartFormSectionAbstract> MultipartForms { get; } = null;

        internal MultipartHttpClientRequestParams(string inUrl,
                                                  HttpMethodType inHttpMethodType,
                                                  string inContentBody,
                                                  List<HttpMultipartFormSectionAbstract> inMultipartForms = null,
                                                  string inBoundaryOfMultipartForms = null,
                                                  Dictionary<string, string> inCustomHeaders = null,
                                                  ResultHandler inResultHandler = null,
                                                  ProgressHandler inProgressHandler = null)
            : base(inUrl, inHttpMethodType, inContentBody, inCustomHeaders, inResultHandler, inProgressHandler)
        {
            BoundaryOfMultipartForms = inBoundaryOfMultipartForms;

            if (inMultipartForms != null && 0 < inMultipartForms.Count)
            {
                MultipartForms = new List<HttpMultipartFormSectionAbstract>(inMultipartForms.Count);
                foreach (HttpMultipartFormSectionAbstract multipartFormSection in inMultipartForms)
                {
                    MultipartForms.Add(multipartFormSection);
                }
            }
        }

        internal override bool IsMultipartForms()
        {
            return MultipartForms != null && 0 < MultipartForms.Count;
        }
    }
}