// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Text;

namespace Sendbird.Chat
{
    internal sealed class ConnectionManager
    {
        private ConnectionManagerContext _connectionManagerContext = null;
        private ConnectionStateAbstract _currentState = null;
        private readonly SendbirdChatMainContext _chatMainContextRef = null;
        private readonly ConnectionPingManager _connectionPingManager = new ConnectionPingManager();
        private readonly Dictionary<ConnectionStateInternalType, ConnectionStateAbstract> _statesByType = new Dictionary<ConnectionStateInternalType, ConnectionStateAbstract>((int)ConnectionStateInternalType.Max);
        private readonly List<IConnectionManagerEventListener> _eventListeners = new List<IConnectionManagerEventListener>();

        internal ConnectionManager(SendbirdChatMainContext inChatMainContext)
        {
            _chatMainContextRef = inChatMainContext;
        }

        internal void Initialize()
        {
            _connectionManagerContext = new ConnectionManagerContext(this, _chatMainContextRef);
            _connectionPingManager.Initialize(_chatMainContextRef.CommandRouter, OnPongTimeout);

            _statesByType.Clear();
            _statesByType.Add(ConnectionStateInternalType.Initialized, new InitializedConnectionState(_connectionManagerContext));
            _statesByType.Add(ConnectionStateInternalType.Connecting, new ConnectingConnectionState(_connectionManagerContext, _chatMainContextRef.CommandRouter));
            _statesByType.Add(ConnectionStateInternalType.Connected, new ConnectedConnectionState(_connectionManagerContext));
            _statesByType.Add(ConnectionStateInternalType.Reconnecting, new ReconnectingConnectionState(_connectionManagerContext, _chatMainContextRef.CommandRouter));
            _statesByType.Add(ConnectionStateInternalType.Disconnected, new DisconnectedConnectionState(_connectionManagerContext, _chatMainContextRef.CommandRouter));
            _statesByType.Add(ConnectionStateInternalType.Logout, new LogoutConnectionState(_connectionManagerContext, _chatMainContextRef.CommandRouter));

            ChangeState(ConnectionStateInternalType.Initialized);
        }

        internal void Terminate()
        {
            _currentState = null;
            _eventListeners.Clear();
            _statesByType.Clear();
            _connectionPingManager.Terminate();
        }

        internal void Update()
        {
            _connectionPingManager.Update();
        }

        internal void InsertEventListener(IConnectionManagerEventListener inConnectionManagerEventListener)
        {
            _eventListeners.AddIfNotContains(inConnectionManagerEventListener);
        }

        internal void RemoveEventListener(IConnectionManagerEventListener inConnectionManagerEventListener)
        {
            _eventListeners.RemoveIfContains(inConnectionManagerEventListener);
        }

        internal void Connect(string inUserId, string inAuthToken = null, string inWsHost = null, SbUserHandler inCompletionHandler = null)
        {
            if (_currentState == null || _chatMainContextRef == null)
            {
                Logger.Error(Logger.CategoryType.Connection, $"ConnectionManager::Connect CurrentState or ChatContext is null");
                SbError error = new SbError(SbErrorCode.InvalidInitialization);
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return;
            }

            if (string.IsNullOrEmpty(inUserId))
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("UserId");
                Logger.Error(Logger.CategoryType.Connection, $"ConnectionManager::Connect {error.ErrorMessage}");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return;
            }

            if (string.IsNullOrEmpty(inWsHost))
            {
                inWsHost = NetworkConfig.GetDefaultWsHost(_chatMainContextRef.ApplicationId);
            }

            string webSocketUri = CreateWebSocketUri(inWsHost, inUserId, inAuthToken);
            if (string.IsNullOrEmpty(webSocketUri))
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("WebSocketUri");
                Logger.Error(Logger.CategoryType.Connection, $"ConnectionManager::Connect {error.ErrorMessage}");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return;
            }

            Logger.Info(Logger.CategoryType.Connection, $"ConnectionManager::Connect current state {_currentState.StateType}");
            int timeoutDuration = _chatMainContextRef.NetworkConfig.ConnectionTimeout + _chatMainContextRef.NetworkConfig.WebsocketResponseTimeout;
            _connectionManagerContext.SetConnectionTimeoutDuration(timeoutDuration);

