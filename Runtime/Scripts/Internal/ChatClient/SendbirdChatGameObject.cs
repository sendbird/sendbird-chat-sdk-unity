// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using UnityEngine;
using Newtonsoft.Json.Utilities;


namespace Sendbird.Chat
{
    internal class SendbirdChatGameObject : MonoBehaviourSingletonAbstract<SendbirdChatGameObject>
    {
        private NetworkReachability _networkReachability = NetworkReachability.NotReachable;
        private SendbirdChatGameObject() { }

        private void Awake()
        {
            gameObject.hideFlags = HideFlags.HideAndDontSave;
        }

        private void Update()
        {
            CheckNetworkReachability();
            SendbirdChatMainManager.Instance.UpdateAllSendbirdChatMains();
            CoroutineManager.Instance.Update();
        }

        private void OnApplicationPause(bool inPauseStatus)
        {
            if (inPauseStatus)
            {
                SendbirdChatMainManager.Instance.DispatchEnterBackgroundAllSendbirdChatMains();
            }
            else
            {
                SendbirdChatMainManager.Instance.DispatchEnterForegroundAllSendbirdChatMains();
            }
        }

        private void CheckNetworkReachability()
        {
            if (_networkReachability != Application.internetReachability)
            {
                NetworkReachabilityType networkReachabilityType = NetworkReachabilityType.NotReachable;
                switch (Application.internetReachability)
                {
                    case NetworkReachability.NotReachable:
                        networkReachabilityType = NetworkReachabilityType.NotReachable;
                        break;
                    case NetworkReachability.ReachableViaCarrierDataNetwork:
                        networkReachabilityType = NetworkReachabilityType.ReachableViaCarrierDataNetwork;
                        break;
                    case NetworkReachability.ReachableViaLocalAreaNetwork:
                        networkReachabilityType = NetworkReachabilityType.ReachableViaLocalAreaNetwork;
                        break;
                }

                SendbirdChatMainManager.Instance.DispatchOnChangeNetworkReachabilityAllSendbirdChatMains(networkReachabilityType);
                _networkReachability = Application.internetReachability;
            }
        }

        internal void ForceApplicationPause(bool inPauseStatus)
        {
            OnApplicationPause(inPauseStatus);
        }

        private void OnApplicationQuit() { }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void OnAfterAssembliesLoaded()
        {
            Instance.hideFlags = HideFlags.HideAndDontSave;
        }
    }
}