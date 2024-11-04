// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using UnityEngine;

namespace Sendbird.Chat
{
    internal class UnityPlatformApplication : IPlatformApplication
    {
        string IPlatformApplication.SdkVersion => "4.0.1";
        string IPlatformApplication.PlatformName => "Unity";
        string IPlatformApplication.PlatformVersion => Application.unityVersion;
        string IPlatformApplication.OsName => Application.platform.ToString();
        string IPlatformApplication.OsVersion => SystemInfo.operatingSystem;
        private IPlatformApplicationEventListener _eventListener = null;
        private bool _canExceptionPropagation = false;

        void IPlatformApplication.StartAsyncProcessIfNotRunning(IPlatformApplicationEventListener inEventListener)
        {
            _eventListener = inEventListener;
            SendbirdChatGameObject.Instance.SetUnityPlatformApplication(this);
        }

        void IPlatformApplication.StopAsyncProcess()
        {
            SendbirdChatGameObject.Instance.SetUnityPlatformApplication(null);
        }

        void IPlatformApplication.SetExceptionPropagation(bool inEnable)
        {
            _canExceptionPropagation = inEnable;
        }

        internal void Update()
        {
            try
            {
                _eventListener?.OnUpdate();
            }
            catch (Exception exception)
            {
                Logger.Error(Logger.CategoryType.Common, exception.Message);
                if (_canExceptionPropagation)
                    throw;
            }
        }

        internal void OnApplicationPause(bool inPauseStatus)
        {
            _eventListener?.OnApplicationPauseChanged(inPauseStatus);
        }

        internal void OnNetworkReachabilityChanged(NetworkReachabilityType inNetworkReachabilityType)
        {
            _eventListener?.OnNetworkReachabilityChanged(inNetworkReachabilityType);
        }
    }
}