// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sendbird.Chat
{
    internal abstract class ApiCommandAbstract
    {
        internal abstract class Request
        {
            protected const string GROUP_CHANNELS_URL_NAME = "group_channels";
            protected const string OPEN_CHANNELS_URL_NAME = "open_channels";
            protected const string USERS_URL_NAME = "users";
            protected const string USERS_PREFIX_URL = "v3/" + USERS_URL_NAME;
            protected const string USERS_INTERNAL_PREFIX_URL = "v3/sdk/" + USERS_URL_NAME;
            protected const string STORAGE_PREFIX_URL = "v3/storage";
            protected const string REPORT_PREFIX_URL = "v3/report";
            protected const string SEARCH_PREFIX_URL = "v3/search";
            private const string GROUP_CHANNELS_PREFIX_URL = "v3/" + GROUP_CHANNELS_URL_NAME;
            private const string GROUP_CHANNELS_INTERNAL_PREFIX_URL = "v3/sdk/" + GROUP_CHANNELS_URL_NAME;
            private const string OPEN_CHANNELS_PREFIX_URL = "v3/" + OPEN_CHANNELS_URL_NAME;
            private const string OPEN_CHANNELS_INTERNAL_PREFIX_URL = "v3/sdk/" + OPEN_CHANNELS_URL_NAME;
            internal delegate void ResultHandler(Response inResponse, SbError inError, bool inIsCanceled);
            internal abstract HttpMethodType HttpMethodType { get; }
            protected ResultHandler resultHandler;
            internal virtual string RequestId { get; } = null;
            internal virtual bool IsLoginRequired => true;
            internal string Url { get; private protected set; }
            internal string ContentTypeValue { get; private protected set; } = MimeType.APPLICATION_JSON;
            internal string ContentBody { get; private protected set; } = null;
            internal bool IsSessionKeyRequired { get; private protected set; } = true;
            internal string OverrideSessionKey { get; private protected set; } = null;
            internal Dictionary<string, string> CustomHeaders { get; } = new Dictionary<string, string>();
            internal Type ResponseType { get; private protected set; } = typeof(Response);

            protected static string ChannelTypeToUrlPrefix(SbChannelType inChannelType, bool inIsInternal = false)
            {
                if (inIsInternal == false)
                {
                    return inChannelType == SbChannelType.Open ? OPEN_CHANNELS_PREFIX_URL : GROUP_CHANNELS_PREFIX_URL;
                }

                return inChannelType == SbChannelType.Open ? OPEN_CHANNELS_INTERNAL_PREFIX_URL : GROUP_CHANNELS_INTERNAL_PREFIX_URL;
            }

            internal virtual bool IsMultipartForms()
            {
                return false;
            }

            internal virtual void OnUploadProgress(ulong inUploadedBytes, ulong inTotalBytes) { }

            internal void InvokeResult(Response inResponse, SbError inError, bool inIsCanceled)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => resultHandler?.Invoke(inResponse, inError, inIsCanceled));
            }
        }

        internal abstract class MultipartRequest : Request
        {
            internal string BoundaryOfMultipartForm { get; private set; } = null;
            internal List<HttpMultipartFormSectionAbstract> MultipartForms { get; private set; } = null;


            internal override bool IsMultipartForms()
            {
                return MultipartForms != null && 0 < MultipartForms.Count;
            }

            protected void AddMultipartFormSectionDataIfNotNullOrEmpty(string inName, string inData)
            {
                if (string.IsNullOrEmpty(inName) || string.IsNullOrEmpty(inData))
                    return;

                CreateMultipartFormsAndSetContentTypeAndBoundaryIfNotCreated();

                MultipartForms.Add(new HttpMultipartFormSectionData(inName, inData));
            }

            protected void AddMultipartFormSectionData(string inName, bool inData)
            {
                AddMultipartFormSectionDataIfNotNullOrEmpty(inName, inData.ToString());
            }

            protected void AddMultipartFormSectionData(string inName, int inData)
            {
                AddMultipartFormSectionDataIfNotNullOrEmpty(inName, inData.ToString());
            }

            protected void AddMultipartFormSectionDataIfNotNullOrEmpty(string inName, List<string> inDataList)
            {
                if (string.IsNullOrEmpty(inName) || inDataList == null || inDataList.Count <= 0)
                    return;

                CreateMultipartFormsAndSetContentTypeAndBoundaryIfNotCreated();

                StringBuilder dataStringBuilder = new StringBuilder();
                for (int index = 0; index < inDataList.Count; index++)
                {
                    dataStringBuilder.Append(inDataList[index]);
                    if (index < inDataList.Count - 1)
                    {
                        dataStringBuilder.Append(",");
                    }
                }

                MultipartForms.Add(new HttpMultipartFormSectionData(inName, dataStringBuilder.ToString()));
            }

            protected void AddMultipartFormSectionFileIfExists(string inName, string inFileName, string inFilePath)
            {
                if (string.IsNullOrEmpty(inName) || string.IsNullOrEmpty(inFilePath) || string.IsNullOrEmpty(inFileName) || File.Exists(inFilePath) == false)
                    return;

                CreateMultipartFormsAndSetContentTypeAndBoundaryIfNotCreated();
                MultipartForms.Add(new HttpMultipartFormSectionFile(inName, inFileName, inFilePath));
            }

            private void CreateMultipartFormsAndSetContentTypeAndBoundaryIfNotCreated()
            {
                if (MultipartForms == null)
                {
                    MultipartForms = new List<HttpMultipartFormSectionAbstract>();
                    BoundaryOfMultipartForm = "sendbird-boundary-" + DateTime.Now.Ticks.ToString("x");
                    ContentTypeValue = "multipart/form-data; boundary=" + BoundaryOfMultipartForm;
                }
            }
        }

        internal abstract class QueryRequest : Request
        {
            internal Dictionary<string, string> QueryParams { get; private set; }
            internal Dictionary<string, IReadOnlyList<string>> QueryParamsWithList { get; private set; }

            protected void InsertQueryParamIfNotNullOrEmpty(string inKey, int inValue)
            {
                InsertQueryParamIfNotNullOrEmpty(inKey, inValue.ToString());
            }

            protected void InsertQueryParamIfNotNullOrEmpty(string inKey, long inValue)
            {
                InsertQueryParamIfNotNullOrEmpty(inKey, inValue.ToString());
            }

            protected void InsertQueryParamIfNotNullOrEmpty(string inKey, bool inValue)
            {
                InsertQueryParamIfNotNullOrEmpty(inKey, inValue.ToString());
            }

            protected void InsertQueryParamIfNotNullOrEmpty(string inKey, string inValue)
            {
                if (string.IsNullOrEmpty(inKey) || string.IsNullOrEmpty(inValue))
                    return;

                if (QueryParams == null)
                    QueryParams = new Dictionary<string, string>();

                if (QueryParams.ContainsKey(inKey))
                {
                    QueryParams[inKey] = inValue;
                }
                else
                {
                    QueryParams.Add(inKey, inValue);
                }
            }

            protected void InsertQueryParamWithListIfNotNullOrEmpty(string inKey, IReadOnlyList<string> inValues)
            {
                if (string.IsNullOrEmpty(inKey) || inValues == null || inValues.Count <= 0)
                    return;

                if (QueryParamsWithList == null)
                    QueryParamsWithList = new Dictionary<string, IReadOnlyList<string>>();

                if (QueryParamsWithList.ContainsKey(inKey))
                {
                    QueryParamsWithList[inKey] = inValues;
                }
                else
                {
                    QueryParamsWithList.Add(inKey, inValues);
                }
            }
        }

        internal abstract class GetRequest : QueryRequest
        {
            internal override HttpMethodType HttpMethodType => HttpMethodType.Get;
        }

        internal abstract class PostRequest : MultipartRequest
        {
            internal override HttpMethodType HttpMethodType => HttpMethodType.Post;
        }

        internal abstract class DeleteRequest : Request
        {
            internal override HttpMethodType HttpMethodType => HttpMethodType.Delete;
        }

        internal abstract class PutRequest : MultipartRequest
        {
            internal override HttpMethodType HttpMethodType => HttpMethodType.Put;
        }

        internal class Response
        {
            internal virtual void OnResponseAfterDeserialize(string inJsonString) { }

            internal virtual void OnResponseAfterDeserialize(byte[] inResponseBytes)
            {
                if (inResponseBytes == null || inResponseBytes.Length == 0)
                    return;

                string jsonString = Encoding.UTF8.GetString(inResponseBytes);
                OnResponseAfterDeserialize(jsonString);
            }
        }
    }
}