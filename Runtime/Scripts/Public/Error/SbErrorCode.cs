// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Error Code that represents the type of the error.
    /// </summary>
    /// @since 4.0.0
    public enum SbErrorCode
    {
        /// <summary>
        /// UnknownError
        /// </summary>
        /// @since 4.0.0
        UnknownError = 800000,

        /// <summary>
        /// The initialization of the SendbirdChat instance failed because the value assigned to APP_ID upon initializing wasn't valid.
        /// </summary>
        /// @since 4.0.0
        InvalidInitialization = 800100,

        /// <summary>
        /// The request from a client app failed because the device wasn't connected to the server.
        /// </summary>
        /// @since 4.0.0
        ConnectionRequired = 800101,

        /// <summary>
        /// The connection is canceled or the disconnecting method is called while the SendbirdChat instance is trying to connect to the server.
        /// </summary>
        /// @since 4.0.0
        ConnectionCanceled = 800102,

        /// <summary>
        /// Invalid or empty value.
        /// </summary>
        /// @since 4.0.0
        InvalidParameter = 800110,

        /// <summary>
        /// The connection failed due to the unstable network or an unexpected error in the Chat SDK network library.
        /// </summary>
        /// @since 4.0.0
        NetworkError = 800120,

        /// <summary>
        /// The request routing to the server failed.
        /// </summary>
        /// @since 4.0.0
        NetworkRoutingError = 800121,

        /// <summary>
        /// The data format of the server response is invalid.
        /// </summary>
        /// @since 4.0.0
        MalformedData = 800130,

        /// <summary>
        /// The data format of the error message is invalid due to the problem with the request.
        /// </summary>
        /// @since 4.0.0
        MalformedErrorData = 800140,

        /// <summary>
        /// The specified channel type in the request is invalid.
        /// </summary>
        /// @since 4.0.0
        WrongChannelType = 800150,

        /// <summary>
        /// The interval between the successive requests is less than a second.
        /// </summary>
        /// @since 4.0.0
        MarkAsReadRateLimitExceeded = 800160,

        /// <summary>
        /// A retrieval request is arriving while the server is still processing the previous retrieval request for channels, messages, or users, and in preparation to send the response.
        /// </summary>
        /// @since 4.0.0
        QueryInProgress = 800170,

        /// <summary>
        /// The server failed to send a response for the request in  a certain amount of seconds.
        /// </summary>
        /// @since 4.0.0
        AckTimeout = 800180,

        /// <summary>
        /// The server failed to send a response for the SendbirdChat instance's login request in 10 seconds.
        /// </summary>
        /// @since 4.0.0
        LoginTimeout = 800190,

        /// <summary>
        /// The request was submitted while disconnected from the server.
        /// </summary>
        /// @since 4.0.0
        WebSocketConnectionClosed = 800200,

        /// <summary>
        /// The websocket connection to the server failed to establish.
        /// </summary>
        /// @since 4.0.0
        WebSocketConnectionFailed = 800210,

        /// <summary>
        /// The server failed to process the request due to an internal reason.
        /// </summary>
        /// @since 4.0.0
        RequestFailed = 800220,

        /// <summary>
        /// The request to cancel file upload failed due to an unexpected error.
        /// </summary>
        /// @since 4.0.0
        FileUploadCancelFailed = 800230,

        /// <summary>
        /// The file upload request is canceled.
        /// </summary>
        /// @since 4.0.0
        FileUploadCanceled = 800240,

        /// <summary>
        /// A time-out error signaling that the Sendbird server has failed to complete the file upload request in the time period allowed.
        /// </summary>
        /// @since 4.0.0
        FileUploadTimeout = 800250,

        /// <summary>
        /// An action wasn’t completed within a certain time frame.
        /// </summary>
        /// @since 4.0.0
        TimerWasExpired = 800310,

        /// <summary>
        /// Actions were completed multiple times when the action should have been completed just once.
        /// </summary>
        /// @since 4.0.0
        TimerWasAlreadyDone = 800302,

        /// <summary>
        /// Message status doesn't match.
        /// </summary>
        /// @since 4.0.0
        PendingError = 800400,

        /// <summary>
        /// The client app failed to refresh a session with the given token because the token is invalid.
        /// </summary>
        /// @since 4.0.0
        PassedInvalidAccessToken = 800500,

        /// <summary>
        /// The session has expired and was refreshed with the given token.
        /// </summary>
        /// @since 4.0.0
        SessionKeyRefreshSucceeded = 800501,

        /// <summary>
        /// The session has expired but the client app failed to refresh the session with the given token.
        /// </summary>
        /// @since 4.0.0
        SessionKeyRefreshFailed = 800502,

        /// <summary>
        /// The client app attempted to use a message collection or a channel collection after it has already been disposed of.
        /// </summary>
        /// @since 4.0.0
        CollectionDisposed = 800600,

        /// <summary>
        /// The client app attempted to use local database with local caching.
        /// </summary>
        /// @since 4.0.0
        LocalDatabaseError = 800700,

        /// <summary>
        /// The request specifies one or more parameters in an unexpected data type. The data type of the parameters should be string.
        /// </summary>
        /// @since 4.0.0
        UnexpectedParameterTypeString = 400100,

        /// <summary>
        /// The request specifies one or more parameters in an unexpected data type. The data type of the parameters should be number.
        /// </summary>
        /// @since 4.0.0
        UnexpectedParameterTypeNumber = 400101,

        /// <summary>
        /// The request specifies one or more parameters in an unexpected data type. The data type of the parameters should be list.
        /// </summary>
        /// @since 4.0.0
        UnexpectedParameterTypeList = 400102,

        /// <summary>
        /// The request specifies one or more parameters in an unexpected data type. The data type of the parameters should be JSON.
        /// </summary>
        /// @since 4.0.0
        UnexpectedParameterTypeJson = 400103,

        /// <summary>
        /// The request specifies one or more parameters in an unexpected data type. The data type of the parameters should be boolean.
        /// </summary>
        /// @since 4.0.0
        UnexpectedParameterTypeBoolean = 400104,

        /// <summary>
        /// The request is missing one or more required parameters.
        /// </summary>
        /// @since 4.0.0
        MissingRequiredParameters = 400105,

        /// <summary>
        /// The parameter specifies an invalid negative number. Its value should be a positive number.
        /// </summary>
        /// @since 4.0.0
        NegativeNumberNotAllowed = 400106,

        /// <summary>
        /// The request isn't authorized and can't access the requested resource.
        /// </summary>
        /// @since 4.0.0
        UnauthorizedRequest = 400108,

        /// <summary>
        /// The value of the token parameter for pagination has expired.
        /// </summary>
        /// @since 4.0.0
        ExpiredPageToken = 400109,

        /// <summary>
        /// The length of the parameter value is too long.
        /// </summary>
        /// @since 4.0.0
        ParameterValueLengthExceeded = 400110,

        /// <summary>
        /// The request specifies an invalid value.
        /// </summary>
        /// @since 4.0.0
        InvalidValue = 400111,

        /// <summary>
        /// The two parameters of the request, which should have unique values, specify the same value.
        /// </summary>
        /// @since 4.0.0
        IncompatibleValues = 400112,

        /// <summary>
        /// The request specifies one or more parameters which are outside the allowed value range.
        /// </summary>
        /// @since 4.0.0
        ParameterValueOutOfRange = 400113,

        /// <summary>
        /// The resource identified with the URL in the request can't be found.
        /// </summary>
        /// @since 4.0.0
        InvalidURLOfResource = 400114,

        /// <summary>
        /// The request specifies an illegal value containing special character, empty string, or white space.
        /// </summary>
        /// @since 4.0.0
        NotAllowedCharacter = 400151,

        /// <summary>
        /// The resource identified with the request's resourceId parameter can't be found.
        /// </summary>
        /// @since 4.0.0
        ResourceNotFound = 400201,

        /// <summary>
        /// The resource identified with the request's resourceId parameter already exists.
        /// </summary>
        /// @since 4.0.0
        ResourceAlreadyExists = 400202,

        /// <summary>
        /// The parameter specifies more items than allowed.
        /// </summary>
        /// @since 4.0.0
        TooManyItemsInParameter = 400203,

        /// <summary>
        /// The request can't retrieve the deactivated user data.
        /// </summary>
        /// @since 4.0.0
        DeactivatedUserNotAccessible = 400300,

        /// <summary>
        /// The user identified with the request's userId parameter can't be found because either the user doesn't exist or has been deleted.
        /// </summary>
        /// @since 4.0.0
        UserNotFound = 400301,

        /// <summary>
        /// The access token provided for the request specifies an invalid value.
        /// </summary>
        /// @since 4.0.0
        InvalidAccessToken = 400302,

        /// <summary>
        /// The session key provided for the request specifies an invalid value.
        /// </summary>
        /// @since 4.0.0
        InvalidSessionKeyValue = 400303,

        /// <summary>
        /// The application identified with the request can't be found.
        /// </summary>
        /// @since 4.0.0
        ApplicationNotFound = 400304,

        /// <summary>
        /// The length of the userId parameter value is too long.
        /// </summary>
        /// @since 4.0.0
        UserIdLengthExceeded = 400305,

        /// <summary>
        /// The request can't be completed because you have exceeded your paid quota.
        /// </summary>
        /// @since 4.0.0
        PaidQuotaExceeded = 400306,

        /// <summary>
        /// The request can't be completed because it came from the restricted domain.
        /// </summary>
        /// @since 4.0.0
        DomainNotAllowed = 400307,

        /// <summary>
        /// The session key of the user has been expired. A new one must be issued.
        /// </summary>
        /// @since 4.0.0
        SessionKeyExpired = 400309,

        /// <summary>
        /// The session key of the user has been revoked. A new one must be issued.
        /// </summary>
        /// @since 4.0.0
        SessionTokenRevoked = 400310,

        /// <summary>
        /// The API token provided for the request specifies an invalid value.
        /// </summary>
        /// @since 4.0.0
        InvalidApiToken = 400401,

        /// <summary>
        /// The request is missing one or more necessary parameters.
        /// </summary>
        /// @since 4.0.0
        MissingSomeParameters = 400402,

        /// <summary>
        /// The request body is an invalid JSON.
        /// </summary>
        /// @since 4.0.0
        InvalidJsonRequestBody = 400403,

        /// <summary>
        /// The request specifies an invalid HTTP request URL that can't be accessed.
        /// </summary>
        /// @since 4.0.0
        InvalidRequestURL = 400404,

        /// <summary>
        /// The number of the user's websocket connections exceeds the allowed amount.
        /// </summary>
        /// @since 4.0.0
        TooManyUserWebsocketConnections = 400500,

        /// <summary>
        /// The number of the application's websocket connections exceeds the allowed amount.
        /// </summary>
        /// @since 4.0.0
        TooManyApplicationWebsocketConnections = 400501,

        /// <summary>
        /// The request can't be completed due to one of the following reasons: The sender is blocked by the recipient or has been deactivated, the message is longer than the maximum message length, or the message contains texts or URLs blocked by application settings or filters.
        /// </summary>
        /// @since 4.0.0
        BlockedUserSendMessageNotAllowed = 400700,

        /// <summary>
        /// The request can't be completed because the blocking user is trying to invite the blocked user to a channel.
        /// </summary>
        /// @since 4.0.0
        BlockedUserInvitedNotAllowed = 400701,

        /// <summary>
        /// The request can't be completed because the blocked user is trying to invite the blocking user to a channel.
        /// </summary>
        /// @since 4.0.0
        BlockedUserInviteNotAllowed = 400702,

        /// <summary>
        /// The request can't be completed because the user is trying to enter a banned channel.
        /// </summary>
        /// @since 4.0.0
        BannedUserEnterChannelNotAllowed = 400750,

        /// <summary>
        /// The request can't be completed because the user is trying to enter a banned custom type channel.
        /// </summary>
        /// @since 4.0.0
        BannedUserEnterCustomChannelNotAllowed = 400751,

        /// <summary>
        /// The request failed because the combination of parameter values is invalid. Even if each parameter value is valid, a combination of parameter values becomes invalid when it doesn't follow specific conditions.
        /// </summary>
        /// @since 4.0.0
        Unacceptable = 400920,

        /// <summary>
        /// The request failed because it is sent to an invalid endpoint that no longer exists.
        /// </summary>
        /// @since 4.0.0
        InvalidEndpoint = 400930,

        /// <summary>
        /// The application identified with the request isn't available.
        /// </summary>
        /// @since 4.0.0
        ApplicationNotAvailable = 403100,

        /// <summary>
        /// The server encounters an error while trying to register the user's push token. Please retry the request.
        /// </summary>
        /// @since 4.0.0
        InternalErrorPushTokenNotRegistered = 500601,

        /// <summary>
        /// The server encounters an error while trying to unregister the user's push token. Please retry the request.
        /// </summary>
        /// @since 4.0.0
        InternalErrorPushTokenNotUnregistered = 500602,

        /// <summary>
        /// The server encounters an unexpected exception while trying to process the request. Please retry the request.
        /// </summary>
        /// @since 4.0.0
        InternalError = 500901,

        /// <summary>
        /// The request can't be completed because you have exceeded your rate limits.
        /// </summary>
        /// @since 4.0.0
        RateLimitExceeded = 500910,

        /// <summary>
        /// The request failed due to a temporary failure of the server. Please retry the request.
        /// </summary>
        /// @since 4.0.0
        ServiceUnavailable = 503,

        /// <summary>
        /// The request failed because the user isn't logged in to the server.
        /// </summary>
        /// @since 4.0.0
        SocketUserLoginRequired = 900010,

        /// <summary>
        /// The request failed because the user isn't a member of the channel.
        /// </summary>
        /// @since 4.0.0
        SocketUserNotMember = 900020,

        /// <summary>
        /// The request failed because the user is deactivated in the service.
        /// </summary>
        /// @since 4.0.0
        SocketUserDeactivated = 900021,

        /// <summary>
        /// The request failed because the user has no permission to edit the other user's message.
        /// </summary>
        /// @since 4.0.0
        SocketUserNotOwnerOfMessage = 900022,

        /// <summary>
        /// The request failed because the user is trying to send the messages in the channel of which they are not the member.
        /// </summary>
        /// @since 4.0.0
        SocketPendingUserSendMessageNotAllowed = 900023,

        /// <summary>
        /// The specified mention type in the request is invalid.
        /// </summary>
        /// @since 4.0.0
        SocketInvalidMentionForMessage = 900025,

        /// <summary>
        /// The specified push option in the request is invalid.
        /// </summary>
        /// @since 4.0.0
        SocketInvalidPushOptionForMessage = 900026,

        /// <summary>
        /// The request failed because it specifies more meta data keys for the message than allowed.
        /// </summary>
        /// @since 4.0.0
        SocketTooManyMetaKeyForMessage = 900027,

        /// <summary>
        /// The request failed because it specifies more meta data values for the message than allowed.
        /// </summary>
        /// @since 4.0.0
        SocketTooManyMetaValueForMessage = 900028,

        /// <summary>
        /// The request failed because it specifies an invalid value in the meta data for the message.
        /// </summary>
        /// @since 4.0.0
        SocketInvalidMetaArrayForMessage = 900029,

        /// <summary>
        /// The request failed because the guest isn't allowed to perform this operation.
        /// </summary>
        /// @since 4.0.0
        SocketGuestNotAllowed = 900030,

        /// <summary>
        /// The request failed because the user is muted in the application and isn't allowed to send the message.
        /// </summary>
        /// @since 4.0.0
        SocketMutedUserInApplicationSendMessageNotAllowed = 900040,

        /// <summary>
        /// The request failed because the user is muted in the channel and isn't allowed to send the message.
        /// </summary>
        /// @since 4.0.0
        SocketMutedUserInChannelSendMessageNotAllowed = 900041,

        /// <summary>
        /// The request failed because the channel is frozen and no one can send the message to the channel.
        /// </summary>
        /// @since 4.0.0
        SocketChannelFrozen = 900050,

        /// <summary>
        /// The request failed because it specifies the message containing a profanity word.
        /// </summary>
        /// @since 4.0.0
        SocketProfanityMessageBlocked = 900060,

        /// <summary>
        /// The request failed because message it specifies the message containing a URL that isn't allowed.
        /// </summary>
        /// @since 4.0.0
        SocketBannedUrlsBlocked = 900061,

        /// <summary>
        /// The request failed because it comes from the domain that isn't allowed.
        /// </summary>
        /// @since 4.0.0
        SocketRestrictedDomainBlocked = 900065,

        /// <summary>
        /// The request failed because it contains the file violating at least one of the content management policies.
        /// </summary>
        /// @since 4.0.0
        SocketModeratedFileBlocked = 900066,

        /// <summary>
        /// The request failed because the user is trying to enter a deleted channel.
        /// </summary>
        /// @since 4.0.0
        SocketEnterDeletedChannel = 900070,

        /// <summary>
        /// The request failed because the blocking user is trying to send the message to the blocked user in a 1-to-1 distinct channel.
        /// </summary>
        /// @since 4.0.0
        SocketBlockedUserReceiveMessageNotAllowed = 900080,

        /// <summary>
        /// The request failed because the user is trying to send the message to the deactivated user in a 1-to-1 distinct channel.
        /// </summary>
        /// @since 4.0.0
        SocketDeactivatedUserReceiveMessageNotAllowed = 900081,

        /// <summary>
        /// The request failed because it specifies a wrong channel type.
        /// </summary>
        /// @since 4.0.0
        SocketWrongChannelType = 900090,

        /// <summary>
        /// The request failed because the user is banned from the channel and isn't allowed to send the message.
        /// </summary>
        /// @since 4.0.0
        SocketBannedUserSendMessageNotAllowed = 900100,

        /// <summary>
        /// The number of the sent messages exceeds the allowed amount.
        /// </summary>
        /// @since 4.0.0
        SocketTooManyMessages = 900200,

        /// <summary>
        /// The request failed because the message to edit can't be retrieved.
        /// </summary>
        /// @since 4.0.0
        SocketMessageNotFound = 900300,

        /// <summary>
        /// The number of the channel's participants exceeds the allowed amount.
        /// </summary>
        /// @since 4.0.0
        SocketTooManyParticipants = 900400,

        /// <summary>
        /// The request failed because there is no channel to perform this operation.
        /// </summary>
        /// @since 4.0.0
        SocketChannelNotFound = 900500,
    }
}