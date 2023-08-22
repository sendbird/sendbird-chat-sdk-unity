// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal sealed class ConnectedConnectionState : ConnectionStateAbstract
    {
        internal class Params : ParamsAbstract
        {
            internal LogiWsReceiveCommand LogiWsReceiveCommand { get; }
            internal string ConnectedUserId { get; }
            internal string ConnectedAuthToken { get; }
            internal SbUserHandler ConnectCompletionHandler { get; }

            internal Params(LogiWsReceiveCommand inLogiWsReceiveCommand, string inConnectedUserId, string inConnectedAuthToken, SbUserHandler inConnectCompletionHandler)
            {
                LogiWsReceiveCommand = inLogiWsReceiveCommand;
                ConnectedUserId = inConnectedUserId;
                ConnectedAuthToken = inConnectedAuthToken;
                ConnectCompletionHandler = inConnectCompletionHandler;
            }
        }

        internal override ConnectionStateInternalType StateType => ConnectionStateInternalType.Connected;

        internal ConnectedConnectionState(ConnectionManagerContext inConnectionManagerContext) : base(inConnectionManagerContext) { }

        internal override void Enter(ParamsAbstract inParams)
        {
            base.Enter(inParams);
            
            if (!(inParams is Params connectedParams) || connectedParams.LogiWsReceiveCommand == null)
            {
                Logger.Error(Logger.CategoryType.Connection, "ConnectedConnectionState::Enter Invalid params");
                return;
            }

            SbUser user = new SbUser(connectedParams.LogiWsReceiveCommand.UserDto, connectionManagerContextRef.ChatMainContextRef);
            connectionManagerContextRef.SetLoggedInUser(user);
            connectionManagerContextRef.ReconnectionContext.OnChangeReconnectionCommand(connectedParams.LogiWsReceiveCommand.reconnect);
            connectionManagerContextRef.ChatMainContextRef.SetMaxUnreadCountOnSuperGroup(connectedParams.LogiWsReceiveCommand.maxUnreadCountOnSuperGroup);
            connectionManagerContextRef.ChatMainContextRef.AppInfo.ResetFromDto(connectedParams.LogiWsReceiveCommand.AppInfoDto);
            CoroutineManager.Instance.CallOnNextFrame(() => connectedParams.ConnectCompletionHandler?.Invoke(user, null));

            connectionManagerContextRef.ConnectionHandlersById.ForEachByValue(inHandler => { inHandler.OnConnected?.Invoke(connectedParams.ConnectedUserId); });
        }

        internal override void OnError(SbError inError)
        {
            base.OnError(inError);
            DisconnectedConnectionState.Params disconnectedParams = new DisconnectedConnectionState.Params(inShouldReconnect: true);
            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, disconnectedParams);
        }

        internal override void OnSessionError(SbErrorCode inErrorCode)
        {
            base.OnSessionError(inErrorCode);
            if (inErrorCode == SbErrorCode.SessionTokenRevoked)
            {
                LogoutConnectionState.Params logoutParams = new LogoutConnectionState.Params(connectionManagerContextRef.LoggedInUser?.UserId);
                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Logout, logoutParams);
            }
            else
            {
                DisconnectedConnectionState.Params disconnectedParams = new DisconnectedConnectionState.Params(inShouldReconnect: false);
                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, disconnectedParams);
            }
        }

        internal override void Connect(string inWsHost, string inUserId, string inAuthToken, SbUserHandler inCompletionHandler, long inTimeoutDuration)
        {
            base.Connect(inWsHost, inUserId, inAuthToken, inCompletionHandler, inTimeoutDuration);
            if (connectionManagerContextRef.LoggedInUser?.UserId == inUserId)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler?.Invoke(connectionManagerContextRef.LoggedInUser, null); });
                return;
            }

            void OnLogoutCompletionHandler()
            {
                ConnectingConnectionState.Params connectingParams = new ConnectingConnectionState.Params(inWsHost, inUserId, inAuthToken, inCompletionHandler, inTimeoutDuration);
                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Connecting, connectingParams);
            }

            LogoutConnectionState.Params logoutParams = new LogoutConnectionState.Params(connectionManagerContextRef.LoggedInUser?.UserId, OnLogoutCompletionHandler);
            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Logout, logoutParams);
        }

        internal override void Logout(Action inDisconnectHandler = null)
        {
            base.Logout(inDisconnectHandler);
            LogoutConnectionState.Params logoutParams = new LogoutConnectionState.Params(connectionManagerContextRef.LoggedInUser?.UserId, inDisconnectHandler);
            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Logout, logoutParams);
        }

        internal override bool Reconnect()
        {
            base.Reconnect();
            DisconnectedConnectionState.Params disconnectedParams = new DisconnectedConnectionState.Params(inShouldReconnect: true);
            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, disconnectedParams);
            return true;
        }

        internal override void OnEnterBackground()
        {
            base.OnEnterBackground();
            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, new DisconnectedConnectionState.Params());
        }

        internal override void OnChangeNetworkReachability(NetworkReachabilityType inNetworkReachabilityType)
        {
            base.OnChangeNetworkReachability(inNetworkReachabilityType);
            if (inNetworkReachabilityType == NetworkReachabilityType.NotReachable)
            {
                connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Disconnected, new DisconnectedConnectionState.Params());
            }
        }
    }
}