// 
//  Copyright (c) 2024 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal interface IPlatformApplicationEventListener
    {
        void OnNetworkReachabilityChanged(NetworkReachabilityType inNetworkReachabilityType);
        void OnUpdate();
        void OnApplicationPauseChanged(bool inIsPaused);
    }
}