            _currentState.Connect(webSocketUri, inUserId, inAuthToken, inCompletionHandler, timeoutDuration);
        }

        internal bool Reconnect()
        {
            if (_currentState != null)
                return _currentState.Reconnect();

            return false;
        }

        internal void Disconnect(SbDisconnectHandler inDisconnectHandler = null)
        {
            void OnLogoutCompletionHandler()
            {
                inDisconnectHandler?.Invoke();
            }

            if (_currentState == null)
            {
                CoroutineManager.Instance.CallOnNextFrame(OnLogoutCompletionHandler);
                return;
            }

            _currentState.Logout(OnLogoutCompletionHandler);
        }

        internal void ChangeState(ConnectionStateInternalType inConnectionStateInternalType, ConnectionStateAbstract.ParamsAbstract inParams = null)
        {
            if (inConnectionStateInternalType == ConnectionStateInternalType.None)
            {
                Logger.Error(Logger.CategoryType.Connection, $"ConnectionManager::ChangeState Invalid state type");
                return;
            }

            ConnectionStateInternalType prevStateType = ConnectionStateInternalType.None;
            if (_currentState != null)
            {
                prevStateType = _currentState.StateType;
                _currentState.Exit();
                _currentState = null;
            }

            Logger.Info(Logger.CategoryType.Connection, $"ConnectionManager::ChangeState {prevStateType} -> {inConnectionStateInternalType}");
            if (_statesByType.TryGetValue(inConnectionStateInternalType, out _currentState) == false)
            {
                Logger.Error(Logger.CategoryType.Connection, $"ConnectionManager::ChangeState {inConnectionStateInternalType} state is null");
                return;
            }

            _currentState.Enter(inParams);

            _connectionPingManager.OnChangeConnectionState(_currentState.StateType, inParams);
            _eventListeners.ForEach(inEventListener => { inEventListener.OnChangeConnectionState(_currentState.StateType, inParams); });
        }

        internal ConnectionStateInternalType GetConnectionStateType()
        {
            return _currentState != null ? _currentState.StateType : ConnectionStateInternalType.None;
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
                uriStringBuilder.Append($"&user_id={inUserId}");

                if (string.IsNullOrEmpty(inAuthToken) == false)
                    uriStringBuilder.Append($"&access_token={inAuthToken}");

                if (string.IsNullOrEmpty(inSessionKey) == false)
                    uriStringBuilder.Append($"&key={inSessionKey}");

                uriStringBuilder.Append($"&pv={_chatMainContextRef.PlatformVersion}");
                uriStringBuilder.Append($"&ai={_chatMainContextRef.ApplicationId}");
                uriStringBuilder.Append($"&av={_chatMainContextRef.CustomerAppVersion}");
                uriStringBuilder.Append($"&o={_chatMainContextRef.OsName}");
                uriStringBuilder.Append("&include_extra_data=premium_feature_list,file_upload_size_limit,application_attributes,emoji_hash");
                uriStringBuilder.Append($"&{ConnectionHeaders.SB_USER_AGENT.Name}={ConnectionHeaders.SB_USER_AGENT.Value}");
            }

            return uriStringBuilder.ToString();
        }

        internal void OnEnterForeground()
        {
            _currentState.OnEnterForeground();
            _connectionPingManager.OnChangeSdkActiveState(true);
        }

        internal void OnEnterBackground()
        {
            _connectionPingManager.OnChangeSdkActiveState(false);
            _currentState.OnEnterBackground();
        }

        internal void OnChangeNetworkReachability(NetworkReachabilityType inNetworkReachabilityType)
        {
            _currentState.OnChangeNetworkReachability(inNetworkReachabilityType);
        }

        private void OnPongTimeout()
        {
            Logger.Info(Logger.CategoryType.Connection, $"ConnectionManager::OnPongTimeout");
            SbError error = new SbError(SbErrorCode.NetworkError, "Pong timed out.");
            _currentState?.OnError(error);
        }

        internal void OnWsError(SbError inError)
        {
            Logger.Info(Logger.CategoryType.Connection, $"ConnectionManager::OnWsError");
            _currentState?.OnError(inError);
        }

        internal void OnReceiveWsEventCommand(WsReceiveCommandAbstract inWsReceiveCommand)
        {
            if (inWsReceiveCommand == null)
            {
                Logger.Warning(Logger.CategoryType.Command, $"ConnectionManager::OnReceiveWsEventCommand received command is null");
                return;
            }

            if (inWsReceiveCommand is LogiWsReceiveCommand logiWsCommand)
            {
                _currentState?.OnLogiReceived(logiWsCommand);
            }

            _connectionPingManager.OnReceiveWsEventCommand();
        }

        internal void OnChangeSessionKey(string inSessionKey)
        {
            _connectionManagerContext.ReconnectionContext.SetSessionKey(inSessionKey);
            _currentState?.OnChangeAuthTokenOrSessionKey();
        }

        internal void OnChangeSessionToken(string inSessionToken)
        {
            _connectionManagerContext.ReconnectionContext.SetAuthToken(inSessionToken);
            _currentState?.OnChangeAuthTokenOrSessionKey();
        }

        internal void OnSessionError(SbErrorCode inErrorCode)
        {
            _currentState?.OnSessionError(inErrorCode);
        }

        internal void AddConnectionHandler(string inIdentifier, SbConnectionHandler inConnectionHandler)
        {
            _connectionManagerContext.AddConnectionHandler(inIdentifier, inConnectionHandler);
        }

        internal void RemoveConnectionHandler(string inIdentifier)
        {
            _connectionManagerContext.RemoveConnectionHandler(inIdentifier);
        }

        internal void RemoveAllConnectionHandlers()
        {
            _connectionManagerContext.RemoveAllConnectionHandlers();
        }
    }
}