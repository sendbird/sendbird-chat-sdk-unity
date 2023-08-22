// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal interface ISessionManagerEventListener
    {
        void OnChangeSessionKey(string inSessionKey);
        void OnChangeAuthToken(string inSessionToken);
        void OnChangeEKey(string inEKey);
        void OnSessionError(SbErrorCode inErrorCode);
    }
}