// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal interface IPlatformApplication
    {
        string SdkVersion { get; }
        string PlatformName { get; }
        string PlatformVersion { get; }
        string OsName { get; }
        string OsVersion { get; }
        void StartAsyncProcessIfNotRunning(IPlatformApplicationEventListener inEventListener);
        void StopAsyncProcess();
        void SetExceptionPropagation(bool inEnable);
    }
}