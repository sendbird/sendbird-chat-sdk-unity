// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sendbird.Chat
{
    internal class SessionManager
    {
        private delegate void SessionRefreshHandler(string inNewKey, SbError inError);

        private readonly SendbirdChatMainContext _chatMainContextRef = null;
        private string _sessionKey = null;
        private string _eKey = null;
        private string _userId = null;
        private string _authToken = null;
        private readonly List<ISessionManagerEventListener> _eventListeners = new List<ISessionManagerEventListener>();
        private SbSessionHandler _sessionHandler = null;
        private CoroutineJob _refreshSessionKeyCoroutine = null;
        private CoroutineJob _requestAuthTokenRequireCoroutine = null;
        private readonly SessionTokenRequester _sessionTokenRequester = new SessionTokenRequester();

        internal SessionManager(SendbirdChatMainContext inSendbirdChatMainContext)
        {
            _chatMainContextRef = inSendbirdChatMainContext;
        }

        internal void Terminate()
        {
            StopRefreshAuthTokenAndSessionKey();
            _eventListeners.Clear();
            _sessionHandler = null;
            _sessionKey = null;
            _eKey = null;
            _userId = null;
            _authToken = null;
        }

        internal void InsertEventListener(ISessionManagerEventListener inEventListener)
        {
            _eventListeners.AddIfNotContains(inEventListener);
        }

        internal void RemoveEventListener(ISessionManagerEventListener inEventListener)
        {
            _eventListeners.RemoveIfContains(inEventListener);
        }

        internal void SetSessionHandler(SbSessionHandler inSessionHandler)
        {
            _sessionHandler = inSessionHandler;
        }

        internal bool HasSessionHandler()
        {
            return _sessionHandler != null;
        }

        internal void OnReceiveSessionError(SbErrorCode inErrorCode)
        {
            if (inErrorCode == SbErrorCode.SessionTokenRevoked)
            {
                OnAuthTokenRevoked();
            }
            else if (inErrorCode == SbErrorCode.InvalidAccessToken)
            {
                RequestAuthTokenRequiredIfNotStarted();
            }
            else if (inErrorCode == SbErrorCode.SessionKeyExpired)
            {
                RefreshSessionKeyIfNotStarted();
            }
        }

        private void ChangeSessionKeyIfNotEqual(string inSessionKey)
        {
            if (_sessionKey != inSessionKey)
            {
                _sessionKey = inSessionKey;
                _eventListeners.ForEach(inEventListener => { inEventListener.OnChangeSessionKey(_sessionKey); });
            }
        }

        private void ChangeAuthTokenIfNotEqual(string inAuthToken)
        {
            if (_authToken != inAuthToken)
            {
                _authToken = inAuthToken;
                _eventListeners.ForEach(inEventListener => { inEventListener.OnChangeAuthToken(_authToken); });
            }
        }

        private void ChangeEKeyIfNotEqual(string inEKey)
        {
            if (_eKey != inEKey)
            {
                _eKey = inEKey;
                _eventListeners.ForEach(inEventListener => { inEventListener.OnChangeEKey(_eKey); });
            }
        }

        private void OnAuthTokenRevoked()
        {
            ChangeSessionKeyIfNotEqual(null);
            _eventListeners.ForEach(inEventListener => { inEventListener.OnSessionError(SbErrorCode.SessionTokenRevoked); });
            _sessionHandler?.OnSessionClosed();
        }

        private bool IsRequestingAuthToken()
        {
            return _requestAuthTokenRequireCoroutine != null;
        }

        private bool IsRefreshingSessionKey()
        {
            return _refreshSessionKeyCoroutine != null;
        }

        private void RequestAuthTokenRequiredIfNotStarted()
        {
            if (IsRequestingAuthToken())
                return;

            if (_requestAuthTokenRequireCoroutine == null)
            {
                _requestAuthTokenRequireCoroutine = CoroutineManager.Instance.StartCoroutine(RequestAuthTokenRequiredCoroutine());
            }
        }

        private IEnumerator RequestAuthTokenRequiredCoroutine()
        {
            ChangeAuthTokenIfNotEqual(null);
            ChangeSessionKeyIfNotEqual(null);

            if (_sessionHandler != null)
            {
                _sessionTokenRequester.Reset();
                _sessionHandler.OnSessionTokenRequired(_sessionTokenRequester);

                Stopwatch stopwatch = Stopwatch.StartNew();
                long sessionTokenRefreshTimeoutMs = _chatMainContextRef.NetworkConfig.SessionTokenRefreshTimeout * 1000;
                while (_sessionTokenRequester.IsResponseComplete == false && stopwatch.ElapsedMilliseconds < sessionTokenRefreshTimeoutMs)
                    yield return null;

                ChangeAuthTokenIfNotEqual(_sessionTokenRequester.NewToken);
                if (string.IsNullOrEmpty(_authToken))
                {
                    SbError error = new SbError(SbErrorCode.PassedInvalidAccessToken);
                    _sessionHandler?.OnSessionError(error);
                }
            }

            _requestAuthTokenRequireCoroutine = null;

            if (string.IsNullOrEmpty(_authToken) == false)
            {
                RefreshSessionKeyIfNotStarted();
            }
            else
            {
                _eventListeners.ForEach(inEventListener => { inEventListener.OnSessionError(SbErrorCode.SessionKeyRefreshFailed); });
            }
        }

        private void RefreshSessionKeyIfNotStarted()
        {
            if (IsRefreshingSessionKey() || IsRequestingAuthToken())
                return;

            if (_refreshSessionKeyCoroutine == null)
            {
                _refreshSessionKeyCoroutine = CoroutineManager.Instance.StartCoroutine(RefreshSessionKeyCoroutine());
            }
        }

        private IEnumerator RefreshSessionKeyCoroutine()
        {
            const int MAX_RETRY_COUNT = 3;

            ChangeSessionKeyIfNotEqual(null);
            string newSessionKey = null;
            SbError error = null;
            bool isCompleteRefresh = false;
            int retryCount = 0;

            void OnSessionRefreshCompleteHandler(string inNewKey, SbError inError)
            {
                newSessionKey = inNewKey;
                error = inError;
                isCompleteRefresh = true;
            }

            RECONNECT_LABEL:
            {
                if (_chatMainContextRef.ConnectionManager.GetConnectionStateType() == ConnectionStateInternalType.Connected)
                {
                    RefreshSessionKeyByWsCommand(OnSessionRefreshCompleteHandler);
                }
                else
                {
                    RefreshSessionKeyByApiCommand(OnSessionRefreshCompleteHandler);
                }

                while (isCompleteRefresh == false)
                    yield return null;
            }

            if (retryCount < MAX_RETRY_COUNT && error != null && error.ErrorCode != SbErrorCode.SessionTokenRevoked && error.ErrorCode != SbErrorCode.InvalidAccessToken)
            {
                newSessionKey = null;
                error = null;
                isCompleteRefresh = false;
                retryCount++;
                goto RECONNECT_LABEL;
            }

            _refreshSessionKeyCoroutine = null;
            if (string.IsNullOrEmpty(newSessionKey) == false)
            {
                ChangeSessionKeyIfNotEqual(newSessionKey);
                _sessionHandler?.OnSessionRefreshed();
            }
            else if (error == null || error.ErrorCode == SbErrorCode.SessionTokenRevoked)
            {
                OnAuthTokenRevoked();
            }
            else if (error.ErrorCode == SbErrorCode.InvalidAccessToken)
            {
                RequestAuthTokenRequiredIfNotStarted();
            }
            else
            {
                SbError sessionRefreshFailedError = new SbError(SbErrorCode.SessionKeyRefreshFailed, error.ErrorMessage);
                _eventListeners.ForEach(inEventListener => { inEventListener.OnSessionError(sessionRefreshFailedError.ErrorCode); });
                _sessionHandler?.OnSessionError(sessionRefreshFailedError);
            }
        }

        private void RefreshSessionKeyByWsCommand(SessionRefreshHandler inCompletionHandler)
        {
            void OnAck(WsReceiveCommandAbstract inReceiveCommand, SbError inError)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                }
                else if (inReceiveCommand is LogiWsReceiveCommand logiWsReceiveCommand)
                {
                    if (logiWsReceiveCommand.hasError)
                    {
                        inCompletionHandler?.Invoke(null, new SbError(logiWsReceiveCommand.errorCode, logiWsReceiveCommand.errorMessage));
                    }
                    else
                    {
                        inCompletionHandler?.Invoke(logiWsReceiveCommand.newKey, null);
                    }
                }
            }

            bool expiringSession = _sessionHandler != null;
            LogiWsSendCommand wsSendCommand = new LogiWsSendCommand(_authToken, expiringSession, OnAck);
            _chatMainContextRef.CommandRouter.SendWsCommand(wsSendCommand);
        }

        private void RefreshSessionKeyByApiCommand(SessionRefreshHandler inCompletionHandler)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is RefreshSessionKeyApiCommand.Response refreshSessionKeyApiCommandResponse)
                {
                    inCompletionHandler?.Invoke(refreshSessionKeyApiCommandResponse.Key, null);
                }
                else if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                }
            }

            bool expiringSession = _sessionHandler != null;
            RefreshSessionKeyApiCommand.Request joinChannelApiCommand =
                new RefreshSessionKeyApiCommand.Request(_chatMainContextRef.ApplicationId, _userId, _authToken, expiringSession, OnCompletionHandler);

            _chatMainContextRef.CommandRouter.RequestApiCommand(joinChannelApiCommand);
        }

        private void StopRefreshAuthTokenAndSessionKey()
        {
            if (_requestAuthTokenRequireCoroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_requestAuthTokenRequireCoroutine);
                _requestAuthTokenRequireCoroutine = null;
            }

            if (_refreshSessionKeyCoroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_refreshSessionKeyCoroutine);
                _refreshSessionKeyCoroutine = null;
            }
        }

        internal void OnChangeConnectionState(ConnectionStateInternalType inChangedStateType, ConnectionStateAbstract.ParamsAbstract inChangedStateParams)
        {
            if (inChangedStateType == ConnectionStateInternalType.Connecting)
            {
                if (inChangedStateParams is ConnectingConnectionState.Params connectingConnectionStateParams)
                {
                    _userId = connectingConnectionStateParams.UserId;
                    _authToken = connectingConnectionStateParams.AuthToken;
                }
            }
            else if (inChangedStateType == ConnectionStateInternalType.Connected)
            {
                if (inChangedStateParams is ConnectedConnectionState.Params connectedConnectionStateParams && connectedConnectionStateParams.LogiWsReceiveCommand != null)
                {
                    ChangeSessionKeyIfNotEqual(connectedConnectionStateParams.LogiWsReceiveCommand.sessionKey);
                    ChangeEKeyIfNotEqual(connectedConnectionStateParams.LogiWsReceiveCommand.eKey);
                }
            }
            else if (inChangedStateType == ConnectionStateInternalType.Logout)
            {
                ChangeSessionKeyIfNotEqual(null);
                StopRefreshAuthTokenAndSessionKey();
            }
        }

        internal void OnReceiveWsEventCommand(WsReceiveCommandAbstract inWsReceiveCommand)
        {
            if (inWsReceiveCommand.CommandType == WsCommandType.SessionExpired)
            {
                SbErrorCode errorCode = SbErrorCode.SessionTokenRevoked;
                if (inWsReceiveCommand is SessionExpiredWsReceiveCommand sessionExpiredWsEventCommand)
                    errorCode = sessionExpiredWsEventCommand.Reason;

                OnReceiveSessionError(errorCode);
            }
        }
    }
}