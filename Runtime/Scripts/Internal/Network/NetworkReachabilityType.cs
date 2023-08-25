// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal enum NetworkReachabilityType
    {
        /// <summary>
        /// Network is not reachable.
        /// </summary>
        NotReachable,
        /// <summary>
        /// Network is reachable via carrier data network.
        /// </summary>
        ReachableViaCarrierDataNetwork,
        /// <summary>
        /// Network is reachable via WiFi or cable.
        /// </summary>
        ReachableViaLocalAreaNetwork,
    }
}