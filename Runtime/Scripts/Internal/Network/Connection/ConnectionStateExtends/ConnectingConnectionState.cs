// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections;

namespace Sendbird.Chat
{
    internal sealed class ConnectingConnectionState : ConnectionStateAbstract
    {
        internal class Params : ParamsAbstract
        {
            internal string WsHostUrl { get; }
            internal string UserId { get; }
            internal string AuthToken { get; }
            internal SbUserHandler ConnectCompletionHandler { get; }
            internal long TimeoutDuration { get; }

            internal Params(string inWsHostUrl, string inUserId, string inAuthToken, SbUserHandler inConnectCompletionHandler, long inTimeoutDuration)
            {
                WsHostUrl = inWsHostUrl;
                UserId = inUserId;
                AuthToken = inAuthToken;
                ConnectCompletionHandler = inConnectCompletionHandler;
                TimeoutDuration = inTimeoutDuration;
            }
        }

        internal override ConnectionStateInternalType StateType => ConnectionStateInternalType.Connecting;
        private readonly CommandRouter _commandRouterRef = null;
        private string _connectingWsHost;
        private string _connectingUserId;
        private string _connectingAuthToken;
        private SbUserHandler _connectCompletionHandler;
        private CoroutineJob _timeoutCoroutineJob;
        private bool _ignoreConnectResult = false;

        internal ConnectingConnectionState(ConnectionManagerContext inConnectionManagerContext, CommandRouter inCommandRouter) : base(inConnectionManagerContext)
        {
            _commandRouterRef = inCommandRouter;
        }

        internal override void Enter(ParamsAbstract inParams)
        {
            base.Enter(inParams);
            
            if (!(inParams is Params connectingParams))
            {
                Logger.Error(Logger.CategoryType.Connection, "ConnectingConnectionState::Enter Invalid params");
                return;
            }

            StartConnectingProcess(connectingParams.WsHostUrl, connectingParams.UserId, connectingParams.AuthToken,
                                   connectingParams.ConnectCompletionHandler, connectingParams.TimeoutDuration);
        }

        internal override void Exit()
        {
            _connectingWsHost = null;
            _connectingUserId = null;
            _connectingAuthToken = null;

            if (_connectCompletionHandler != null)
            {
                SbUserHandler completionHandler = _connectCompletionHandler;
                CoroutineManager.Instance.CallOnNextFrame(() => completionHandler.Invoke(null, new SbError(SbErrorCode.ConnectionCanceled)));
                _connectCompletionHandler = null;
            }

            if (_timeoutCoroutineJob != null)
            {
                CoroutineManager.Instance.StopCoroutine(_timeoutCoroutineJob);
                _timeoutCoroutineJob = null;
            }
            
            base.Exit();
        }

        internal override void Connect(string inWsHost, string inUserId, string inAuthToken, SbUserHandler inCompletionHandler, long inTimeoutDuration)
        {
            base.Connect(inWsHost, inUserId, inAuthToken, inCompletionHandler, inTimeoutDuration);
            StartConnectingProcess(inWsHost, inUserId, inAuthToken, inCompletionHandler, inTimeoutDuration);
        }

        internal override void Logout(Action inDisconnectHandler = null)
        {
            base.Logout(inDisconnectHandler);
            if (_connectCompletionHandler != null)
            {
                SbUserHandler completionHandler = _connectCompletionHandler;
                CoroutineManager.Instance.CallOnNextFrame(() => completionHandler.Invoke(null, new SbError(SbErrorCode.ConnectionCanceled)));
                _connectCompletionHandler = null;
            }

            LogoutConnectionState.Params logoutParams = new LogoutConnectionState.Params(_connectingUserId, inDisconnectHandler);
            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Logout, logoutParams);
        }

