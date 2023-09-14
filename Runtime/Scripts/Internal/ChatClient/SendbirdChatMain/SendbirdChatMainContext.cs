// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using UnityEngine;

namespace Sendbird.Chat
{
    internal class SendbirdChatMainContext
    {
        internal const string SDK_VERSION = "4.0.0-beta.1";
        internal const string PLATFORM_NAME = "Unity";
        internal static readonly string PLATFORM_VERSION = Application.version;
        internal static readonly string OS_NAME = Application.platform.ToString();
        internal static readonly string OS_VERSION = SystemInfo.operatingSystem;
        internal const int QUERY_DEFAULT_LIMIT = 20;
        internal string SdkVersion { get; private set; }
        internal string PlatformName { get; private set; }
        internal string PlatformVersion { get; private set; }
        internal string OsName { get; private set; }
        internal string OsVersion { get; private set; }
        internal string ApplicationId { get; private set; }
        internal string CustomerAppVersion { get; private set; }
        internal SbUser CurrentUserRef { get; private set; }
        internal string CurrentUserId { get; private set; }
        internal string EKey { get; private set; } = null;
        internal bool UserLocalCache { get; } = false;
        internal int MaxUnreadCountOnSuperGroup { get; private set; }
        internal SbUnreadMessageCount UnreadMessageCount { get; } = new SbUnreadMessageCount();
        internal bool UseMemberInfoInMessage { get; private set; } = true;
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

        internal void Initialize(string inSdkVersion, string inPlatformName, string inPlatformVersion,
                                 string inOsName, string inOsVersion, string inApplicationId, string inCustomerAppVersion)
        {
            SdkVersion = inSdkVersion;
            PlatformName = inPlatformName;
            PlatformVersion = inPlatformVersion;
            OsName = inOsName;
            OsVersion = inOsVersion;
            ApplicationId = inApplicationId;
            CustomerAppVersion = inCustomerAppVersion;

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