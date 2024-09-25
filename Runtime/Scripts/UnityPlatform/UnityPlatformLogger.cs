// 
//  Copyright (c) 2024 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal class UnityPlatformLogger : IPlatformLogger
    {
        void IPlatformLogger.Error(string inMessage)
        {
            UnityEngine.Debug.LogError(inMessage);
        }

        void IPlatformLogger.Warning(string inMessage)
        {
            UnityEngine.Debug.LogWarning(inMessage);
        }

        void IPlatformLogger.Info(string inMessage)
        {
            UnityEngine.Debug.Log(inMessage);
        }

        void IPlatformLogger.Debug(string inMessage)
        {
            UnityEngine.Debug.Log(inMessage);
        }

        void IPlatformLogger.Verbose(string inMessage)
        {
            UnityEngine.Debug.Log(inMessage);
        }
    }
}
