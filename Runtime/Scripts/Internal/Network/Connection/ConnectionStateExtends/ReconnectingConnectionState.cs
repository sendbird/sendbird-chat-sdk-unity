// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections;

namespace Sendbird.Chat
{
    internal sealed class ReconnectingConnectionState : ConnectionStateAbstract
    {
        internal override ConnectionStateInternalType StateType => ConnectionStateInternalType.Reconnecting;
        private readonly CommandRouter _commandRouterRef = null;
        private SbUserHandler _connectCompletionHandler = null;
        private string _connectingWsHost;
        private string _connectingUserId;
        private string _connectingAuthToken;
        private string _connectingSessionKey;
        private float _retryBaseInterval = ReconnectionContext.DEFAULT_RETRY_BASE_INTERVAL;
        private float _retryMaxInterval = ReconnectionContext.DEFAULT_RETRY_MAX_INTERVAL;
        private float _retryIntervalMultiplier = ReconnectionContext.DEFAULT_RETRY_INTERVAL_MULTIPLIER;
        private int _retryMaxCount = ReconnectionContext.DEFAULT_RETRY_MAX_COUNT;
        private int _retryCount = 0;
        private CoroutineJob _reconnectingCoroutineJob;
        private CoroutineJob _timeoutCoroutineJob;

        internal ReconnectingConnectionState(ConnectionManagerContext inConnectionManagerContext, CommandRouter inCommandRouter) : base(inConnectionManagerContext)
        {
            _commandRouterRef = inCommandRouter;
        }

        internal override void Enter(ParamsAbstract inParams)
        {
            base.Enter(inParams);

            _connectingWsHost = connectionManagerContextRef.ReconnectionContext.WsHostUrl;
            _connectingUserId = connectionManagerContextRef.ReconnectionContext.UserId;
            _connectingAuthToken = connectionManagerContextRef.ReconnectionContext.AuthToken;
            _connectingSessionKey = connectionManagerContextRef.ReconnectionContext.SessionKey;
            _retryBaseInterval = connectionManagerContextRef.ReconnectionContext.RetryBaseInterval;
            _retryMaxInterval = connectionManagerContextRef.ReconnectionContext.RetryMaxInterval;
            _retryIntervalMultiplier = connectionManagerContextRef.ReconnectionContext.RetryIntervalMultiplier;
            _retryMaxCount = connectionManagerContextRef.ReconnectionContext.RetryMaxCount;

            ClearConnectionRetryCount();

            ConnectWebSocketAfterCloseIfConnectingOrOpen();
            StartTimeoutCoroutineAfterStopIfStarted(connectionManagerContextRef.ReconnectionContext.ConnectionTimeoutDuration);
            connectionManagerContextRef.ConnectionHandlersById.ForEachByValue(inHandler => { inHandler.OnReconnectStarted?.Invoke(); });
        }

        internal override void Exit()
        {
            if (_connectCompletionHandler != null)
            {
                SbUserHandler completionHandler = _connectCompletionHandler;
                CoroutineManager.Instance.CallOnNextFrame(() => completionHandler.Invoke(null, new SbError(SbErrorCode.ConnectionCanceled)));
                _connectCompletionHandler = null;
            }

            _connectingWsHost = null;
            _connectingUserId = null;
            _connectingAuthToken = null;
            _connectingSessionKey = null;

            if (_timeoutCoroutineJob != null)
            {
                CoroutineManager.Instance.StopCoroutine(_timeoutCoroutineJob);
                _timeoutCoroutineJob = null;
            }

            if (_reconnectingCoroutineJob != null)
            {
                CoroutineManager.Instance.StopCoroutine(_reconnectingCoroutineJob);
                _reconnectingCoroutineJob = null;
            }

            base.Exit();
        }

