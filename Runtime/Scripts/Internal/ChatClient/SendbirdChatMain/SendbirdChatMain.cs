// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using UnityEngine;

namespace Sendbird.Chat
{
    internal partial class SendbirdChatMain : ICommandRouterEventListener, ISessionManagerEventListener, IConnectionManagerEventListener
    {
        internal SendbirdChatMainContext ChatMainContext { get; }
        private bool _isTerminated = true;

        internal string ApplicationId => ChatMainContext.ApplicationId;
        internal string EKey => ChatMainContext.EKey;
        internal string OSVersion => ChatMainContext.OsVersion;
        internal SbAppInfo AppInfo => ChatMainContext.AppInfo;
        internal SbUser CurrentUser => ChatMainContext.CurrentUserRef;
        internal ConnectionStateInternalType ConnectionStateInternalType() => ChatMainContext.ConnectionManager.GetConnectionStateType();
        private readonly Dictionary<string, SbUserEventHandler> _userEventHandlersById = new Dictionary<string, SbUserEventHandler>();

        internal SendbirdChatMain()
        {
            ChatMainContext = new SendbirdChatMainContext(this);
        }

        internal void Initialize(SbInitParams inInitParams)
        {
            if (_isTerminated == false)
                Terminate();

            ChatMainContext.Initialize(SendbirdChatMainContext.SDK_VERSION, SendbirdChatMainContext.PLATFORM_NAME, SendbirdChatMainContext.PLATFORM_VERSION,
                                       SendbirdChatMainContext.OS_NAME, SendbirdChatMainContext.OS_VERSION, inInitParams.ApplicationId, inInitParams.AppVersion);

            _isTerminated = false;

            SendbirdChatMainManager.Instance.InsertChatMain(this);
        }

        private void Terminate()
        {
            if (_isTerminated)
                return;

            SendbirdChatMainManager.Instance.RemoveChatMain(this);
            ChatMainContext.Terminate();
            _isTerminated = true;
        }

        internal void Update()
        {
            ChatMainContext.CommandRouter.Update();
            ChatMainContext.ConnectionManager.Update();
            ChatMainContext.GroupChannelManager.Update();
        }

        internal void OnEnterForeground()
        {
            ChatMainContext.ConnectionManager.OnEnterForeground();
        }

        internal void OnEnterBackground()
        {
            ChatMainContext.ConnectionManager.OnEnterBackground();
        }

        internal void OnChangeNetworkReachability(NetworkReachabilityType inNetworkReachabilityType)
        {
            ChatMainContext.ConnectionManager.OnChangeNetworkReachability(inNetworkReachabilityType);
        }

        internal void SetChannelInvitationPreference(bool inAutoAccept, SbErrorHandler inCompletionHandler)
        {
            if (CurrentUser == null)
            {
                if (inCompletionHandler != null)
                {
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            string userId = CurrentUser?.UserId;
            SetChannelInvitationPreferenceApiCommand.Request apiCommand = new SetChannelInvitationPreferenceApiCommand.Request(userId, inAutoAccept, OnCompletionHandler);
            ChatMainContext.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void GetChannelInvitationPreference(SbChannelInvitationPreferenceHandler inCompletionHandler)
        {
            if (CurrentUser == null)
            {
                if (inCompletionHandler != null)
                {
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(false, SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is GetChannelInvitationPreferenceApiCommand.Response response)
                {
                    inCompletionHandler?.Invoke(response.autoAccept, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(false, inError);
                }
            }

            string userId = CurrentUser?.UserId;
            GetChannelInvitationPreferenceApiCommand.Request apiCommand = new GetChannelInvitationPreferenceApiCommand.Request(userId, OnCompletionHandler);
            ChatMainContext.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void SetSessionHandler(SbSessionHandler inSessionHandler)
        {
            ChatMainContext.SessionManager.SetSessionHandler(inSessionHandler);
        }

        internal SbApplicationUserListQuery CreateApplicationUserListQuery(SbApplicationUserListQueryParams inParams = null)
        {
            if (inParams == null)
                inParams = new SbApplicationUserListQueryParams();

            return new SbApplicationUserListQuery(inParams, ChatMainContext);
        }

        internal SbBlockedUserListQuery CreateBlockedUserListQuery(SbBlockedUserListQueryParams inParams = null)
        {
            if (inParams == null)
                inParams = new SbBlockedUserListQueryParams();

            return new SbBlockedUserListQuery(inParams, ChatMainContext);
        }

        public SbMessageSearchQuery CreateMessageSearchQuery(SbMessageSearchQueryParams inParams = null)
        {
            if (inParams == null)
                inParams = new SbMessageSearchQueryParams();

            return new SbMessageSearchQuery(inParams, ChatMainContext);
        }
    }
}