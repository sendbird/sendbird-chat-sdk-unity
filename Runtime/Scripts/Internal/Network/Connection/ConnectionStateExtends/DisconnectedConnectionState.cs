// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal sealed class DisconnectedConnectionState : ConnectionStateAbstract
    {
        internal class Params : ParamsAbstract
        {
            internal bool ShouldReconnect { get; }
            internal SbErrorCode ErrorCode { get; }

            internal Params(bool inShouldReconnect = false, SbErrorCode inErrorCode = SbErrorCode.UnknownError)
            {
                ShouldReconnect = inShouldReconnect;
                ErrorCode = inErrorCode;
            }
        }

        internal override ConnectionStateInternalType StateType => ConnectionStateInternalType.Disconnected;
        private CoroutineJob _changeReconnectingStateCoroutine = null;
        private readonly CommandRouter _commandRouterRef = null;

        internal DisconnectedConnectionState(ConnectionManagerContext inConnectionManagerContext, CommandRouter inCommandRouter) : base(inConnectionManagerContext)
        {
            _commandRouterRef = inCommandRouter;
        }

        internal override void Enter(ParamsAbstract inParams)
        {
            base.Enter(inParams);

            if (!(inParams is Params disconnectParams))
            {
                Logger.Error(Logger.CategoryType.Connection, "DisconnectedConnectionState::Enter Invalid params");
                return;
            }

            _commandRouterRef.CloseWs();

            if (disconnectParams.ShouldReconnect)
            {
                _changeReconnectingStateCoroutine = CoroutineManager.Instance.CallOnNextFrame(
                    () => connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Reconnecting));
            }
        }

        internal override void Connect(string inWsHost, string inUserId, string inAuthToken, SbUserHandler inCompletionHandler, long inTimeoutDuration)
        {
            base.Connect(inWsHost, inUserId, inAuthToken, inCompletionHandler, inTimeoutDuration);
            StopChangeToReconnectingStateCoroutineIfStarted();

            void OnChangeConnectingState()
            {
                ConnectingConnectionState.Params connectingParams = new ConnectingConnectionState.Params(inWsHost, inUserId, inAuthToken, inCompletionHandler, inTimeoutDuration);
                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Connecting, connectingParams);
            }

            if (connectionManagerContextRef.LoggedInUser?.UserId == inUserId)
            {
                OnChangeConnectingState();
                return;
            }

            LogoutConnectionState.Params logoutParams = new LogoutConnectionState.Params(connectionManagerContextRef.LoggedInUser?.UserId, OnChangeConnectingState);
            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Logout, logoutParams);
        }

        internal override void Logout(Action inDisconnectHandler = null)
        {
            base.Logout(inDisconnectHandler);
            StopChangeToReconnectingStateCoroutineIfStarted();

            LogoutConnectionState.Params logoutParams = new LogoutConnectionState.Params(connectionManagerContextRef.LoggedInUser?.UserId, inDisconnectHandler);
            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Logout, logoutParams);
        }

        internal override bool Reconnect()
        {
            base.Reconnect();
            StopChangeToReconnectingStateCoroutineIfStarted();
            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Reconnecting);
            return true;
        }

        internal override void OnSessionError(SbErrorCode inErrorCode)
        {
            base.OnSessionError(inErrorCode);
            if (inErrorCode == SbErrorCode.SessionTokenRevoked)
            {
                LogoutConnectionState.Params logoutParams = new LogoutConnectionState.Params(connectionManagerContextRef.LoggedInUser?.UserId);
                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Logout, logoutParams);
            }
        }

        internal override void OnChangeAuthTokenOrSessionKey()
        {
            base.OnChangeAuthTokenOrSessionKey();
            if (connectionManagerContextRef.ReconnectionContext.IsValidAuthTokenAndSessionKey())
            {
                StopChangeToReconnectingStateCoroutineIfStarted();
                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Reconnecting);
            }
        }

        internal override void OnEnterForeground()
        {
            base.OnEnterForeground();
            StopChangeToReconnectingStateCoroutineIfStarted();
            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Reconnecting);
        }

        internal override void OnChangeNetworkReachability(NetworkReachabilityType inNetworkReachabilityType)
        {
            base.OnChangeNetworkReachability(inNetworkReachabilityType);
            if (inNetworkReachabilityType != NetworkReachabilityType.NotReachable)
            {
                StopChangeToReconnectingStateCoroutineIfStarted();
                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Reconnecting);
            }
        }

        private void StopChangeToReconnectingStateCoroutineIfStarted()
        {
            if (_changeReconnectingStateCoroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_changeReconnectingStateCoroutine);
                _changeReconnectingStateCoroutine = null;
            }
        }
    }
}