        internal override void Connect(string inWsHost, string inUserId, string inAuthToken, SbUserHandler inCompletionHandler, long inTimeoutDuration)
        {
            base.Connect(inWsHost, inUserId, inAuthToken, inCompletionHandler, inTimeoutDuration);
            if (_connectingUserId == inUserId)
            {
                _connectCompletionHandler += inCompletionHandler;
                return;
            }

            InvokeAndSetNullToCompletionHandler(null, new SbError(SbErrorCode.ConnectionCanceled));

            void OnChangeConnectingState()
            {
                ConnectingConnectionState.Params connectingParams = new ConnectingConnectionState.Params(inWsHost, inUserId, inAuthToken, inCompletionHandler, inTimeoutDuration);
                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Connecting, connectingParams);
            }

            LogoutConnectionState.Params logoutParams = new LogoutConnectionState.Params(_connectingUserId, OnChangeConnectingState);
            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Logout, logoutParams);
        }

        internal override void Logout(Action inDisconnectHandler = null)
        {
            base.Logout(inDisconnectHandler);
            LogoutConnectionState.Params logoutParams = new LogoutConnectionState.Params(_connectingUserId, inDisconnectHandler);
            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Logout, logoutParams);
        }

        internal override bool Reconnect()
        {
            base.Reconnect();
            ClearConnectionRetryCount();
            return true;
        }

        internal override void OnEnterBackground()
        {
            base.OnEnterBackground();
            SbError error = new SbError(SbErrorCode.ConnectionCanceled);
            InvokeAndSetNullToCompletionHandler(null, error);

            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, new DisconnectedConnectionState.Params());
        }

        internal override void OnChangeNetworkReachability(NetworkReachabilityType inNetworkReachabilityType)
        {
            base.OnChangeNetworkReachability(inNetworkReachabilityType);
            ClearConnectionRetryCount();
        }

        private void OnWebSocketConnectResultHandler(WsClientConnectResultType inResultType, WsClientError inErrorNullable)
        {
            if (isEnteredState == false)
                return;

            Logger.Info(Logger.CategoryType.Connection, $"ReconnectingConnectionState::OnWebSocketConnectResultHandler ResultType:{inResultType}");
            switch (inResultType)
            {
                case WsClientConnectResultType.Succeeded:
                {
                    ClearConnectionRetryCount();
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
                    if (CanConnectionRetry())
                    {
                        IncreaseConnectionRetryCount();
                        ConnectWebSocketAfterCloseIfConnectingOrOpen();
                        return;
                    }

                    InvokeAndSetNullToCompletionHandler(null, new SbError(SbErrorCode.NetworkError));
                    connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, new DisconnectedConnectionState.Params());
                    return;
                }
            }
        }

        private void ConnectWebSocketAfterCloseIfConnectingOrOpen()
        {
            if (_reconnectingCoroutineJob != null)
            {
                CoroutineManager.Instance.StopCoroutine(_reconnectingCoroutineJob);
                _reconnectingCoroutineJob = null;
            }

            float delayTime = GetConnectionRetryInterval();
            _reconnectingCoroutineJob = CoroutineManager.Instance.StartCoroutine(ConnectWebSocketAfterCloseIfConnectingOrOpenCoroutine(delayTime));
        }

        private IEnumerator ConnectWebSocketAfterCloseIfConnectingOrOpenCoroutine(float inDelayTime)
        {
            yield return new WaitForSecondsYield(inDelayTime);

            WsClientStateType wsClientStateType = _commandRouterRef.GetWsClientStateType();
            if (wsClientStateType == WsClientStateType.Connecting || wsClientStateType == WsClientStateType.Open)
            {
                void OnCloseResultHandler(WsClientCloseResultType inWsClientCloseResultType)
                {
                    if (isEnteredState == false)
                        return;

                    Logger.Info(Logger.CategoryType.Connection, $"ReconnectingConnectionState::ConnectWebSocketAfterClose Close ResultType:{inWsClientCloseResultType}");
                    _commandRouterRef.ConnectWs(_connectingWsHost, _connectingUserId, _connectingAuthToken, _connectingSessionKey, OnWebSocketConnectResultHandler);
                }

                Logger.Info(Logger.CategoryType.Connection, $"ReconnectingConnectionState::ConnectWebSocketAfterClose Close ClientState:{wsClientStateType}");
                _commandRouterRef.CloseWs(OnCloseResultHandler);
            }
            else
            {
                _commandRouterRef.ConnectWs(_connectingWsHost, _connectingUserId, _connectingAuthToken, _connectingSessionKey, OnWebSocketConnectResultHandler);
            }

            _reconnectingCoroutineJob = null;
        }

        internal override void OnError(SbError inError)
        {
            base.OnError(inError);
            bool isRetryableErrorCode = inError == null || (inError.ErrorCode != SbErrorCode.InvalidAccessToken && inError.ErrorCode != SbErrorCode.SessionTokenRevoked);

            if (isRetryableErrorCode == false || CanConnectionRetry() == false)
            {
                InvokeAndSetNullToCompletionHandler(null, inError);
                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, new DisconnectedConnectionState.Params());
                connectionManagerContextRef.ConnectionHandlersById.ForEachByValue(inHandler => { inHandler.OnReconnectFailed?.Invoke(); });
                return;
            }

            IncreaseConnectionRetryCount();
            ConnectWebSocketAfterCloseIfConnectingOrOpen();
        }

        internal override void OnLogiReceived(LogiWsReceiveCommand inLogiWsReceiveCommand)
        {
            base.OnLogiReceived(inLogiWsReceiveCommand);
            if (inLogiWsReceiveCommand == null || inLogiWsReceiveCommand.hasError)
            {
                if (inLogiWsReceiveCommand != null)
                {
                    SbErrorCode errorCode = (SbErrorCode)inLogiWsReceiveCommand.errorCode;
                    if (errorCode != SbErrorCode.InvalidAccessToken && errorCode != SbErrorCode.SessionTokenRevoked && CanConnectionRetry())
                    {
                        IncreaseConnectionRetryCount();
                        ConnectWebSocketAfterCloseIfConnectingOrOpen();
                        return;
                    }

                    InvokeAndSetNullToCompletionHandler(null, new SbError(errorCode, inLogiWsReceiveCommand.errorMessage));
                }
                else
                {
                    InvokeAndSetNullToCompletionHandler(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }

                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, new DisconnectedConnectionState.Params());
                connectionManagerContextRef.ConnectionHandlersById.ForEachByValue(inHandler => { inHandler.OnReconnectFailed?.Invoke(); });
            }
            else
            {
                ConnectedConnectionState.Params connectedParams = new ConnectedConnectionState.Params(inLogiWsReceiveCommand, _connectingUserId, _connectingAuthToken, _connectCompletionHandler);
                _connectCompletionHandler = null;
                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Connected, connectedParams);
                connectionManagerContextRef.ConnectionHandlersById.ForEachByValue(inHandler => { inHandler.OnReconnectSucceeded?.Invoke(); });
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

        private IEnumerator ReconnectionTimeoutCoroutine(long inTimeoutDuration)
        {
            if (inTimeoutDuration <= 0)
            {
                Logger.Warning(Logger.CategoryType.Connection, "Reconnection timeout is 0");
                inTimeoutDuration = NetworkConfig.DEFAULT_CONNECTION_TIMEOUT_DURATION;
            }

            yield return new WaitForSecondsYield(inTimeoutDuration);

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

            _timeoutCoroutineJob = CoroutineManager.Instance.StartCoroutine(ReconnectionTimeoutCoroutine(inTimeoutDuration));
        }

        private void IncreaseConnectionRetryCount()
        {
            _retryCount++;
        }

        private void ClearConnectionRetryCount()
        {
            _retryCount = 0;
        }

        private bool CanConnectionRetry()
        {
            bool ignoreMaxCount = _retryMaxCount < 0;
            if (ignoreMaxCount)
                return true;

            return _retryCount < _retryMaxCount;
        }

        private float GetConnectionRetryInterval()
        {
            if (_retryCount == 0)
                return 0f;

            return Math.Min(_retryMaxInterval, _retryBaseInterval + (_retryCount * _retryIntervalMultiplier));
        }
    }
}