// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// @since 4.0.0
    public static partial class SendbirdChat
    {
        private static readonly ISendbirdChatClient _sendbirdChatClient = new SendbirdChatClient();

        /// <summary>
        /// Current SDK version.
        /// </summary>
        /// @since 4.0.0
        public static string SDKVersion => _sendbirdChatClient.SDKVersion;

        /// <summary>
        /// Current OS version.
        /// </summary>
        /// @since 4.0.0
        public static string OSVersion => _sendbirdChatClient.OSVersion;

        /// <summary>
        /// Current application ID.
        /// </summary>
        /// @since 4.0.0
        public static string ApplicationId => _sendbirdChatClient.ApplicationId;

        /// <summary>
        /// The key to authenticate the url retrieved from SbFileMessage.PlainUrl, SbUser.PlainProfileImageUrl and SbThumbnail.PlainUrl. This key has to be put into the HTTP header to access the url provided by above methods.
        /// </summary>
        /// @since 4.0.0
        public static string EKey => _sendbirdChatClient.EKey;

        /// <summary>
        /// Represents information obtained from the application settings.
        /// </summary>
        /// @since 4.0.0
        public static SbAppInfo AppInfo => _sendbirdChatClient.AppInfo;

        /// <summary>
        /// The current connected User. null if connect is not called.
        /// </summary>
        /// @since 4.0.0
        public static SbUser CurrentUser => _sendbirdChatClient.CurrentUser;

        /// <summary>
        /// Gets the SDK connection state.
        /// </summary>
        /// @since 4.0.0
        public static SbConnectionState GetConnectionState() => _sendbirdChatClient.GetConnectionState();

        /// <summary>
        /// SbGroupChannelModule class.
        /// </summary>
        /// @since 4.0.0
        public static SbGroupChannelModule GroupChannel => _sendbirdChatClient.GroupChannel;

        /// <summary>
        /// SbOpenChannelModule class.
        /// </summary>
        /// @since 4.0.0
        public static SbOpenChannelModule OpenChannel => _sendbirdChatClient.OpenChannel;

        /// <summary>
        /// SbMessageModule class.
        /// </summary>
        /// @since 4.0.0
        public static SbMessageModule Message => _sendbirdChatClient.Message;

        /// <summary>
        /// SendbirdChatOptions class.
        /// </summary>
        /// @since 4.0.0
        public static SendbirdChatOptions Options => _sendbirdChatClient.Options;

        /// <summary>
        /// Initializes SendbirdChat with given app ID.
        /// </summary>
        /// <param name="inInitParams"></param>
        /// @since 4.0.0
        public static void Init(SbInitParams inInitParams)
        {
            Logger.SetLogLevel(inInitParams.SbLogLevel.ToInternalLogLevel());
            _sendbirdChatClient.Init(inInitParams);
        }

        /// <summary>
        /// Performs a connection to Sendbird with the user ID.
        /// </summary>
        /// <param name="inUserId">The user ID.</param>
        /// <param name="inCompletionHandler">The handler block to execute. user is the object to represent the current user.</param>
        /// @since 4.0.0
        public static void Connect(string inUserId, SbUserHandler inCompletionHandler)
        {
            _sendbirdChatClient.Connect(inUserId, null, null, null, inCompletionHandler);
        }

        /// <summary>
        /// Performs a connection to Sendbird with the user ID and the access token.
        /// </summary>
        /// <param name="inUserId">The user ID.</param>
        /// <param name="inAuthToken">The auth token. If the user doesn't have auth token, set nil.</param>
        /// <param name="inCompletionHandler">The handler block to execute. user is the object to represent the current user.</param>
        /// @since 4.0.0
        public static void Connect(string inUserId, string inAuthToken, SbUserHandler inCompletionHandler)
        {
            _sendbirdChatClient.Connect(inUserId, inAuthToken, null, null, inCompletionHandler);
        }

        /// <summary>
        /// Performs a connection to Sendbird with the user ID and the access token.
        /// </summary>
        /// <param name="inUserId">The user ID.</param>
        /// <param name="inAuthToken">The auth token. If the user doesn't have auth token, set nil.</param>
        /// <param name="inApiHost">apiHost</param>
        /// <param name="inWsHost">wsHost</param>
        /// <param name="inCompletionHandler">The handler block to execute. user is the object to represent the current user.</param>
        /// @since 4.0.0
        public static void Connect(string inUserId, string inAuthToken, string inApiHost, string inWsHost, SbUserHandler inCompletionHandler)
        {
            _sendbirdChatClient.Connect(inUserId, inAuthToken, inApiHost, inWsHost, inCompletionHandler);
        }

        /// <summary>
        /// Disconnects from Sendbird. If this method is invoked, the current user will be invalidated.
        /// </summary>
        /// <param name="inDisconnectHandler">The handler block to execute.</param>
        /// @since 4.0.0
        public static void Disconnect(SbDisconnectHandler inDisconnectHandler)
        {
            _sendbirdChatClient.Disconnect(inDisconnectHandler);
        }

        /// <summary>
        /// Starts reconnection explicitly. The ConnectionDelegate delegates will be invoked by the reconnection process.
        /// </summary>
        /// <returns>true if there is the data to be used for reconnection.</returns>
        /// @since 4.0.0
        public static bool Reconnect()
        {
            return _sendbirdChatClient.Reconnect();
        }

        /// <summary>
        /// Sets group channel invitation preference for auto acceptance.
        /// </summary>
        /// <param name="inAutoAccept">If true, the current user will accept the group channel invitation automatically.</param>
        /// <param name="inCompletionHandler">The handler block to execute.</param>
        /// @since 4.0.0
        public static void SetChannelInvitationPreference(bool inAutoAccept, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatClient.SetChannelInvitationPreference(inAutoAccept, inCompletionHandler);
        }

        /// <summary>
        /// Gets group channel invitation preference for auto acceptance.
        /// </summary>
        /// <param name="inCompletionHandler">The handler block to execute.</param>
        /// @since 4.0.0
        public static void GetChannelInvitationPreference(SbChannelInvitationPreferenceHandler inCompletionHandler)
        {
            _sendbirdChatClient.GetChannelInvitationPreference(inCompletionHandler);
        }

        /// <summary>
        /// Set a SessionHandler which is required for SDK refresh the session when the current session expires.
        /// </summary>
        /// <param name="inSessionHandler"></param>
        /// @since 4.0.0
        public static void SetSessionHandler(SbSessionHandler inSessionHandler)
        {
            _sendbirdChatClient.SetSessionHandler(inSessionHandler);
        }

        /// <summary>
        /// Adds a user event handler. All added handlers will be notified when events occur.
        /// </summary>
        /// <param name="inIdentifier"></param>
        /// <param name="inUserEventHandler"></param>
        /// @since 4.0.0
        public static void AddUserEventHandler(string inIdentifier, SbUserEventHandler inUserEventHandler)
        {
            _sendbirdChatClient.AddUserEventHandler(inIdentifier, inUserEventHandler);
        }

        /// <summary>
        /// Removes a user event handler. The deleted handler no longer be notified.
        /// </summary>
        /// <param name="inIdentifier"></param>
        /// @since 4.0.0
        public static void RemoveUserEventHandler(string inIdentifier)
        {
            _sendbirdChatClient.RemoveUserEventHandler(inIdentifier);
        }

        /// <summary>
        /// Removes all user event handlers added by AddUseEventHandler.
        /// </summary>
        /// @since 4.0.0
        public static void RemoveAllUserEventHandlers()
        {
            _sendbirdChatClient.RemoveAllUserEventHandlers();
        }

        /// <summary>
        /// Adds a connection handler. All added handlers will be notified when events occurs.
        /// </summary>
        /// <param name="inIdentifier"></param>
        /// <param name="inConnectionHandler"></param>
        /// @since 4.0.0
        public static void AddConnectionHandler(string inIdentifier, SbConnectionHandler inConnectionHandler)
        {
            _sendbirdChatClient.AddConnectionHandler(inIdentifier, inConnectionHandler);
        }

        /// <summary>
        /// Removes a connection handler. The deleted handler no longer be notified.
        /// </summary>
        /// <param name="inIdentifier"></param>
        /// @since 4.0.0
        public static void RemoveConnectionHandler(string inIdentifier)
        {
            _sendbirdChatClient.RemoveConnectionHandler(inIdentifier);
        }

        /// <summary>
        /// Removes all connection handlers added by AddConnectionHandler.
        /// </summary>
        /// @since 4.0.0
        public static void RemoveAllConnectionHandlers()
        {
            _sendbirdChatClient.RemoveAllConnectionHandlers();
        }

        /// <summary>
        /// Blocks the specified User ID. Blocked User cannot send messages to the blocker.
        /// </summary>
        /// <param name="inUserId"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public static void BlockUser(string inUserId, SbUserHandler inCompletionHandler)
        {
            _sendbirdChatClient.BlockUser(inUserId, inCompletionHandler);
        }

        /// <summary>
        /// Unblocks the specified User ID. Unblocked User cannot send messages to the ex-blocker.
        /// </summary>
        /// <param name="inUserId"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public static void UnblockUser(string inUserId, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatClient.UnblockUser(inUserId, inCompletionHandler);
        }

        /// <summary>
        /// Creates a query instance to get the whole User list.
        /// </summary>
        /// @since 4.0.0
        public static SbApplicationUserListQuery CreateApplicationUserListQuery(SbApplicationUserListQueryParams inParams = null)
        {
            return _sendbirdChatClient.CreateApplicationUserListQuery(inParams);
        }

        /// <summary>
        /// Creates a query instance to get only the blocked User (by me) list.
        /// </summary>
        /// <param name="inParams"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public static SbBlockedUserListQuery CreateBlockedUserListQuery(SbBlockedUserListQueryParams inParams = null)
        {
            return _sendbirdChatClient.CreateBlockedUserListQuery(inParams);
        }

        /// <summary>
        /// Creates a query instance to search for a message.
        /// </summary>
        /// <param name="inParams"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public static SbMessageSearchQuery CreateMessageSearchQuery(SbMessageSearchQueryParams inParams = null)
        {
            return _sendbirdChatClient.CreateMessageSearchQuery(inParams);
        }

        /// <summary>
        /// Updates current User's information.
        /// </summary>
        /// <param name="inParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public static void UpdateCurrentUserInfo(SbUserUpdateParams inParams, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatClient.UpdateCurrentUserInfo(inParams, inCompletionHandler);
        }

        /// <summary>
        /// Updates current User's information.
        /// </summary>
        /// <param name="inPreferredLanguages">New array of preferred languages</param>
        /// <param name="inCompletionHandler">The handler block to execute.</param>
        /// @since 4.0.0
        public static void UpdateCurrentUserInfo(List<string> inPreferredLanguages, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatClient.UpdateCurrentUserInfo(inPreferredLanguages, inCompletionHandler);
        }

        /// <summary>
        /// Registers push token for the current User to receive push notification.
        /// </summary>
        /// <param name="inPushTokenType">The enum type to represent the type of push token.</param>
        /// <param name="inPushToken">Device token.</param>
        /// <param name="inUnique">If true, register device token after removing existing all device tokens of the current user. If false, just add the device token.</param>
        /// <param name="inCompletionHandler">The handler block to execute. status is the status for push token registration. It is defined in SbPushTokenRegistrationStatus.Success represents the devToken is registered. Pending represents the devToken is not registered because the connection is not established, so this method has to be invoked with getPendingPushToken method after the connection. Error represents the push token registration is failed.</param>
        /// @since 4.0.0
        public static void RegisterPushToken(SbPushTokenType inPushTokenType, string inPushToken, bool inUnique, SbPushTokenRegistrationStatusHandler inCompletionHandler)
        {
            _sendbirdChatClient.RegisterPushToken(inPushTokenType, inPushToken, inUnique, inCompletionHandler);
        }

        /// <summary>
        /// Unregisters push token for the current User.
        /// </summary>
        /// <param name="inPushTokenType"></param>
        /// <param name="inPushToken"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public static void UnregisterPushToken(SbPushTokenType inPushTokenType, string inPushToken, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatClient.UnregisterPushToken(inPushTokenType, inPushToken, inCompletionHandler);
        }

        /// <summary>
        /// Unregisters all push token bound to the current User.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public static void UnregisterAllPushToken(SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatClient.UnregisterAllPushToken(inCompletionHandler);
        }

        /// <summary>
        /// Requests push tokens of current user from given token. The result is passed to handler.
        /// </summary>
        /// <param name="inToken">The token used to get next pagination of device push tokens.</param>
        /// <param name="inPushTokenType">The enum type to represent the type of push token.</param>
        /// <param name="inCompletionHandler">The handler block to be executed after requests. This block has no return value and takes 5 arguments that are device push token list, push token type you are requesting, boolean that indicates having next pagination, token to be used next pagination and error.</param>
        /// @since 4.0.0
        public static void GetMyPushTokensByToken(string inToken, SbPushTokenType inPushTokenType, SbPushTokensHandler inCompletionHandler)
        {
            _sendbirdChatClient.GetMyPushTokensByToken(inToken, inPushTokenType, inCompletionHandler);
        }

        /// <summary>
        /// Sets push template option for the current User.
        /// </summary>
        /// <param name="inTemplateName">The name of push template.</param>
        /// <param name="inCompletionHandler">The handler block to execute.</param>
        /// @since 4.0.0
        public static void SetPushTemplate(string inTemplateName, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatClient.SetPushTemplate(inTemplateName, inCompletionHandler);
        }

        /// <summary>
        /// Gets push template option for the current User. For details of push template option, refer to SetPushTemplate. This can be used, for instance, when you need to check the push notification content preview is on or off at the moment.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public static void GetPushTemplate(SbPushTemplateHandler inCompletionHandler)
        {
            _sendbirdChatClient.GetPushTemplate(inCompletionHandler);
        }

        /// <summary>
        /// Sets snooze period for the current User. If this option is enabled, the current User does not receive push notification during the given period. It's not a repetitive operation. If you want to snooze repeatedly, use SetDoNotDisturb.
        /// </summary>
        /// <param name="inEnabled">Enabled means snooze remote push notification in duration. If set to disabled, current user can receive remote push notification.</param>
        /// <param name="inStartTimestamp">Unix timestamp to start snooze.</param>
        /// <param name="inEndTimestamp">Unix timestamp to end snooze.</param>
        /// <param name="inCompletionHandler">The handler block to execute when setting notification snoozed is complete.</param>
        /// @since 4.0.0
        public static void SetSnoozePeriod(bool inEnabled, long inStartTimestamp, long inEndTimestamp, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatClient.SetSnoozePeriod(inEnabled, inStartTimestamp, inEndTimestamp, inCompletionHandler);
        }

        /// <summary>
        /// Gets snooze period for the current User.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public static void GetSnoozePeriod(SbSnoozePeriodHandler inCompletionHandler)
        {
            _sendbirdChatClient.GetSnoozePeriod(inCompletionHandler);
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
        public static void SetDoNotDisturb(bool inEnabled, int inStartHour, int inStartMin, int inEndHour, int inEndMin, string inTimezone, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatClient.SetDoNotDisturb(inEnabled, inStartHour, inStartMin, inEndHour, inEndMin, inTimezone, inCompletionHandler);
        }

        /// <summary>
        /// Gets Do-not-disturb option for the current User.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public static void GetDoNotDisturb(SbDoNotDisturbHandler inCompletionHandler)
        {
            _sendbirdChatClient.GetDoNotDisturb(inCompletionHandler);
        }

        /// <summary>
        /// Requests a setting that decides which push notification for the current user to receive in all the group channel.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.1.0
        public static void GetPushTriggerOption(SbPushTriggerOptionHandler inCompletionHandler)
        {
            _sendbirdChatClient.GetPushTriggerOption(inCompletionHandler);
        }

        /// <summary>
        /// Changes a setting that decides which push notification for the current user to receive in all the group channel.
        /// </summary>
        /// <param name="inPushTriggerOption"></param>
        /// <param name="inCompletionHandler"></param>
        /// /// @since 4.1.0
        public static void SetPushTriggerOption(SbPushTriggerOption inPushTriggerOption, SbErrorHandler inCompletionHandler)
        {
            _sendbirdChatClient.SetPushTriggerOption(inPushTriggerOption, inCompletionHandler);
        }

        /// <summary>
        /// Sets the log level. The log level is defined by SbLogLevel.
        /// </summary>
        /// <param name="inLogLevel"></param>
        /// @since 4.0.0
        public static void SetLogLevel(SbLogLevel inLogLevel)
        {
            Logger.SetLogLevel(inLogLevel.ToInternalLogLevel());
        }
    }
}