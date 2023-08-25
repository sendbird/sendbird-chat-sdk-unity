// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal class SessionTokenRequester : ISbSessionTokenRequester
    {
        internal bool IsResponseComplete { get; private set; } = false;
        internal string NewToken { get; private set; } = null;

        internal void Reset()
        {
            IsResponseComplete = false;
            NewToken = null;
        }

        void ISbSessionTokenRequester.OnFail()
        {
            IsResponseComplete = true;
        }

        void ISbSessionTokenRequester.OnSuccess(string inNewToken)
        {
            IsResponseComplete = true;
            NewToken = inNewToken;
        }
    }
}