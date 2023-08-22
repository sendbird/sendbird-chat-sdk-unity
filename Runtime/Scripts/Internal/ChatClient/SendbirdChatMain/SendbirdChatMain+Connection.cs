// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal partial class SendbirdChatMain
    {
        internal void AddConnectionHandler(string inIdentifier, SbConnectionHandler inConnectionHandler)
        {
            ChatMainContext.ConnectionManager.AddConnectionHandler(inIdentifier, inConnectionHandler);
        }

        internal void RemoveConnectionHandler(string inIdentifier)
        {
            ChatMainContext.ConnectionManager.RemoveConnectionHandler(inIdentifier);
        }

        internal void RemoveAllConnectionHandlers()
        {
            ChatMainContext.ConnectionManager.RemoveAllConnectionHandlers();
        }

        internal void Connect(string inUserId, string inAuthToken, string inApiHost, string inWsHost, SbUserHandler inCompletionHandler)
        {
            Logger.Info(Logger.CategoryType.Common, $"Connect UserId:{inUserId}, AuthToken:{inAuthToken}");

            if (string.IsNullOrEmpty(inUserId))
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("UserId");
                Logger.Error(Logger.CategoryType.Connection, $"ConnectionManager::Connect {error.ErrorMessage}");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(null, error));
                return;
            }

            if (string.IsNullOrEmpty(inApiHost))
            {
                inApiHost = NetworkConfig.GetDefaultApiHost(ChatMainContext.ApplicationId);
            }

            ChatMainContext.CommandRouter.SetApiHost(inApiHost);

            if (string.IsNullOrEmpty(inWsHost))
            {
                inWsHost = NetworkConfig.GetDefaultWsHost(ChatMainContext.ApplicationId);
            }

            ChatMainContext.ConnectionManager.Connect(inUserId, inAuthToken, inWsHost, inCompletionHandler);
        }

        internal void Disconnect(SbDisconnectHandler inDisconnectHandler = null)
        {
            ChatMainContext.ConnectionManager.Disconnect(inDisconnectHandler);
        }

        internal bool Reconnect()
        {
            return ChatMainContext.ConnectionManager.Reconnect();
        }

        internal SbConnectionState ConnectionState()
        {
            switch (ChatMainContext.ConnectionManager.GetConnectionStateType())
            {
                case Chat.ConnectionStateInternalType.Connecting: return SbConnectionState.Connecting;
                case Chat.ConnectionStateInternalType.Connected:  return SbConnectionState.Open;
                default:                                          return SbConnectionState.Closed;
            }
        }

        void IConnectionManagerEventListener.OnChangeConnectionState(ConnectionStateInternalType inChangedStateType, ConnectionStateAbstract.ParamsAbstract inChangedStateParams)
        {
            ChatMainContext.SessionManager.OnChangeConnectionState(inChangedStateType, inChangedStateParams);
            ChatMainContext.CommandRouter.OnChangeConnectionState(inChangedStateType);
            ChatMainContext.OpenChannelManager.OnChangeConnectionState(inChangedStateType);
            ChatMainContext.CollectionManager.OnChangeConnectionState(inChangedStateType);
        }
    }
}