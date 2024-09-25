// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal sealed class LogoutConnectionState : ConnectionStateAbstract
    {
        internal class Params : ParamsAbstract
        {
            internal string UserId { get; }
            internal Action CompletionHandler { get; }

            internal Params(string inUserId, Action inCompletionHandler = null)
            {
                UserId = inUserId;
                if (string.IsNullOrEmpty(UserId))
                {
                    UserId = string.Empty;
                }

                CompletionHandler = inCompletionHandler;
            }
        }

        internal override ConnectionStateInternalType StateType => ConnectionStateInternalType.Logout;
        private Action _completionHandler = null;
        private readonly CommandRouter _commandRouterRef = null;

        internal LogoutConnectionState(ConnectionManagerContext inConnectionManagerContext, CommandRouter inCommandRouter) : base(inConnectionManagerContext)
        {
            _commandRouterRef = inCommandRouter;
        }

        internal override void Enter(ParamsAbstract inParams)
        {
            base.Enter(inParams);

            if (!(inParams is Params logoutParams))
            {
                Logger.Error(Logger.CategoryType.Connection, "LogoutConnectionState::Enter Invalid params");
                return;
            }

            connectionManagerContextRef.SetLoggedInUser(null);

            _completionHandler = logoutParams.CompletionHandler;


            _commandRouterRef.CloseWs(OnCloseWebSocket);

            connectionManagerContextRef.ConnectionHandlersById.ForEachByValue(inHandler => { inHandler.OnDisconnected?.Invoke(logoutParams.UserId); });
            return;
            void OnCloseWebSocket(WsClientCloseResult inWsClientCloseResult)
            {
                if (isEnteredState == false)
                    return;

                InvokeAndSetNullToCompletionHandler();
            }
        }

        internal override void Exit()
        {
            if (_completionHandler != null)
            {
                _completionHandler.Invoke();
                _completionHandler = null;
            }

            base.Exit();
        }

        internal override void Connect(string inWsHost, string inUserId, string inAuthToken, SbUserHandler inCompletionHandler, long inTimeoutDuration)
        {
            base.Connect(inWsHost, inUserId, inAuthToken, inCompletionHandler, inTimeoutDuration);
            ConnectingConnectionState.Params connectingParams = new ConnectingConnectionState.Params(inWsHost, inUserId, inAuthToken, inCompletionHandler, inTimeoutDuration);
            connectionManagerContextRef.OwnerConnectionManager.ChangeState(ConnectionStateInternalType.Connecting, connectingParams);
        }

        internal override void Logout(Action inDisconnectHandler = null)
        {
            base.Logout(inDisconnectHandler);
            CoroutineManager.Instance.CallOnNextFrame(() => { inDisconnectHandler?.Invoke(); });
        }

        private void InvokeAndSetNullToCompletionHandler()
        {
            if (_completionHandler != null)
            {
                Action completionHandler = _completionHandler;
                CoroutineManager.Instance.CallOnNextFrame(() => completionHandler.Invoke());
            }

            _completionHandler = null;
        }
    }
}