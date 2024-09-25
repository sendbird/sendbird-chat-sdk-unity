// 
//  Copyright (c) 2024 Sendbird, Inc.
// 

using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal class SdkManager : SingletonAbstract<SdkManager>, IPlatformApplicationEventListener
    {
        private readonly List<SendbirdChatMain> _sendbirdChatMains = new List<SendbirdChatMain>();
        private readonly BlockingCollection<SendbirdChatMain> _insertChatMainBlockingCollection = new BlockingCollection<SendbirdChatMain>();
        private readonly BlockingCollection<SendbirdChatMain> _removeChatMainBlockingCollection = new BlockingCollection<SendbirdChatMain>();
        private SdkManager() { }

        internal void InsertChatMain(SendbirdChatMain inSendbirdChatMain)
        {
            _insertChatMainBlockingCollection.Add(inSendbirdChatMain);
            PlatformModule.PlatformProvider.PlatformApplication.StartAsyncProcessIfNotRunning(this);
        }

        internal void RemoveChatMain(SendbirdChatMain inSendbirdChatMain)
        {
            _removeChatMainBlockingCollection.Add(inSendbirdChatMain);
        }

        internal void StartAsyncProcessIfNotRunning()
        {
            PlatformModule.PlatformProvider.PlatformApplication.StartAsyncProcessIfNotRunning(this);
        }

        void IPlatformApplicationEventListener.OnNetworkReachabilityChanged(NetworkReachabilityType inNetworkReachabilityType)
        {
            foreach (SendbirdChatMain sendbirdChatMain in _sendbirdChatMains)
            {
                sendbirdChatMain.OnChangeNetworkReachability(inNetworkReachabilityType);
            }
        }

        void IPlatformApplicationEventListener.OnUpdate()
        {
            while (0 < _removeChatMainBlockingCollection.Count)
            {
                _sendbirdChatMains.RemoveIfContains(_removeChatMainBlockingCollection.Take());
                if (_sendbirdChatMains.Count <= 0)
                {
                    PlatformModule.PlatformProvider.PlatformApplication.StopAsyncProcess();
                }
            }
            
            while (0 < _insertChatMainBlockingCollection.Count)
            {
                _sendbirdChatMains.AddIfNotContains(_insertChatMainBlockingCollection.Take());
                PlatformModule.PlatformProvider.PlatformApplication.StartAsyncProcessIfNotRunning(this);
            }

            foreach (SendbirdChatMain sendbirdChatMain in _sendbirdChatMains)
            {
                sendbirdChatMain.Update();
            }

            CoroutineManager.Instance.Update();
        }

        void IPlatformApplicationEventListener.OnApplicationPauseChanged(bool inPauseStatus)
        {
            if (inPauseStatus)
            {
                foreach (SendbirdChatMain sendbirdChatMain in _sendbirdChatMains)
                {
                    sendbirdChatMain.OnEnterBackground();
                }
            }
            else
            {
                foreach (SendbirdChatMain sendbirdChatMain in _sendbirdChatMains)
                {
                    sendbirdChatMain.OnEnterForeground();
                }
            }
        }
    }
}