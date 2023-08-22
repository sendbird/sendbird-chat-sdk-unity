// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal abstract class ConnectionStateAbstract
    {
        internal abstract class ParamsAbstract { }

        internal abstract ConnectionStateInternalType StateType { get; }
        protected readonly ConnectionManagerContext connectionManagerContextRef;
        protected bool isEnteredState = false;

        protected ConnectionStateAbstract(ConnectionManagerContext inConnectionManagerContext)
        {
            connectionManagerContextRef = inConnectionManagerContext;
        }

        internal virtual void Enter(ParamsAbstract inParams)
        {
            Logger.Info(Logger.CategoryType.Connection, $"ConnectionStateAbstract::Enter {StateType}");
            isEnteredState = true;
        }

        internal virtual void Exit()
        {
            Logger.Info(Logger.CategoryType.Connection, $"ConnectionStateAbstract::Exit {StateType}");
            isEnteredState = false;
        }

        internal virtual void Connect(string inWsHost, string inUserId, string inAccessToken, SbUserHandler inCompletionHandler, long inTimeoutDuration)
        {
            Logger.Info(Logger.CategoryType.Connection, $"ConnectionStateAbstract::Connect {StateType}");
        }

        internal virtual bool Reconnect()
        {
            Logger.Info(Logger.CategoryType.Connection, $"ConnectionStateAbstract::Reconnect {StateType}");
            return false;
        }

        internal virtual void Logout(Action inDisconnectHandler = null)
        {
            Logger.Info(Logger.CategoryType.Connection, $"ConnectionStateAbstract::Disconnect {StateType}");
        }

        internal virtual void OnLogiReceived(LogiWsReceiveCommand inLogiWsReceiveCommand)
        {
            Logger.Info(Logger.CategoryType.Connection, $"ConnectionStateAbstract::LogiReceived {StateType}");
        }

        internal virtual void OnError(SbError inError)
        {
            Logger.Info(Logger.CategoryType.Connection, $"ConnectionStateAbstract::OnError {StateType} ErrorCode:{inError?.ErrorCode}");
        }

        internal virtual void OnChangeAuthTokenOrSessionKey()
        {
            Logger.Info(Logger.CategoryType.Connection, $"ConnectionStateAbstract::OnChangeAuthTokenOrSessionKey {StateType}");
        }

        internal virtual void OnSessionError(SbErrorCode inErrorCode)
        {
            Logger.Info(Logger.CategoryType.Connection, $"ConnectionStateAbstract::OnSessionError {StateType} ErrorCode:{inErrorCode}");
        }

        internal virtual void OnEnterForeground()
        {
            Logger.Info(Logger.CategoryType.Connection, $"ConnectionStateAbstract::EnterForeground {StateType}");
        }

        internal virtual void OnEnterBackground()
        {
            Logger.Info(Logger.CategoryType.Connection, $"ConnectionStateAbstract::EnterBackground {StateType}");
        }

        internal virtual void OnChangeNetworkReachability(NetworkReachabilityType inNetworkReachabilityType)
        {
            Logger.Info(Logger.CategoryType.Connection, $"ConnectionStateAbstract::OnChangeNetworkReachability {inNetworkReachabilityType}");
        }
    }
}