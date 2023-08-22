// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal class ConnectionManagerContext
    {
        internal ConnectionManager OwnerConnectionManager { get; }
        internal SbUser LoggedInUser { get; private set; } = null;
        internal long ConnectionTimeoutDuration { get; private set; } = NetworkConfig.DEFAULT_CONNECTION_TIMEOUT_DURATION;
        internal ReconnectionContext ReconnectionContext { get; } = new ReconnectionContext();
        internal SendbirdChatMainContext ChatMainContextRef { get; }
        internal Dictionary<string, SbConnectionHandler> ConnectionHandlersById { get; } = new Dictionary<string, SbConnectionHandler>();
        private readonly int _connectionRetryMaxCount = NetworkConfig.DEFAULT_CONNECTION_RETRY_MAX_COUNT;
        private int _connectionRetryCount = 0;

        internal ConnectionManagerContext(ConnectionManager inOwnerConnectionManager, SendbirdChatMainContext inChatMainContext)
        {
            OwnerConnectionManager = inOwnerConnectionManager;
            ChatMainContextRef = inChatMainContext;
        }

        internal void IncreaseConnectionRetryCount()
        {
            _connectionRetryCount++;
        }

        internal void ClearConnectionRetryCount()
        {
            _connectionRetryCount = 0;
        }

        internal bool CanConnectionRetry()
        {
            bool ignoreMaxCount = _connectionRetryMaxCount < 0;
            if (ignoreMaxCount)
                return true;

            return _connectionRetryCount < _connectionRetryMaxCount;
        }

        internal void SetConnectionTimeoutDuration(int inTimeoutDuration)
        {
            ConnectionTimeoutDuration = inTimeoutDuration;
            ReconnectionContext.SetConnectionTimeoutDuration(inTimeoutDuration);
        }

        internal void SetLoggedInUser(SbUser inUser)
        {
            LoggedInUser = inUser;
            ChatMainContextRef.SetCurrentUser(inUser);
        }

        internal void AddConnectionHandler(string inIdentifier, SbConnectionHandler inConnectionHandler)
        {
            if (string.IsNullOrEmpty(inIdentifier) || inConnectionHandler == null)
            {
                Logger.Warning(Logger.CategoryType.Connection, "AddConnectionHandler() : Invalid parameter.");
                return;
            }

            if (ConnectionHandlersById.ContainsKey(inIdentifier) == false)
            {
                ConnectionHandlersById.Add(inIdentifier, inConnectionHandler);
            }
            else
            {
                ConnectionHandlersById[inIdentifier] = inConnectionHandler;
            }
        }

        internal void RemoveConnectionHandler(string inIdentifier)
        {
            ConnectionHandlersById.RemoveIfContains(inIdentifier);
        }

        internal void RemoveAllConnectionHandlers()
        {
            ConnectionHandlersById.Clear();
        }
    }
}