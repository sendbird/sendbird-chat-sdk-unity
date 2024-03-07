// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    internal class CommandRouter : IWsClientEventListener
    {
        private readonly WsClient _wsClient = new WsClient();
        private readonly IHttpClient _apiClient = new UnityHttpClient();
        private readonly SendbirdChatMainContext _chatMainContextRef = null;
        private string _sessionKey = null;
        private readonly Dictionary<string, WsCommandAckTimer> _commandAckTimersByRequestId = new Dictionary<string, WsCommandAckTimer>();
        private readonly Queue<WsSendCommandAbstract> _wsCommandSendQueue = new Queue<WsSendCommandAbstract>();
        private WsSendCommandAbstract _sendingWsCommand = null;
        private readonly Queue<ApiCommandAbstract.Request> _apiCommandRequestQueue = new Queue<ApiCommandAbstract.Request>();
        private ApiCommandAbstract.Request _requestingApiCommand = null;
        private ConnectionStateInternalType _connectionStateInternalType = ConnectionStateInternalType.None;

        private readonly List<ICommandRouterEventListener> _eventListeners = new List<ICommandRouterEventListener>();

        internal CommandRouter(SendbirdChatMainContext inSendbirdChatMainContext)
        {
            _chatMainContextRef = inSendbirdChatMainContext;
        }

        internal void Initialize()
        {
            Terminate();

            _connectionStateInternalType = ConnectionStateInternalType.None;
            _sendingWsCommand = null;
            _requestingApiCommand = null;
            _wsClient.Initialize(this);
        }

        internal void Terminate()
        {
            _connectionStateInternalType = ConnectionStateInternalType.None;
            _sendingWsCommand = null;
            _requestingApiCommand = null;

            _wsClient.Terminate();
        }

        internal void Update()
        {
            RequestApiCommandFromQueueIfAble();
            SendWsCommandFromQueueIfAble();
            _wsClient.Update();
        }

        internal void InsertEventListener(ICommandRouterEventListener inCommandRouterEventListener)
        {
            _eventListeners.AddIfNotContains(inCommandRouterEventListener);
        }

        internal void RemoveEventListener(ICommandRouterEventListener inCommandRouterEventListener)
        {
            _eventListeners.RemoveIfContains(inCommandRouterEventListener);
        }

        internal void SetApiHost(string inApiHost)
        {
            _apiClient.SetHost(inApiHost);
        }

        internal void RequestApiCommand(ApiCommandAbstract.Request inApiRequest)
        {
            if (inApiRequest == null || string.IsNullOrEmpty(inApiRequest.Url))
            {
                Logger.Warning(Logger.CategoryType.Command, $"RequestApiCommand invalid type:{inApiRequest?.GetType()}");
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("ApiPath");
                inApiRequest?.InvokeResult(null, error, inIsCanceled: false);
                return;
            }

            if (_apiClient == null || _apiClient.IsValid() == false)
            {
                Logger.Warning(Logger.CategoryType.Command, $"RequestApiCommand invalid type:{inApiRequest.GetType()}");
                inApiRequest.InvokeResult(null, new SbError(SbErrorCode.InvalidInitialization), inIsCanceled: false);
                return;
            }

            _apiCommandRequestQueue.Enqueue(inApiRequest);
        }

        internal bool CancelRequestingApiCommand(string inRequestId)
        {
            if (_requestingApiCommand?.RequestId == inRequestId)
            {
                _apiClient.AbortIfRequesting();
                return true;
            }
            else if (0 < _apiCommandRequestQueue.Count)
            {
                bool containsRequest = _apiCommandRequestQueue.Any(inRequest => inRequest.RequestId == inRequestId);
                if (containsRequest)
                {
                    Queue<ApiCommandAbstract.Request> tempQueue = new Queue<ApiCommandAbstract.Request>(_apiCommandRequestQueue);
                    _apiCommandRequestQueue.Clear();
                    foreach (ApiCommandAbstract.Request request in tempQueue)
                    {
                        _apiCommandRequestQueue.Enqueue(request);
                    }

                    return true;
                }
            }

            return false;
        }

        private void RequestApiCommandFromQueueIfAble()
        {
            if (_requestingApiCommand != null || _apiCommandRequestQueue.Count <= 0)
                return;

            ApiCommandAbstract.Request peekedApiCommandRequest = _apiCommandRequestQueue.Peek();
            if (peekedApiCommandRequest.IsLoginRequired)
            {
                if (_connectionStateInternalType == ConnectionStateInternalType.Logout ||
                    _connectionStateInternalType == ConnectionStateInternalType.Initialized)
                {
                    _requestingApiCommand = _apiCommandRequestQueue.Dequeue();
                    _requestingApiCommand.InvokeResult(null, SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR, false);
                    return;
                }
                else if (_connectionStateInternalType == ConnectionStateInternalType.Connecting)
                {
                    return;
                }
            }

            if (peekedApiCommandRequest.IsSessionKeyRequired && string.IsNullOrEmpty(_sessionKey) && string.IsNullOrEmpty(peekedApiCommandRequest.OverrideSessionKey))
                return;

            _requestingApiCommand = _apiCommandRequestQueue.Dequeue();
            HttpClientRequestParamsBase requestParams = CreateHttpRequestParams(_requestingApiCommand, OnRequestResultHandler);
            _apiClient.Request(requestParams);
        }

        private void OnRequestResultHandler(HttpResultType inHttpResultType, string inResponseOrError)
        {
            if (_requestingApiCommand == null)
            {
                Logger.Warning(Logger.CategoryType.Http, $"OnRequestResultHandler request is null\nResponseOrError:{inResponseOrError}");
                return;
            }

            ApiCommandAbstract.Request request = _requestingApiCommand;
            _requestingApiCommand = null;

            if (inHttpResultType == HttpResultType.Canceled)
            {
                Logger.Info(Logger.CategoryType.Http, $"OnRequestResultHandler type:{request.GetType()} Canceled");
                request.InvokeResult(null, new SbError(SbErrorCode.UnknownError), inIsCanceled: true);
                return;
            }
            else if (inHttpResultType != HttpResultType.Succeeded)
            {
                string errorMessage = inResponseOrError ?? string.Empty;
                Logger.Warning(Logger.CategoryType.Http, $"OnRequestResultHandler type:{request.GetType()}\n error:{errorMessage}");
                request.InvokeResult(null, new SbError(SbErrorCode.NetworkError, errorMessage), inIsCanceled: false);
                return;
            }

            string responseJsonString = inResponseOrError;
            SbError error = TryConvertJsonToError(responseJsonString);
            if (error != null)
            {
                Logger.Warning(Logger.CategoryType.Command, $"OnRequestResultHandler type:{request.GetType()}\n ErrorCode:{error.ErrorCode}\n ErrorMessage:{error.ErrorMessage}");

                if (request.IsSessionKeyRequired && error.ErrorCode.IsSessionError())
                {
                    _chatMainContextRef.SessionManager.OnReceiveSessionError(error.ErrorCode);
                    if (error.ErrorCode.IsSessionErrorThatNeedRefresh())
                    {
                        _apiCommandRequestQueue.InsertFirst(request);
                        return;
                    }
                }

                request.InvokeResult(null, error, inIsCanceled: false);
                return;
            }

            Logger.Info(Logger.CategoryType.Http, $"type:{request.GetType()} Result\n text:{responseJsonString}");

            ApiCommandAbstract.Response responseAbstract = null;
            Type responseType = request.ResponseType;
            if (responseType != null)
            {
                responseAbstract = NewtonsoftJsonExtension.DeserializeObjectIgnoreException(responseJsonString, responseType) as ApiCommandAbstract.Response;

                if (responseAbstract == null)
                {
                    request.InvokeResult(null, new SbError(SbErrorCode.MalformedData, $"Parsed command ${responseType.Name} is not a ApiResponse."), inIsCanceled: false);
                    return;
                }

                responseAbstract.OnResponseAfterDeserialize(responseJsonString);
            }

            request.InvokeResult(responseAbstract, null, inIsCanceled: false);
        }

        private HttpClientRequestParamsBase CreateHttpRequestParams(ApiCommandAbstract.Request inApiRequest, HttpClientRequestParamsBase.ResultHandler inResultHandler)
        {
            HttpClientRequestParamsBase requestParams = null;
            if (inApiRequest is ApiCommandAbstract.MultipartRequest multipartRequest && multipartRequest.IsMultipartForms())
            {
                requestParams = new MultipartHttpClientRequestParams(multipartRequest.Url, multipartRequest.HttpMethodType, multipartRequest.ContentBody, multipartRequest.MultipartForms,
                                                                     multipartRequest.BoundaryOfMultipartForm, multipartRequest.CustomHeaders, inResultHandler, multipartRequest.OnUploadProgress);
            }
            else if (inApiRequest is ApiCommandAbstract.QueryRequest queryRequest)
            {
                requestParams = new QueryHttpClientRequestParams(queryRequest.Url, queryRequest.HttpMethodType, queryRequest.ContentBody, queryRequest.CustomHeaders,
                                                                 inResultHandler, queryRequest.OnUploadProgress, queryRequest.QueryParams, queryRequest.QueryParamsWithList);
            }
            else
            {
                requestParams = new HttpClientRequestParamsBase(inApiRequest.Url, inApiRequest.HttpMethodType, inApiRequest.ContentBody, inApiRequest.CustomHeaders,
                                                                inResultHandler, inApiRequest.OnUploadProgress);
            }

            requestParams.InsertCustomHeader(ConnectionHeaders.CONTENT_TYPE_NAME, inApiRequest.ContentTypeValue);
            requestParams.InsertCustomHeader(ConnectionHeaders.ACCEPT.Name, ConnectionHeaders.ACCEPT.Value);
            requestParams.InsertCustomHeader(ConnectionHeaders.USER_AGENT.Name, ConnectionHeaders.USER_AGENT.Value);
            requestParams.InsertCustomHeader(ConnectionHeaders.SB_USER_AGENT.Name, ConnectionHeaders.SB_USER_AGENT.Value);
            requestParams.InsertCustomHeader(ConnectionHeaders.REQUEST_SENT_TIMESTAMP.Name, ConnectionHeaders.REQUEST_SENT_TIMESTAMP.Value);
            string valueOfSendbirdName = ConnectionHeaders.BuildValueOfSendbirdName(_chatMainContextRef.ApplicationId, _chatMainContextRef.CustomerAppVersion);
            requestParams.InsertCustomHeader(ConnectionHeaders.SENDBIRD_NAME, valueOfSendbirdName);

            if (inApiRequest.IsSessionKeyRequired)
            {
                string sessionKey = inApiRequest.OverrideSessionKey ?? _sessionKey;
                requestParams.InsertCustomHeader(ConnectionHeaders.SESSION_KEY_NAME, sessionKey);
            }


            return requestParams;
        }

        private SbError TryConvertJsonToError(string inJsonString)
        {
            if (string.IsNullOrEmpty(inJsonString))
            {
                return new SbError(SbErrorCode.MalformedData, $"Response string is null or empty.");
            }

            JObject jObject = null;
            try
            {
                jObject = JObject.Parse(inJsonString);
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.Http, $"TryConvertJsonToError invalid format json:{inJsonString} exception:{exception.Message}");
                return new SbError(SbErrorCode.MalformedData, $"Invalid response:{inJsonString}.");
            }

            ErrorApiCommand.Response errorCommand = ErrorApiCommand.Response.TryConvertJsonToResponse(jObject);
            if (errorCommand != null && errorCommand.IsError())
            {
                return new SbError(errorCommand.GetErrorCode(), errorCommand.GetMessage());
            }

            return null;
        }

        internal void ConnectWs(string inWsHost, string inUserId, string inAccessToken = null, string inSessionKey = null, WsClient.WsConnectResultHandler inResultHandler = null)
        {
            string uri = CreateWebSocketUri(inWsHost, inUserId, inAccessToken, inSessionKey);
            WsClientConnectParams wsClientConnectParams = new WsClientConnectParams(uri);
            {
                wsClientConnectParams.CustomHeaders.Add(ConnectionHeaders.USER_AGENT.Name, ConnectionHeaders.USER_AGENT.Value);
                wsClientConnectParams.CustomHeaders.Add(ConnectionHeaders.REQUEST_SENT_TIMESTAMP.Name, ConnectionHeaders.REQUEST_SENT_TIMESTAMP.Value);
            }

            _wsClient.Connect(wsClientConnectParams, inResultHandler);
        }

        internal void SendWsCommand(WsSendCommandAbstract inWsSendCommand)
        {
            if (inWsSendCommand == null)
            {
                Logger.Error(Logger.CategoryType.Command, $"SendWsCommand is null.");
                return;
            }

            if (_wsClient == null)
            {
                SbError error = new SbError(SbErrorCode.InvalidInitialization);
                Logger.Warning(Logger.CategoryType.Command, $"SendWsCommand invalid type:{inWsSendCommand.GetType()}");
                inWsSendCommand.SendCompletionHandler?.Invoke(error);
                inWsSendCommand.AckCompletionHandler?.Invoke(null, error);
                return;
            }

            _wsCommandSendQueue.Enqueue(inWsSendCommand);
        }

        private void SendWsCommandFromQueueIfAble()
        {
            if (_wsCommandSendQueue.Count <= 0 && _commandAckTimersByRequestId.Count <= 0)
                return;

            if (_connectionStateInternalType == ConnectionStateInternalType.Logout ||
                _connectionStateInternalType == ConnectionStateInternalType.Initialized)
            {
                FlushAllWsSendCommandQueueWithError(SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR);
                return;
            }
            else if (_connectionStateInternalType == ConnectionStateInternalType.Disconnected)
            {
                FlushAllWsSendCommandQueueWithError(SbErrorCodeExtension.WEB_SOCKET_CONNECTION_CLOSED_ERROR);
                return;
            }
            else if (_connectionStateInternalType != ConnectionStateInternalType.Connected)
            {
                return;
            }

            if (_sendingWsCommand == null && 0 < _wsCommandSendQueue.Count)
            {
                void OnSendResult(WsClientSendResultType inSendResultType, WsClientError inWsClientErrorNullable)
                {
                    if (inSendResultType != WsClientSendResultType.Succeeded)
                    {
                        SbError error = inWsClientErrorNullable == null ? new SbError(SbErrorCode.RequestFailed) : new SbError(SbErrorCode.RequestFailed, inWsClientErrorNullable.ErrorMessage);
                        _sendingWsCommand.SendCompletionHandler?.Invoke(error);
                        _eventListeners.ForEach(inEventListener => { inEventListener.OnWsError(error); });
                    }
                    else
                    {
                        _sendingWsCommand.SendCompletionHandler?.Invoke(null);
                    }

                    _sendingWsCommand = null;
                }

                _sendingWsCommand = _wsCommandSendQueue.Dequeue();
                if (_sendingWsCommand.CommandType.IsAckRequired() && _sendingWsCommand.AckCompletionHandler != null)
                {
                    StartAckTimer(_sendingWsCommand.ReqId, _sendingWsCommand.AckCompletionHandler);
                }

                _wsClient.Send(_sendingWsCommand.ToJsonString(), OnSendResult);
            }
        }

        internal void CloseWs(Action<WsClientCloseResultType> inResultHandler = null)
        {
            _wsClient.Close(inResultHandler);
        }

        internal WsClientStateType GetWsClientStateType()
        {
            return _wsClient.StateType;
        }

        private string CreateWebSocketUri(string inWebSocketHostUrl, string inUserId, string inAuthToken = null, string inSessionKey = null)
        {
            if (string.IsNullOrEmpty(inWebSocketHostUrl) || string.IsNullOrEmpty(inUserId) || _chatMainContextRef == null)
            {
                Logger.Error(Logger.CategoryType.Connection, "ConnectionManager::CreateWebSocketUri HostUrl or UserId or ChatContext is null");
                return null;
            }

            StringBuilder uriStringBuilder = new StringBuilder(inWebSocketHostUrl);
            {
                uriStringBuilder.Append($"/?p={_chatMainContextRef.PlatformName}");
                uriStringBuilder.Append($"&user_id={WebUtility.UrlEncode(inUserId)}");

                if (string.IsNullOrEmpty(inAuthToken) == false)
                    uriStringBuilder.Append($"&access_token={WebUtility.UrlEncode(inAuthToken)}");

                if (string.IsNullOrEmpty(inSessionKey) == false)
                    uriStringBuilder.Append($"&key={WebUtility.UrlEncode(inSessionKey)}");

                if (_chatMainContextRef.SessionManager != null && _chatMainContextRef.SessionManager.HasSessionHandler())
                    uriStringBuilder.Append("&expiring_session=1");

                if (_chatMainContextRef.UserLocalCache)
                    uriStringBuilder.Append("&use_local_cache=1");
                
                uriStringBuilder.Append($"&pv={WebUtility.UrlEncode(_chatMainContextRef.PlatformVersion)}");
                uriStringBuilder.Append($"&ai={WebUtility.UrlEncode(_chatMainContextRef.ApplicationId)}");
                uriStringBuilder.Append($"&av={WebUtility.UrlEncode(_chatMainContextRef.CustomerAppVersion)}");
                uriStringBuilder.Append($"&o={WebUtility.UrlEncode(_chatMainContextRef.OsName)}");
                uriStringBuilder.Append("&include_extra_data=premium_feature_list,file_upload_size_limit,application_attributes,emoji_hash");
                uriStringBuilder.Append($"&{ConnectionHeaders.SB_USER_AGENT.Name}={ConnectionHeaders.SB_USER_AGENT.Value}");
                uriStringBuilder.Append($"&{ConnectionHeaders.SB_SDK_USER_AGENT.Name}={ConnectionHeaders.SB_SDK_USER_AGENT.Value}");
            }

            return uriStringBuilder.ToString();
        }

        private void StartAckTimer(string inRequestId, WsSendCommandAbstract.AckHandler inAckHandler)
        {
            if (inAckHandler == null || string.IsNullOrEmpty(inRequestId))
                return;

            if (_commandAckTimersByRequestId.ContainsKey(inRequestId))
            {
                Logger.Warning(Logger.CategoryType.Command, $"CommandRouter::AddCommandAckTimer Already contains RequestId:{inRequestId}");
                return;
            }

            WsCommandAckTimer wsCommandAckTimer = new WsCommandAckTimer(inAckHandler);
            wsCommandAckTimer.Start();
            _commandAckTimersByRequestId.Add(inRequestId, wsCommandAckTimer);
        }

        private bool EndAckTimerIfStartedTimerByRequestId(WsReceiveCommandAbstract inAckReceiveCommand)
        {
            if (inAckReceiveCommand == null || string.IsNullOrEmpty(inAckReceiveCommand.ReqId))
                return false;

            if (_commandAckTimersByRequestId.TryGetValue(inAckReceiveCommand.ReqId, out WsCommandAckTimer commandAckTimer))
            {
                if (inAckReceiveCommand is ErrorWsReceiveCommand errorWsEventCommand)
                {
                    SbError error = new SbError(errorWsEventCommand.errorCode, errorWsEventCommand.errorMessage);
                    commandAckTimer.StopAndInvokeCompletionHandler(null, error);
                }
                else
                {
                    commandAckTimer.StopAndInvokeCompletionHandler(inAckReceiveCommand);
                }

                _commandAckTimersByRequestId.Remove(inAckReceiveCommand.ReqId);
                return true;
            }

            return false;
        }

        private void FlushAllWsSendCommandQueueWithError(SbError inError)
        {
            if (0 < _commandAckTimersByRequestId.Count)
            {
                foreach (WsCommandAckTimer commandAckTimer in _commandAckTimersByRequestId.Values)
                {
                    commandAckTimer.StopAndInvokeCompletionHandler(null, inError);
                }

                _commandAckTimersByRequestId.Clear();
            }

            if (0 < _wsCommandSendQueue.Count)
            {
                foreach (WsSendCommandAbstract sendCommand in _wsCommandSendQueue)
                {
                    sendCommand.SendCompletionHandler?.Invoke(inError);
                    sendCommand.AckCompletionHandler?.Invoke(null, inError);
                }

                _wsCommandSendQueue.Clear();
            }
        }

        private void FlushAllApiRequestCommandsWithError(SbError inError)
        {
            if (0 < _apiCommandRequestQueue.Count)
            {
                foreach (ApiCommandAbstract.Request apiCommandRequest in _apiCommandRequestQueue)
                {
                    apiCommandRequest.InvokeResult(null, inError, inIsCanceled: false);
                }

                _apiCommandRequestQueue.Clear();
            }
        }

        void IWsClientEventListener.OnErrorInOpenState(WsClientErrorType inErrorType, WsClientError inWsClientErrorNullable)
        {
            SbErrorCode errorCode = SbErrorCode.WebSocketConnectionFailed;
            if (inErrorType == WsClientErrorType.ReceivedClose || inErrorType == WsClientErrorType.SocketClosed)
            {
                errorCode = SbErrorCode.WebSocketConnectionClosed;
            }

            SbError error = inWsClientErrorNullable == null ? new SbError(errorCode) : new SbError(errorCode, inWsClientErrorNullable.ErrorMessage);
            _eventListeners.ForEach(inEventListener => { inEventListener.OnWsError(error); });
        }

        void IWsClientEventListener.OnReceive(string inReceivedMessage)
        {
            if (string.IsNullOrEmpty(inReceivedMessage))
            {
                Logger.Info(Logger.CategoryType.Command, "CommandRouter::OnReceive received message is null or empty");
                return;
            }

            WsReceiveCommandAbstract wsReceiveCommandBase = WsEventCommandFactory.CreateCommandFromReceivedMessage(inReceivedMessage);
            if (wsReceiveCommandBase == null)
            {
                Logger.Info(Logger.CategoryType.Command, $"CommandRouter::OnReceive command is null json:{inReceivedMessage}");
                return;
            }

            if (EndAckTimerIfStartedTimerByRequestId(wsReceiveCommandBase) == false)
            {
                _eventListeners.ForEach(inEventListener => { inEventListener.OnReceiveWsEventCommand(wsReceiveCommandBase); });
            }
        }

        internal void OnChangeConnectionState(ConnectionStateInternalType inChangedStateType)
        {
            _connectionStateInternalType = inChangedStateType;
            if (inChangedStateType == ConnectionStateInternalType.Logout)
            {
                FlushAllApiRequestCommandsWithError(SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR);
            }
        }

        internal void OnChangeSessionKey(string inSessionKey)
        {
            _sessionKey = inSessionKey;
        }

        internal void OnSessionError(SbErrorCode inErrorCode)
        {
            _sessionKey = null;
        }
    }
}