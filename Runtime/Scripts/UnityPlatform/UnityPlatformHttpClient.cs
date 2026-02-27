// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace Sendbird.Chat
{
    internal class UnityPlatformHttpClient : IPlatformHttpClient
    {
        private readonly StringBuilder _uriStringBuilder = new StringBuilder();
        private string _host = null;
        private bool _forceAbortIfRequesting = false;

        void IPlatformHttpClient.SetHost(string inHostUrl)
        {
            _host = inHostUrl;
        }
        
        bool IPlatformHttpClient.IsValid()
        {
            return !string.IsNullOrEmpty(_host);
        }

        void IPlatformHttpClient.Request(HttpClientRequestParamsBase inRequestParams)
        {
            SendbirdChatGameObject.Instance.StartCoroutine(RequestCoroutine(inRequestParams));
        }

        void IPlatformHttpClient.AbortIfRequesting()
        {
            _forceAbortIfRequesting = true;
        }

        private IEnumerator RequestCoroutine(HttpClientRequestParamsBase inRequestParams)
        {
            _forceAbortIfRequesting = false;
            if (inRequestParams == null || string.IsNullOrEmpty(inRequestParams.Url) || string.IsNullOrEmpty(inRequestParams.HttpMethodType.ToMethodString()))
            {
                Logger.Error(Logger.CategoryType.Http, $"UnityHttpClient::RequestCoroutine invalid params");
                inRequestParams?.InvokeResult(HttpResultType.Failed, Encoding.UTF8.GetBytes("Invalid RequestParams"));
                yield break;
            }

            _uriStringBuilder.Clear();
            _uriStringBuilder.Append(_host);
            _uriStringBuilder.Append("/");
            _uriStringBuilder.Append(inRequestParams.Url);

            int autoRetryCountIfNetworkError = 0;
            SEND_WEB_REQUEST_START_LABEL:
            {
                UnityWebRequest webRequest;
                if (inRequestParams.IsMultipartForms() && inRequestParams is MultipartHttpClientRequestParams multipartHttpClientRequestParams)
                {
                    List<IMultipartFormSection> unityMultipartFormSections = HttpMultipartFormsToUnityMultipartForms(multipartHttpClientRequestParams.MultipartForms);
                    byte[] boundary = Encoding.UTF8.GetBytes(multipartHttpClientRequestParams.BoundaryOfMultipartForms);
                    webRequest = UnityWebRequest.Post(_uriStringBuilder.ToString(), unityMultipartFormSections, boundary);
                    webRequest.method = multipartHttpClientRequestParams.HttpMethodType.ToMethodString();
                }
                else
                {
                    string contentBody = inRequestParams.ContentBody;
                    webRequest = new UnityWebRequest(_uriStringBuilder.ToString(), inRequestParams.HttpMethodType.ToMethodString());
                    if (string.IsNullOrEmpty(contentBody) == false)
                    {
                        byte[] contents = Encoding.UTF8.GetBytes(contentBody);
                        webRequest.uploadHandler = new UploadHandlerRaw(contents);
                    }
                }

                webRequest.downloadHandler = new DownloadHandlerBuffer();

                if (Logger.InternalLogLevel < InternalLogLevel.Info)
                {
                    string encodedStringForLog = null;
                    if (webRequest.uploadHandler != null)
                    {
                        encodedStringForLog = Encoding.UTF8.GetString(webRequest.uploadHandler.data);
                    }

                    Logger.Verbose(Logger.CategoryType.Http, $"UnityHttpClient::RequestCoroutine uri:{_uriStringBuilder}\n string:{encodedStringForLog}");
                }
                else
                {
                    Logger.Info(Logger.CategoryType.Http, $"UnityHttpClient::RequestCoroutine uri:{_uriStringBuilder}");
                }


                if (inRequestParams.CustomHeaders != null && 0 < inRequestParams.CustomHeaders.Count)
                {
                    foreach (KeyValuePair<string, string> keyValuePair in inRequestParams.CustomHeaders)
                    {
                        webRequest.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
                    }
                }

                webRequest.timeout = 5;
                ulong uploadTotalBytes = 0;
                if (webRequest.uploadHandler != null && webRequest.uploadHandler.data != null)
                    uploadTotalBytes = (ulong)webRequest.uploadHandler.data.Length;

                webRequest.SendWebRequest();

                while (!webRequest.isDone)
                {
                    if (_forceAbortIfRequesting)
                    {
                        webRequest.Abort();
                        break;
                    }

                    inRequestParams.InvokeProgress(webRequest.uploadedBytes, uploadTotalBytes);
                    yield return null;
                }

                if (_forceAbortIfRequesting)
                {
                    webRequest.Dispose();
                    inRequestParams.InvokeResult(HttpResultType.Canceled, null);
                    yield break;
                }

                bool isNetworkError = false;
#if UNITY_2020_1_OR_NEWER
                isNetworkError = webRequest.result == UnityWebRequest.Result.ConnectionError;
#else
                isNetworkError = webRequest.isNetworkError;
#endif
                if (isNetworkError)
                {
                    Logger.Warning(Logger.CategoryType.Http, $"UnityHttpClient::RequestCoroutine Response uri:{_uriStringBuilder}\nerror:{webRequest.error}");

                    const int AUTO_RETRY_MAX = 3;
                    autoRetryCountIfNetworkError++;
                    if (autoRetryCountIfNetworkError <= AUTO_RETRY_MAX)
                    {
                        webRequest.Dispose();
                        goto SEND_WEB_REQUEST_START_LABEL;
                    }

                    string error = webRequest.error;
                    webRequest.Dispose();
                    inRequestParams.InvokeResult(HttpResultType.Failed, Encoding.UTF8.GetBytes(error ?? string.Empty));
                    yield break;
                }

                byte[] responseBytes = Array.Empty<byte>();
                if (webRequest.downloadHandler != null)
                {
                    responseBytes = webRequest.downloadHandler.data;
                    Logger.Info(Logger.CategoryType.Http, $"DownloadHandler length:{responseBytes?.Length ?? 0}");
                }

                webRequest.Dispose();
                inRequestParams.InvokeResult(HttpResultType.Succeeded, responseBytes);
            }
        }

        private List<IMultipartFormSection> HttpMultipartFormsToUnityMultipartForms(List<HttpMultipartFormSectionAbstract> inHttpMultipartFormSections)
        {
            if (inHttpMultipartFormSections == null || inHttpMultipartFormSections.Count <= 0)
            {
                Logger.Warning(Logger.CategoryType.Http, "HttpMultipartFormsToUnityMultipartForms() inHttpMultipartFormSections is null or empty.");
                return null;
            }

            List<IMultipartFormSection> multipartFormSections = new List<IMultipartFormSection>(inHttpMultipartFormSections.Count);
            foreach (HttpMultipartFormSectionAbstract multipartFormSectionAbstract in inHttpMultipartFormSections)
            {
                IMultipartFormSection iMultipartFormSection = null;
                string name = multipartFormSectionAbstract.Name;
                byte[] data = multipartFormSectionAbstract.GetBytesOfData();
                string contentType = multipartFormSectionAbstract.ContentType;

                if (multipartFormSectionAbstract is HttpMultipartFormSectionData)
                {
                    //iMultipartFormSection = new MultipartFormDataSection(name, data, contentType);
                    iMultipartFormSection = new MultipartFormDataSection(name, data);
                }
                else if (multipartFormSectionAbstract is HttpMultipartFormSectionFile fileSection)
                {
                    iMultipartFormSection = new MultipartFormFileSection(name, data, fileSection.FileName, contentType);
                }
                else
                {
                    Logger.Warning(Logger.CategoryType.Http, $"HttpMultipartFormsToUnityMultipartForms() Invalid HttpMultipartFormSectionAbstract.");
                }

                if (iMultipartFormSection != null)
                {
                    multipartFormSections.Add(iMultipartFormSection);
                }
            }

            return multipartFormSections;
        }
    }
}