        internal override void OnEnterBackground()
        {
            base.OnEnterBackground();
            SbError error = new SbError(SbErrorCode.ConnectionCanceled);
            InvokeAndSetNullToCompletionHandler(null, error);

            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, new DisconnectedConnectionState.Params());
        }

        private void StartConnectingProcess(string inWsHost, string inUserId, string inAuthToken, SbUserHandler inCompletionHandler, long inTimeoutDuration)
        {
            if (string.IsNullOrEmpty(inUserId) || string.IsNullOrEmpty(inWsHost))
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("UserId or WebsocketUri");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return;
            }

            if (string.IsNullOrEmpty(_connectingUserId) == false)
            {
                //Connect while connecting
                if (_connectingUserId == inUserId)
                {
                    _connectCompletionHandler += inCompletionHandler;
                    return;
                }

                InvokeAndSetNullToCompletionHandler(null, new SbError(SbErrorCode.ConnectionCanceled));
            }

            ConnectWebSocketAfterCloseIfConnectingOrOpen(inWsHost, inUserId, inAuthToken, inCompletionHandler);
            StartTimeoutCoroutineAfterStopIfStarted(inTimeoutDuration);
        }

        private void OnWebSocketConnectResultHandler(WsClientConnectResultType inResultType, WsClientError inErrorNullable)
        {
            if (isEnteredState == false || _ignoreConnectResult)
                return;

            Logger.Info(Logger.CategoryType.Connection, $"ConnectingConnectionState::OnWebSocketConnectResultHandler ResultType:{inResultType}");

            switch (inResultType)
            {
                case WsClientConnectResultType.Succeeded:
                {
                    connectionManagerContextRef.ClearConnectionRetryCount();
                    return;
                }
                case WsClientConnectResultType.InvalidParams:
                case WsClientConnectResultType.Terminated:
                {
                    SbError error = null;
                    if (inResultType == WsClientConnectResultType.InvalidParams)
                    {
                        error = SbErrorCodeExtension.CreateInvalidParameterError("UserId or WsHostUrl");
                    }
                    else
                    {
                        error = new SbError(SbErrorCode.InvalidInitialization);
                    }

                    InvokeAndSetNullToCompletionHandler(null, error);
                    connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, new DisconnectedConnectionState.Params());
                    return;
                }
                default:
                {
                    if (inErrorNullable != null)
                    {
                        Logger.Warning(Logger.CategoryType.Connection, $"ConnectingConnectionState::OnWebSocketConnectResultHandler Error:{inErrorNullable.ErrorMessage}");
                    }

                    if (!connectionManagerContextRef.CanConnectionRetry())
                    {
                        InvokeAndSetNullToCompletionHandler(null, new SbError(SbErrorCode.NetworkError));
                        connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, new DisconnectedConnectionState.Params());
                        return;
                    }

                    connectionManagerContextRef.IncreaseConnectionRetryCount();
                    ConnectWebSocketAfterCloseIfConnectingOrOpen(_connectingWsHost, _connectingUserId, _connectingAuthToken, _connectCompletionHandler);
                    return;
                }
            }
        }

        private void ConnectWebSocketAfterCloseIfConnectingOrOpen(string inWsHost, string inUserId, string inAuthToken, SbUserHandler inCompletionHandler)
        {
            void SetValuesBeforeConnect()
            {
                connectionManagerContextRef.ReconnectionContext.SetUserId(inUserId);
                connectionManagerContextRef.ReconnectionContext.SetAuthToken(inAuthToken);
                connectionManagerContextRef.ReconnectionContext.SetWsHostUrl(inWsHost);

                _connectingWsHost = inWsHost;
                _connectingUserId = inUserId;
                _connectingAuthToken = inAuthToken;
                _connectCompletionHandler = inCompletionHandler;

                connectionManagerContextRef.ClearConnectionRetryCount();
            }

            _ignoreConnectResult = false;
            WsClientStateType wsClientStateType = _commandRouterRef.GetWsClientStateType();
            if (wsClientStateType == WsClientStateType.Connecting || wsClientStateType == WsClientStateType.Open)
            {
                void OnCloseResultHandler(WsClientCloseResultType inWsClientCloseResultType)
                {
                    _ignoreConnectResult = false;
                    if (isEnteredState == false)
                        return;

                    Logger.Info(Logger.CategoryType.Connection, $"ConnectingConnectionState::ConnectWebSocketAfterClose Close ResultType:{inWsClientCloseResultType}");
                    SetValuesBeforeConnect();
                    _commandRouterRef.ConnectWs(_connectingWsHost, _connectingUserId, _connectingAuthToken, null, OnWebSocketConnectResultHandler);
                }

                Logger.Info(Logger.CategoryType.Connection, $"ConnectingConnectionState::ConnectWebSocketAfterClose Close ClientState:{wsClientStateType}");
                _ignoreConnectResult = true;
                _commandRouterRef.CloseWs(OnCloseResultHandler);
            }
            else
            {
                SetValuesBeforeConnect();
                _commandRouterRef.ConnectWs(_connectingWsHost, _connectingUserId, _connectingAuthToken, null, OnWebSocketConnectResultHandler);
            }
        }

        internal override void OnError(SbError inError)
        {
            base.OnError(inError);
            bool canRetryErrorCode = inError == null || (inError.ErrorCode != SbErrorCode.InvalidAccessToken && inError.ErrorCode != SbErrorCode.SessionTokenRevoked);

            if (!canRetryErrorCode || !connectionManagerContextRef.CanConnectionRetry())
            {
                InvokeAndSetNullToCompletionHandler(null, inError);

                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, new DisconnectedConnectionState.Params());
                return;
            }

            connectionManagerContextRef.IncreaseConnectionRetryCount();
            ConnectWebSocketAfterCloseIfConnectingOrOpen(_connectingWsHost, _connectingUserId, _connectingAuthToken, _connectCompletionHandler);
        }

        internal override void OnLogiReceived(LogiWsReceiveCommand inLogiWsReceiveCommand)
        {
            base.OnLogiReceived(inLogiWsReceiveCommand);
            if (inLogiWsReceiveCommand == null || inLogiWsReceiveCommand.hasError)
            {
                if (inLogiWsReceiveCommand != null)
                {
                    SbErrorCode errorCode = (SbErrorCode)inLogiWsReceiveCommand.errorCode;
                    if (errorCode != SbErrorCode.InvalidAccessToken && errorCode != SbErrorCode.SessionTokenRevoked && connectionManagerContextRef.CanConnectionRetry())
                    {
                        connectionManagerContextRef.IncreaseConnectionRetryCount();
                        ConnectWebSocketAfterCloseIfConnectingOrOpen(_connectingWsHost, _connectingUserId, _connectingAuthToken, _connectCompletionHandler);
                        return;
                    }

                    InvokeAndSetNullToCompletionHandler(null, new SbError(errorCode, inLogiWsReceiveCommand.errorMessage));
                }
                else
                {
                    InvokeAndSetNullToCompletionHandler(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }

                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, new DisconnectedConnectionState.Params());
            }
            else
            {
                ConnectedConnectionState.Params connectedParams = new ConnectedConnectionState.Params(inLogiWsReceiveCommand, _connectingUserId, _connectingAuthToken, _connectCompletionHandler);
                _connectCompletionHandler = null;
                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Connected, connectedParams);
            }
        }

        private void InvokeAndSetNullToCompletionHandler(SbUser inUser, SbError inError)
        {
            if (_connectCompletionHandler != null)
            {
                SbUserHandler completionHandler = _connectCompletionHandler;
                CoroutineManager.Instance.CallOnNextFrame(() => completionHandler.Invoke(inUser, inError));
            }

            _connectCompletionHandler = null;
        }

        private IEnumerator ConnectionTimeoutCoroutine(long inTimeoutDuration)
        {
            if (inTimeoutDuration <= 0)
            {
                Logger.Warning(Logger.CategoryType.Connection, "Connection timeout is 0");
                inTimeoutDuration = NetworkConfig.DEFAULT_CONNECTION_TIMEOUT_DURATION;
            }

            inTimeoutDuration = 5;
            Logger.Info(Logger.CategoryType.Connection, "TimeoutStart");
            yield return new WaitForSecondsYield(inTimeoutDuration);
            Logger.Info(Logger.CategoryType.Connection, "TimeoutEnd");

            InvokeAndSetNullToCompletionHandler(null, new SbError(SbErrorCode.LoginTimeout));

            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, new DisconnectedConnectionState.Params());
            _timeoutCoroutineJob = null;
        }

        private void StartTimeoutCoroutineAfterStopIfStarted(long inTimeoutDuration)
        {
            if (_timeoutCoroutineJob != null)
            {
                CoroutineManager.Instance.StopCoroutine(_timeoutCoroutineJob);
                _timeoutCoroutineJob = null;
            }

            _timeoutCoroutineJob = CoroutineManager.Instance.StartCoroutine(ConnectionTimeoutCoroutine(inTimeoutDuration));
        }
    }
}