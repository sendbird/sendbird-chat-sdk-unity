// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal class SendbirdChatMainContext
    {
        internal static readonly string SDK_VERSION = PlatformModule.PlatformProvider.PlatformApplication.SdkVersion;
        internal static readonly string PLATFORM_NAME = PlatformModule.PlatformProvider.PlatformApplication.PlatformName;
        internal static readonly string PLATFORM_VERSION = PlatformModule.PlatformProvider.PlatformApplication.PlatformVersion;
        internal static readonly string OS_NAME = PlatformModule.PlatformProvider.PlatformApplication.OsName;
        internal static readonly string OS_VERSION = PlatformModule.PlatformProvider.PlatformApplication.OsVersion;
        internal const int QUERY_DEFAULT_LIMIT = 20;
        internal string SdkVersion { get; private set; }
        internal string OsVersion { get; private set; }
        internal string ApplicationId { get; private set; }
        internal string CustomerAppVersion { get; private set; }
        internal SbUser CurrentUserRef { get; private set; }
        internal string CurrentUserId { get; private set; }
        internal string EKey { get; private set; } = null;
        internal bool UserLocalCache { get; } = false;
        internal int MaxUnreadCountOnSuperGroup { get; private set; }
        internal bool UseMemberInfoInMessage { get; private set; }
        internal SbUnreadMessageCount UnreadMessageCount { get; } = new SbUnreadMessageCount();
        internal float TypingIndicatorThrottle { get; private set; } = 1.0f;
        internal SbAppInfo AppInfo { get; } = new SbAppInfo();
        internal NetworkConfig NetworkConfig { get; } = new NetworkConfig();
        internal CommandRouter CommandRouter { get; }
        internal ConnectionManager ConnectionManager { get; }
        internal OpenChannelManager OpenChannelManager { get; }
        internal GroupChannelManager GroupChannelManager { get; }
        internal CollectionManager CollectionManager { get; }
        internal SessionManager SessionManager { get; }
        internal PushManager PushManager { get; }
        private readonly SendbirdChatMain _ownerChatMain = null;

        internal SendbirdChatMainContext(SendbirdChatMain inSendbirdChatMain)
        {
            _ownerChatMain = inSendbirdChatMain;
            CommandRouter = new CommandRouter(this);
            ConnectionManager = new ConnectionManager(this);
            SessionManager = new SessionManager(this);
            OpenChannelManager = new OpenChannelManager(this);
            GroupChannelManager = new GroupChannelManager(this);
            PushManager = new PushManager(this);
            CollectionManager = new CollectionManager(this);
        }

        internal void Initialize(string inApplicationId, string inCustomerAppVersion, string inSdkVersion = null, string inOsVersion = null)
        {
            ApplicationId = string.IsNullOrEmpty(inApplicationId) ? string.Empty : inApplicationId;
            CustomerAppVersion = string.IsNullOrEmpty(inCustomerAppVersion) ? string.Empty : inCustomerAppVersion;
            SdkVersion = string.IsNullOrEmpty(inSdkVersion) ? SendbirdChatMainContext.SDK_VERSION : inSdkVersion;
            OsVersion = string.IsNullOrEmpty(inOsVersion) ? SendbirdChatMainContext.OS_VERSION : inOsVersion;

            CommandRouter.Initialize();
            CommandRouter.InsertEventListener(_ownerChatMain);

            ConnectionManager.Initialize();
            ConnectionManager.InsertEventListener(_ownerChatMain);

            SessionManager.InsertEventListener(_ownerChatMain);
        }

        internal void Terminate()
        {
            GroupChannelManager.Terminate();
            OpenChannelManager.Terminate();

            SessionManager.RemoveEventListener(_ownerChatMain);
            SessionManager.Terminate();

            ConnectionManager.RemoveEventListener(_ownerChatMain);
            ConnectionManager.Terminate();

            CommandRouter.RemoveEventListener(_ownerChatMain);
            CommandRouter.Terminate();
        }

        internal void SetCurrentUser(SbUser inUser)
        {
            CurrentUserRef = inUser;
            CurrentUserId = inUser?.UserId;
        }

        internal void SetMemberInfoInMessage(bool inUse)
        {
            UseMemberInfoInMessage = inUse;
        }

        internal void SetTypingIndicatorThrottle(float inInterval)
        {
            TypingIndicatorThrottle = Math.Max(1.0f, inInterval);
            TypingIndicatorThrottle = Math.Min(inInterval, 9.0f);
        }

        internal void SetEKey(string inEKey)
        {
            EKey = inEKey;
        }

        internal void SetMaxUnreadCountOnSuperGroup(int inMaxUnreadCountOnSuperGroup)
        {
            MaxUnreadCountOnSuperGroup = inMaxUnreadCountOnSuperGroup;
        }
    }
}