// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal enum NetworkReachabilityType
    {
        /// Network is not reachable.
        NotReachable,
        /// Network is reachable via carrier data network.
        ReachableViaCarrierDataNetwork,
        /// Network is reachable via Wi-Fi or cable.
        ReachableViaLocalAreaNetwork,
    }
}