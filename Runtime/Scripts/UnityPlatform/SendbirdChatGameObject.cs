// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using UnityEngine;


namespace Sendbird.Chat
{
    internal class SendbirdChatGameObject : MonoBehaviour
    {
        private static SendbirdChatGameObject _internalInstance = null;
        private UnityPlatformApplication _unityPlatformApplication = null;

        internal static SendbirdChatGameObject Instance
        {
            get
            {
                if (_internalInstance == null)
                    CreateInstance();

                return _internalInstance;
            }
        }

        private static void CreateInstance()
        {
            if (_internalInstance != null)
                return;

            _internalInstance = FindObjectOfType<SendbirdChatGameObject>();
            if (_internalInstance != null)
                return;

            GameObject gameObject = new GameObject("SendbirdChatGameObject");
            DontDestroyOnLoad(gameObject);
            _internalInstance = gameObject.AddComponent<SendbirdChatGameObject>();
        }

        private NetworkReachability _networkReachability = NetworkReachability.NotReachable;
        private SendbirdChatGameObject() { }

        internal void SetUnityPlatformApplication(UnityPlatformApplication inUnityPlatformApplication)
        {
            _unityPlatformApplication = inUnityPlatformApplication;
        }

        private void Awake()
        {
            gameObject.hideFlags = HideFlags.HideAndDontSave;
        }

        private void Update()
        {
            _unityPlatformApplication?.Update();
            CheckNetworkReachability();
        }

        private void OnApplicationPause(bool inPauseStatus)
        {
            _unityPlatformApplication?.OnApplicationPause(inPauseStatus);
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

                _unityPlatformApplication?.OnNetworkReachabilityChanged(networkReachabilityType);
                _networkReachability = Application.internetReachability;
            }
        }
    }
}