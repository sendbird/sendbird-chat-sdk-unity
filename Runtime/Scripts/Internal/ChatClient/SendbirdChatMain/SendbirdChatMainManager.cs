// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal class SendbirdChatMainManager : SingletonAbstract<SendbirdChatMainManager>
    {
        private readonly List<SendbirdChatMain> _sendbirdChatMains = new List<SendbirdChatMain>();
        private SendbirdChatMainManager(){}

        internal void InsertChatMain(SendbirdChatMain inSendbirdChatMain)
        {
            _sendbirdChatMains.AddIfNotContains(inSendbirdChatMain);
        }

        internal void RemoveChatMain(SendbirdChatMain inSendbirdChatMain)
        {
            _sendbirdChatMains.RemoveIfContains(inSendbirdChatMain);
        }

        internal void UpdateAllSendbirdChatMains()
        {
            foreach (SendbirdChatMain sendbirdChatMain in _sendbirdChatMains)
            {
                sendbirdChatMain.Update();
            }
        }

        internal void DispatchEnterForegroundAllSendbirdChatMains()
        {
            foreach (SendbirdChatMain sendbirdChatMain in _sendbirdChatMains)
            {
                sendbirdChatMain.OnEnterForeground();
            }
        }
        
        internal void DispatchEnterBackgroundAllSendbirdChatMains()
        {
            foreach (SendbirdChatMain sendbirdChatMain in _sendbirdChatMains)
            {
                sendbirdChatMain.OnEnterBackground();
            }
        }
        
        internal void DispatchOnChangeNetworkReachabilityAllSendbirdChatMains(NetworkReachabilityType inNetworkReachabilityType)
        {
            foreach (SendbirdChatMain sendbirdChatMain in _sendbirdChatMains)
            {
                sendbirdChatMain.OnChangeNetworkReachability(inNetworkReachabilityType);
            }
        }
    }
}