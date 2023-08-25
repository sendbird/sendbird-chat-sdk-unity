// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// @since 4.0.0
    public partial class SendbirdChatClient : ISendbirdChatClient
    {
        /// <summary>
        /// Current SDK version.
        /// </summary>
        /// @since 4.0.0
        public string SDKVersion => SendbirdChatMainContext.SDK_VERSION;

        /// <summary>
        /// Current OS version.
        /// </summary>
        /// @since 4.0.0
        public string OSVersion => _sendbirdChatMain.OSVersion;

        /// <summary>
        /// Current application ID.
        /// </summary>
        /// @since 4.0.0
        public string ApplicationId => _sendbirdChatMain.ApplicationId;

        /// <summary>
        /// The key to authenticate the url retrieved from SbFileMessage.PlainUrl, SbUser.PlainProfileImageUrl and SbThumbnail.PlainUrl. This key has to be put into the HTTP header to access the url provided by above methods.
        /// </summary>
        /// @since 4.0.0
        public string EKey => _sendbirdChatMain.EKey;

        /// <summary>
        /// Represents information obtained from the application settings.
        /// </summary>
        /// @since 4.0.0
        public SbAppInfo AppInfo => _sendbirdChatMain.AppInfo;

        /// <summary>
        /// The current connected User. null if connect is not called.
        /// </summary>
        /// @since 4.0.0
        public SbUser CurrentUser => _sendbirdChatMain.CurrentUser;

        /// <summary>
        /// SbGroupChannelModule class.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelModule GroupChannel { get; }

        /// <summary>
        /// SbOpenChannelModule class.
        /// </summary>
        /// @since 4.0.0
        public SbOpenChannelModule OpenChannel { get; }

        /// <summary>
        /// SbMessageModule class.
        /// </summary>
        /// @since 4.0.0
        public SbMessageModule Message { get; }

        /// <summary>
        /// SendbirdChatOptions class.
        /// </summary>
        /// @since 4.0.0
        public SendbirdChatOptions Options { get; }

        public SendbirdChatClient()
        {
            _sendbirdChatMain = new SendbirdChatMain();
            GroupChannel = new SbGroupChannelModule(_sendbirdChatMain);
            OpenChannel = new SbOpenChannelModule(_sendbirdChatMain);
            Message = new SbMessageModule(_sendbirdChatMain);
            Options = new SendbirdChatOptions(_sendbirdChatMain);
        }

        /// <summary>
        /// Initializes SendbirdChat with given app ID.
        /// </summary>
        /// <param name="inInitParams"></param>
        /// @since 4.0.0
        public void Init(SbInitParams inInitParams)
        {
            Logger.SetLogLevel(inInitParams.SbLogLevel.ToInternalLogLevel());
            _sendbirdChatMain.Initialize(inInitParams);
        }

        /// <summary>
        /// Performs a connection to Sendbird with the user ID.
        /// </summary>
        /// <param name="inUserId">The user ID.</param>
        /// <param name="inCompletionHandler">The handler block to execute. user is the object to represent the current user.</param>
        /// @since 4.0.0
        public void Connect(string inUserId, SbUserHandler inCompletionHandler)
        {
            _sendbirdChatMain.Connect(inUserId, null, null, null, inCompletionHandler);
        }

        /// <summary>
        /// Performs a connection to Sendbird with the user ID and the access token.
        /// </summary>
        /// <param name="inUserId">The user ID.</param>
        /// <param name="inAuthToken">The auth token. If the user doesnt have auth token, set nil.</param>
        /// <param name="inCompletionHandler">The handler block to execute. user is the object to represent the current user.</param>
        /// @since 4.0.0
        public void Connect(string inUserId, string inAuthToken, SbUserHandler inCompletionHandler)
        {
            _sendbirdChatMain.Connect(inUserId, inAuthToken, null, null, inCompletionHandler);
        }

        /// <summary>
        /// Performs a connection to Sendbird with the user ID and the access token.
        /// </summary>
        /// <param name="inUserId">The user ID.</param>
        /// <param name="inAuthToken">The auth token. If the user doesnt have auth token, set nil.</param>
        /// <param name="inApiHost">apiHost</param>
        /// <param name="inWsHost">wsHost</param>
        /// <param name="inCompletionHandler">The handler block to execute. user is the object to represent the current user.</param>
        /// @since 4.0.0
        public void Connect(string inUserId, string inAuthToken, string inApiHost, string inWsHost, SbUserHandler inCompletionHandler)
        {
            _sendbirdChatMain.Connect(inUserId, inAuthToken, inApiHost, inWsHost, inCompletionHandler);
        }

        /// <summary>
        /// Disconnects from Sendbird. If this method is invoked, the current user will be invalidated.
        /// </summary>
        /// <param name="inDisconnectHandler">The handler block to execute.</param>
        /// @since 4.0.0
        public void Disconnect(SbDisconnectHandler inDisconnectHandler)
        {
            _sendbirdChatMain.Disconnect(inDisconnectHandler);
        }

        /// <summary>
        /// Starts reconnection explicitly. The ConnectionDelegate delegates will be invoked by the reconnection process.
        /// </summary>
        /// <returns>true if there is the data to be used for reconnection.</returns>
        /// @since 4.0.0
        public bool Reconnect()
        {
            return _sendbirdChatMain.Reconnect();
        }

        /// <summary>
        /// Gets the SDK connection state.
        /// </summary>
        /// @since 4.0.0
        public SbConnectionState GetConnectionState()
        {
            return _sendbirdChatMain.ConnectionState();
        }

        /// <summary>
        /// Set a SessionHandler which is required for SDK refresh the session when the current session expires.
        /// </summary>
        /// <param name="inSessionHandler"></param>
        /// @since 4.0.0
        public void SetSessionHandler(SbSessionHandler inSessionHandler)
        {
            _sendbirdChatMain.SetSessionHandler(inSessionHandler);
        }

        /// <summary>
        /// Sets group channel invitation preference for auto acceptance.
        /// </summary>
        /// <param name="inAutoAccept">If true, the current user will accept the group channel invitation automatically.</param>
        /// <param name="inCompletionHandler">The handler block to execute.</param>
        /// @since 4.0.0
        public void SetChannelInvitationPreference(bool inAutoAccept, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatMain.SetChannelInvitationPreference(inAutoAccept, inCompletionHandler);
        }

        /// <summary>
        /// Gets group channel invitation preference for auto acceptance.
        /// </summary>
        /// <param name="inCompletionHandler">The handler block to execute.</param>
        /// @since 4.0.0
        public void GetChannelInvitationPreference(SbChannelInvitationPreferenceHandler inCompletionHandler)
        {
            _sendbirdChatMain.GetChannelInvitationPreference(inCompletionHandler);
        }

        /// <summary>
        /// Adds a user event handler. All added handlers will be notified when events occur.
        /// </summary>
        /// <param name="inIdentifier"></param>
        /// <param name="inUserEventHandler"></param>
        /// @since 4.0.0
        public void AddUserEventHandler(string inIdentifier, SbUserEventHandler inUserEventHandler)
        {
            _sendbirdChatMain.AddUserEventHandler(inIdentifier, inUserEventHandler);
        }

        /// <summary>
        /// Removes a user event handler. The deleted handler no longer be notified.
        /// </summary>
        /// <param name="inIdentifier"></param>
        /// @since 4.0.0
        public void RemoveUserEventHandler(string inIdentifier)
        {
            _sendbirdChatMain.RemoveUserEventHandler(inIdentifier);
        }

        /// <summary>
        /// Removes all user event handlers added by AddUseEventHandler.
        /// </summary>
        /// @since 4.0.0
        public void RemoveAllUserEventHandlers()
        {
            _sendbirdChatMain.RemoveAllUserEventHandlers();
        }

        /// <summary>
        /// Adds a connection handler. All added handlers will be notified when events occurs.
        /// </summary>
        /// <param name="inIdentifier"></param>
        /// <param name="inConnectionHandler"></param>
        /// @since 4.0.0
        public void AddConnectionHandler(string inIdentifier, SbConnectionHandler inConnectionHandler)
        {
            _sendbirdChatMain.AddConnectionHandler(inIdentifier, inConnectionHandler);
        }

        /// <summary>
        /// Removes a connection handler. The deleted handler no longer be notified.
        /// </summary>
        /// <param name="inIdentifier"></param>
        /// @since 4.0.0
        public void RemoveConnectionHandler(string inIdentifier)
        {
            _sendbirdChatMain.RemoveConnectionHandler(inIdentifier);
        }

        /// <summary>
        /// Removes all connection handlers added by AddConnectionHandler.
        /// </summary>
        /// @since 4.0.0
        public void RemoveAllConnectionHandlers()
        {
            _sendbirdChatMain.RemoveAllConnectionHandlers();
        }

        /// <summary>
        /// Blocks the specified User ID. Blocked User cannot send messages to the blocker.
        /// </summary>
        /// <param name="inUserId"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void BlockUser(string inUserId, SbUserHandler inCompletionHandler)
        {
            _sendbirdChatMain.BlockUser(inUserId, inCompletionHandler);
        }

        /// <summary>
        /// Unblocks the specified User ID. Unblocked User cannot send messages to the ex-blocker.
        /// </summary>
        /// <param name="inUserId"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void UnblockUser(string inUserId, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatMain.UnblockUser(inUserId, inCompletionHandler);
        }

        /// <summary>
        /// Updates current User's information.
        /// </summary>
        /// <param name="inParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void UpdateCurrentUserInfo(SbUserUpdateParams inParams, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatMain.UpdateCurrentUserInfo(inParams, inCompletionHandler);
        }

        /// <summary>
        /// Updates current User's information.
        /// </summary>
        /// <param name="inPreferredLanguages">New array of preferred languages</param>
        /// <param name="inCompletionHandler">The handler block to execute.</param>
        /// @since 4.0.0
        public void UpdateCurrentUserInfo(List<string> inPreferredLanguages, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatMain.UpdateCurrentUserPreferredLanguages(inPreferredLanguages, inCompletionHandler);
        }

        /// <summary>
        /// Creates a query instance to get the whole User list.
        /// </summary>
        /// @since 4.0.0
        public SbApplicationUserListQuery CreateApplicationUserListQuery(SbApplicationUserListQueryParams inParams = null)
        {
            return _sendbirdChatMain.CreateApplicationUserListQuery(inParams);
        }

        /// <summary>
        /// Creates a query instance to get only the blocked User (by me) list.
        /// </summary>
        /// <param name="inParams"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public SbBlockedUserListQuery CreateBlockedUserListQuery(SbBlockedUserListQueryParams inParams = null)
        {
            return _sendbirdChatMain.CreateBlockedUserListQuery(inParams);
        }

        /// <summary>
        /// Creates a query instance to search for a message.
        /// </summary>
        /// <param name="inParams"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public SbMessageSearchQuery CreateMessageSearchQuery(SbMessageSearchQueryParams inParams = null)
        {
            return _sendbirdChatMain.CreateMessageSearchQuery(inParams);
        }

        /// <summary>
        /// Registers push token for the current User to receive push notification.
        /// </summary>
        /// <param name="inPushTokenType">The enum type to represent the type of push token.</param>
        /// <param name="inPushToken">Device token.</param>
        /// <param name="inUnique">If true, register device token after removing existing all device tokens of the current user. If false, just add the device token.</param>
        /// <param name="inCompletionHandler">The handler block to execute. status is the status for push token registration. It is defined in SbPushTokenRegistrationStatus.Success represents the devToken is registered. Pending represents the devToken is not registered because the connection is not established, so this method has to be invoked with getPendingPushToken method after the connection. Error represents the push token registration is failed.</param>
        /// @since 4.0.0
        public void RegisterPushToken(SbPushTokenType inPushTokenType, string inPushToken, bool inUnique, SbPushTokenRegistrationStatusHandler inCompletionHandler)
        {
            _sendbirdChatMain.RegisterPushToken(inPushTokenType, inPushToken, inUnique, inCompletionHandler);
        }

        /// <summary>
        /// Unregisters push token for the current User.
        /// </summary>
        /// <param name="inPushTokenType"></param>
        /// <param name="inPushToken"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void UnregisterPushToken(SbPushTokenType inPushTokenType, string inPushToken, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatMain.UnregisterPushToken(inPushTokenType, inPushToken, inCompletionHandler);
        }

        /// <summary>
        /// Unregisters all push token bound to the current User.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void UnregisterAllPushToken(SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatMain.UnregisterAllPushToken(inCompletionHandler);
        }

        /// <summary>
        /// Requests push tokens of current user from given token. The result is passed to handler.
        /// </summary>
        /// <param name="inToken">The token used to get next pagination of device push tokens.</param>
        /// <param name="inPushTokenType">The enum type to represent the type of push token.</param>
        /// <param name="inCompletionHandler">The handler block to be executed after requests. This block has no return value and takes 5 arguments that are device push token list, push token type you are requesting, boolean that indicates having next pagination, token to be used next pagination and error.</param>
        /// @since 4.0.0
        public void GetMyPushTokensByToken(string inToken, SbPushTokenType inPushTokenType, SbPushTokensHandler inCompletionHandler)
        {
            _sendbirdChatMain.GetMyPushTokensByToken(inToken, inPushTokenType, inCompletionHandler);
        }

        /// <summary>
        /// Sets push template option for the current User.
        /// </summary>
        /// <param name="inTemplateName">The name of push template.</param>
        /// <param name="inCompletionHandler">The handler block to execute.</param>
        /// @since 4.0.0
        public void SetPushTemplate(string inTemplateName, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatMain.SetPushTemplate(inTemplateName, inCompletionHandler);
        }

        /// <summary>
        /// Gets push template option for the current User. For details of push template option, refer to SetPushTemplate. This can be used, for instance, when you need to check the push notification content preview is on or off at the moment.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetPushTemplate(SbPushTemplateHandler inCompletionHandler)
        {
            _sendbirdChatMain.GetPushTemplate(inCompletionHandler);
        }

        /// <summary>
        /// Sets snooze period for the current User. If this option is enabled, the current User does not receive push notification during the given period. It's not a repetitive operation. If you want to snooze repeatedly, use SetDoNotDisturb.
        /// </summary>
        /// <param name="inEnabled">Enabled means snooze remote push notification in duration. If set to disabled, current user can receive remote push notification.</param>
        /// <param name="inStartTimestamp">Unix timestamp to start snooze.</param>
        /// <param name="inEndTimestamp">Unix timestamp to end snooze.</param>
        /// <param name="inCompletionHandler">The handler block to execute when setting notification snoozed is complete.</param>
        /// @since 4.0.0
        public void SetSnoozePeriod(bool inEnabled, long inStartTimestamp, long inEndTimestamp, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatMain.SetSnoozePeriod(inEnabled, inStartTimestamp, inEndTimestamp, inCompletionHandler);
        }

        /// <summary>
        /// Gets snooze period for the current User.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetSnoozePeriod(SbSnoozePeriodHandler inCompletionHandler)
        {
            _sendbirdChatMain.GetSnoozePeriod(inCompletionHandler);
        }

        /// <summary>
        /// Sets Do-not-disturb option for the current User. If this option is enabled, the current User does not receive push notification during the specified time repeatedly. If you want to snooze specific period, use SetSnoozePeriod.
        /// </summary>
        /// <param name="inEnabled">Enables or not.</param>
        /// <param name="inStartHour">Start hour.</param>
        /// <param name="inStartMin">Start minute.</param>
        /// <param name="inEndHour">End hour.</param>
        /// <param name="inEndMin">End minute.</param>
        /// <param name="inTimezone">Sets timezone.</param>
        /// <param name="inCompletionHandler">The handler block to execute.</param>
        /// @since 4.0.0
        public void SetDoNotDisturb(bool inEnabled, int inStartHour, int inStartMin, int inEndHour, int inEndMin, string inTimezone, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatMain.SetDoNotDisturb(inEnabled, inStartHour, inStartMin, inEndHour, inEndMin, inTimezone, inCompletionHandler);
        }

        /// <summary>
        /// Gets Do-not-disturb option for the current User.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetDoNotDisturb(SbDoNotDisturbHandler inCompletionHandler)
        {
            _sendbirdChatMain.GetDoNotDisturb(inCompletionHandler);
        }
    }
}