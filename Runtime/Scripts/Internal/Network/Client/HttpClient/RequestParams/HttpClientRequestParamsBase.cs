// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal class HttpClientRequestParamsBase
    {
        internal delegate void ResultHandler(HttpResultType inResultType, string inResponseOrError);
        internal delegate void ProgressHandler(ulong inUploadedBytes, ulong inTotalBytes);

        internal string Url { get; private protected set; }
        internal HttpMethodType HttpMethodType { get; }
        internal string ContentBody { get; } = null;
        internal Dictionary<string, string> CustomHeaders { get; } = new Dictionary<string, string>();
        private readonly ResultHandler _resultHandler = null;
        private readonly ProgressHandler _uploadProgressHandler = null;

        internal HttpClientRequestParamsBase(string inUrl, HttpMethodType inHttpMethodType, string inContentBody, Dictionary<string, string> inCustomHeaders = null,
                                             ResultHandler inResultHandler = null, ProgressHandler inProgressHandler = null)
        {
            Url = inUrl;
            HttpMethodType = inHttpMethodType;
            ContentBody = inContentBody;
            _resultHandler = inResultHandler;
            _uploadProgressHandler = inProgressHandler;

            if (inCustomHeaders != null && 0 < inCustomHeaders.Count)
            {
                foreach (KeyValuePair<string, string> keyValuePair in inCustomHeaders)
                {
                    CustomHeaders.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        internal void InsertCustomHeader(string inHeaderName, string inHeaderValue)
        {
            if (CustomHeaders.ContainsKey(inHeaderName))
            {
                CustomHeaders[inHeaderName] = inHeaderValue;
            }
            else
            {
                CustomHeaders.Add(inHeaderName, inHeaderValue);
            }
        }

        internal void InvokeResult(HttpResultType inResultType, string inResponseOrError)
        {
            _resultHandler?.Invoke(inResultType, inResponseOrError);
        }

        internal void InvokeProgress(ulong inUploadedBytes, ulong inTotalBytes)
        {
            inUploadedBytes = Math.Min(inUploadedBytes, inTotalBytes);
            _uploadProgressHandler?.Invoke(inUploadedBytes, inTotalBytes);
        }

        internal virtual bool IsMultipartForms()
        {
            return false;
        }
    }
}