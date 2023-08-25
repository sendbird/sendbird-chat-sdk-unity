// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal sealed class InitializedConnectionState : ConnectionStateAbstract
    {
        internal override ConnectionStateInternalType StateType => ConnectionStateInternalType.Initialized;

        internal InitializedConnectionState(ConnectionManagerContext inConnectionManagerContext) : base(inConnectionManagerContext) { }

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
    }
}