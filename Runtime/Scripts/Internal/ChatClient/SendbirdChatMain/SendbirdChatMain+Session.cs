// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal partial class SendbirdChatMain
    {
        void ISessionManagerEventListener.OnChangeSessionKey(string inSessionKey)
        {
            ChatMainContext.ConnectionManager.OnChangeSessionKey(inSessionKey);
            ChatMainContext.CommandRouter.OnChangeSessionKey(inSessionKey);
        }

        void ISessionManagerEventListener.OnChangeAuthToken(string inSessionToken)
        {
            ChatMainContext.ConnectionManager.OnChangeSessionToken(inSessionToken);
        }

        void ISessionManagerEventListener.OnChangeEKey(string inEKey)
        {
            ChatMainContext.SetEKey(inEKey);
        }

        void ISessionManagerEventListener.OnSessionError(SbErrorCode inErrorCode)
        {
            ChatMainContext.ConnectionManager.OnSessionError(inErrorCode);
            ChatMainContext.CommandRouter.OnSessionError(inErrorCode);
        }
    }
}