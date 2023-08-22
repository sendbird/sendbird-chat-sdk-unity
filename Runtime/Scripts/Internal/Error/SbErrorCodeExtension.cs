// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal static class SbErrorCodeExtension
    {
        internal static readonly SbError COLLECTION_DISPOSED = new SbError(SbErrorCode.CollectionDisposed);
        internal static readonly SbError CONNECTION_REQUIRED_ERROR = new SbError(SbErrorCode.ConnectionRequired);
        internal static readonly SbError INVALID_INITIALIZATION_ERROR = new SbError(SbErrorCode.InvalidInitialization);
        internal static readonly SbError INVALID_PARAMETER_ERROR = new SbError(SbErrorCode.InvalidParameter);
        internal static readonly SbError LOCAL_DATABASE_ERROR = new SbError(SbErrorCode.LocalDatabaseError);
        internal static readonly SbError MALFORMED_DATA_ERROR = new SbError(SbErrorCode.MalformedData);
        internal static readonly SbError MARK_AS_READ_RATE_LIMIT_EXCEEDED_ERROR = new SbError(SbErrorCode.MarkAsReadRateLimitExceeded);
        internal static readonly SbError QUERY_IN_PROGRESS = new SbError(SbErrorCode.QueryInProgress);
        internal static readonly SbError WEB_SOCKET_CONNECTION_CLOSED_ERROR = new SbError(SbErrorCode.WebSocketConnectionClosed);

        internal static class DescriptionDefines
        {
            internal const string NOT_RESEND_ABLE = "The message can only resend if the problem is due to network related error.";
            internal const string NEED_SUCCEEDED_MESSAGE = "Need a succeeded message.";
            internal const string MESSAGE_DOESNT_BELONG_TO_CHANNEL = "The message doesn't belong to this channel.";
        }

        internal static string ToDescriptionString(this SbErrorCode inErrorCode, string inOptionValue = "")
        {
            switch (inErrorCode)
            {
                case SbErrorCode.InvalidParameter:                                  return $"{inOptionValue} is an invalid or empty value.";
                case SbErrorCode.UnknownError:                                      return "UnknownError";
                case SbErrorCode.InvalidInitialization:                             return "The initialization of the SendbirdChat instance failed because the value assigned to APP_ID upon initializing wasn't valid.";
                case SbErrorCode.ConnectionRequired:                                return "The request from a client app failed because the device wasn't connected to the server.";
                case SbErrorCode.ConnectionCanceled:                                return "The connection is canceled or the disconnecting method is called while the SendbirdChat instance is trying to connect to the server.";
                case SbErrorCode.NetworkError:                                      return "The connection failed due to the unstable network or an unexpected error in the Chat SDK network library.";
                case SbErrorCode.NetworkRoutingError:                               return "The request routing to the server failed.";
                case SbErrorCode.MalformedData:                                     return "The data format of the server response is invalid.";
                case SbErrorCode.MalformedErrorData:                                return "The data format of the error message is invalid due to the problem with the request.";
                case SbErrorCode.WrongChannelType:                                  return "The specified channel type in the request is invalid.";
                case SbErrorCode.MarkAsReadRateLimitExceeded:                       return "The interval between the successive requests is less than a second.";
                case SbErrorCode.QueryInProgress:                                   return "A retrieval request is arriving while the server is still processing the previous retrieval request for channels, messages, or users, and in preparation to send the response.";
                case SbErrorCode.AckTimeout:                                        return "The server failed to send a response for the request in  a certain amount of seconds.";
                case SbErrorCode.LoginTimeout:                                      return "The server failed to send a response for the SendbirdChat instance's login request in 10 seconds.";
                case SbErrorCode.WebSocketConnectionClosed:                         return "The request was submitted while disconnected from the server.";
                case SbErrorCode.WebSocketConnectionFailed:                         return "The websocket connection to the server failed to establish.";
                case SbErrorCode.RequestFailed:                                     return "The server failed to process the request due to an internal reason.";
                case SbErrorCode.FileUploadCancelFailed:                            return "The request to cancel file upload failed due to an unexpected error.";
                case SbErrorCode.FileUploadCanceled:                                return "The file upload request is canceled.";
                case SbErrorCode.FileUploadTimeout:                                 return "A time-out error signaling that the Sendbird server has failed to complete the file upload request in the time period allowed.";
                case SbErrorCode.TimerWasExpired:                                   return "An action wasn’t completed within a certain time frame.";
                case SbErrorCode.TimerWasAlreadyDone:                               return "Actions were completed multiple times when the action should have been completed just once.";
                case SbErrorCode.PendingError:                                      return "Message status doesn't match.";
                case SbErrorCode.PassedInvalidAccessToken:                          return "The client app failed to refresh a session with the given token because the token is invalid.";
                case SbErrorCode.SessionKeyRefreshSucceeded:                        return "The session has expired and was refreshed with the given token.";
                case SbErrorCode.SessionKeyRefreshFailed:                           return "The session has expired but the client app failed to refresh the session with the given token.";
                case SbErrorCode.CollectionDisposed:                                return "The client app attempted to use a message collection or a channel collection after it has already been disposed of.";
                case SbErrorCode.LocalDatabaseError:                                return "The client app attempted to use local database with local caching.";
                case SbErrorCode.UnexpectedParameterTypeString:                     return "The request specifies one or more parameters in an unexpected data type. The data type of the parameters should be string.";
                case SbErrorCode.UnexpectedParameterTypeNumber:                     return "The request specifies one or more parameters in an unexpected data type. The data type of the parameters should be number.";
                case SbErrorCode.UnexpectedParameterTypeList:                       return "The request specifies one or more parameters in an unexpected data type. The data type of the parameters should be list.";
                case SbErrorCode.UnexpectedParameterTypeJson:                       return "The request specifies one or more parameters in an unexpected data type. The data type of the parameters should be JSON.";
                case SbErrorCode.UnexpectedParameterTypeBoolean:                    return "The request specifies one or more parameters in an unexpected data type. The data type of the parameters should be boolean.";
                case SbErrorCode.MissingRequiredParameters:                         return "The request is missing one or more required parameters.";
                case SbErrorCode.NegativeNumberNotAllowed:                          return "The parameter specifies an invalid negative number. Its value should be a positive number.";
                case SbErrorCode.UnauthorizedRequest:                               return "The request isn't authorized and can't access the requested resource.";
                case SbErrorCode.ExpiredPageToken:                                  return "The value of the token parameter for pagination has expired.";
                case SbErrorCode.ParameterValueLengthExceeded:                      return "The length of the parameter value is too long.";
                case SbErrorCode.InvalidValue:                                      return "The request specifies an invalid value.";
                case SbErrorCode.IncompatibleValues:                                return "The two parameters of the request, which should have unique values, specify the same value.";
                case SbErrorCode.ParameterValueOutOfRange:                          return "The request specifies one or more parameters which are outside the allowed value range.";
                case SbErrorCode.InvalidURLOfResource:                              return "The resource identified with the URL in the request can't be found.";
                case SbErrorCode.NotAllowedCharacter:                               return "The request specifies an illegal value containing special character, empty string, or white space.";
                case SbErrorCode.ResourceNotFound:                                  return "The resource identified with the request's resourceId parameter can't be found.";
                case SbErrorCode.ResourceAlreadyExists:                             return "The resource identified with the request's resourceId parameter already exists.";
                case SbErrorCode.TooManyItemsInParameter:                           return "The parameter specifies more items than allowed.";
                case SbErrorCode.DeactivatedUserNotAccessible:                      return "The request can't retrieve the deactivated user data.";
                case SbErrorCode.UserNotFound:                                      return "The user identified with the request's userId parameter can't be found because either the user doesn't exist or has been deleted.";
                case SbErrorCode.InvalidAccessToken:                                return "The access token provided for the request specifies an invalid value.";
                case SbErrorCode.InvalidSessionKeyValue:                            return "The session key provided for the request specifies an invalid value.";
                case SbErrorCode.ApplicationNotFound:                               return "The application identified with the request can't be found.";
                case SbErrorCode.UserIdLengthExceeded:                              return "The length of the userId parameter value is too long.";
                case SbErrorCode.PaidQuotaExceeded:                                 return "The request can't be completed because you have exceeded your paid quota.";
                case SbErrorCode.DomainNotAllowed:                                  return "The request can't be completed because it came from the restricted domain.";
                case SbErrorCode.SessionKeyExpired:                                 return "The session key of the user has been expired. A new one must be issued.";
                case SbErrorCode.SessionTokenRevoked:                               return "The session key of the user has been revoked. A new one must be issued.";
                case SbErrorCode.InvalidApiToken:                                   return "The API token provided for the request specifies an invalid value.";
                case SbErrorCode.MissingSomeParameters:                             return "The request is missing one or more necessary parameters.";
                case SbErrorCode.InvalidJsonRequestBody:                            return "The request body is an invalid JSON.";
                case SbErrorCode.InvalidRequestURL:                                 return "The request specifies an invalid HTTP request URL that can't be accessed.";
                case SbErrorCode.TooManyUserWebsocketConnections:                   return "The number of the user's websocket connections exceeds the allowed amount.";
                case SbErrorCode.TooManyApplicationWebsocketConnections:            return "The number of the application's websocket connections exceeds the allowed amount.";
                case SbErrorCode.BlockedUserSendMessageNotAllowed:                  return "The request can't be completed due to one of the following reasons: The sender is blocked by the recipient or has been deactivated, the message is longer than the maximum message length, or the message contains texts or URLs blocked by application settings or filters.";
                case SbErrorCode.BlockedUserInvitedNotAllowed:                      return "The request can't be completed because the blocking user is trying to invite the blocked user to a channel.";
                case SbErrorCode.BlockedUserInviteNotAllowed:                       return "The request can't be completed because the blocked user is trying to invite the blocking user to a channel.";
                case SbErrorCode.BannedUserEnterChannelNotAllowed:                  return "The request can't be completed because the user is trying to enter a banned channel.";
                case SbErrorCode.BannedUserEnterCustomChannelNotAllowed:            return "The request can't be completed because the user is trying to enter a banned custom type channel.";
                case SbErrorCode.Unacceptable:                                      return "The request failed because the combination of parameter values is invalid. Even if each parameter value is valid, a combination of parameter values becomes invalid when it doesn't follow specific conditions.";
                case SbErrorCode.InvalidEndpoint:                                   return "The request failed because it is sent to an invalid endpoint that no longer exists.";
                case SbErrorCode.ApplicationNotAvailable:                           return "The application identified with the request isn't available.";
                case SbErrorCode.InternalErrorPushTokenNotRegistered:               return "The server encounters an error while trying to register the user's push token. Please retry the request.";
                case SbErrorCode.InternalErrorPushTokenNotUnregistered:             return "The server encounters an error while trying to unregister the user's push token. Please retry the request.";
                case SbErrorCode.InternalError:                                     return "The server encounters an unexpected exception while trying to process the request. Please retry the request.";
                case SbErrorCode.RateLimitExceeded:                                 return "The request can't be completed because you have exceeded your rate limits.";
                case SbErrorCode.ServiceUnavailable:                                return "The request failed due to a temporary failure of the server. Please retry the request.";
                case SbErrorCode.SocketUserLoginRequired:                           return "The request failed because the user isn't logged in to the server.";
                case SbErrorCode.SocketUserNotMember:                               return "The request failed because the user isn't a member of the channel.";
                case SbErrorCode.SocketUserDeactivated:                             return "The request failed because the user is deactivated in the service.";
                case SbErrorCode.SocketUserNotOwnerOfMessage:                       return "The request failed because the user has no permission to edit the other user's message.";
                case SbErrorCode.SocketPendingUserSendMessageNotAllowed:            return "The request failed because the user is trying to send the messages in the channel of which they are not the member.";
                case SbErrorCode.SocketInvalidMentionForMessage:                    return "The specified mention type in the request is invalid.";
                case SbErrorCode.SocketInvalidPushOptionForMessage:                 return "The specified push option in the request is invalid.";
                case SbErrorCode.SocketTooManyMetaKeyForMessage:                    return "The request failed because it specifies more meta data keys for the message than allowed.";
                case SbErrorCode.SocketTooManyMetaValueForMessage:                  return "The request failed because it specifies more meta data values for the message than allowed.";
                case SbErrorCode.SocketInvalidMetaArrayForMessage:                  return "The request failed because it specifies an invalid value in the meta data for the message.";
                case SbErrorCode.SocketGuestNotAllowed:                             return "The request failed because the guest isn't allowed to perform this operation.";
                case SbErrorCode.SocketMutedUserInApplicationSendMessageNotAllowed: return "The request failed because the user is muted in the application and isn't allowed to send the message.";
                case SbErrorCode.SocketMutedUserInChannelSendMessageNotAllowed:     return "The request failed because the user is muted in the channel and isn't allowed to send the message.";
                case SbErrorCode.SocketChannelFrozen:                               return "The request failed because the channel is frozen and no one can send the message to the channel.";
                case SbErrorCode.SocketProfanityMessageBlocked:                     return "The request failed because it specifies the message containing a profanity word.";
                case SbErrorCode.SocketBannedUrlsBlocked:                           return "The request failed because message it specifies the message containing a URL that isn't allowed.";
                case SbErrorCode.SocketRestrictedDomainBlocked:                     return "The request failed because it comes from the domain that isn't allowed.";
                case SbErrorCode.SocketModeratedFileBlocked:                        return "The request failed because it contains the file violating at least one of the content management policies.";
                case SbErrorCode.SocketEnterDeletedChannel:                         return "The request failed because the user is trying to enter a deleted channel.";
                case SbErrorCode.SocketBlockedUserReceiveMessageNotAllowed:         return "The request failed because the blocking user is trying to send the message to the blocked user in a 1-to-1 distinct channel.";
                case SbErrorCode.SocketDeactivatedUserReceiveMessageNotAllowed:     return "The request failed because the user is trying to send the message to the deactivated user in a 1-to-1 distinct channel.";
                case SbErrorCode.SocketWrongChannelType:                            return "The request failed because it specifies a wrong channel type.";
                case SbErrorCode.SocketBannedUserSendMessageNotAllowed:             return "The request failed because the user is banned from the channel and isn't allowed to send the message.";
                case SbErrorCode.SocketTooManyMessages:                             return "The number of the sent messages exceeds the allowed amount.";
                case SbErrorCode.SocketMessageNotFound:                             return "The request failed because the message to edit can't be retrieved.";
                case SbErrorCode.SocketTooManyParticipants:                         return "The number of the channel's participants exceeds the allowed amount.";
                case SbErrorCode.SocketChannelNotFound:                             return "The request failed because there is no channel to perform this operation.";
                default:                                                            return string.Empty;
            }
        }

        internal static bool IsSessionError(this SbErrorCode inErrorCode)
        {
            if (inErrorCode == SbErrorCode.SessionTokenRevoked || inErrorCode.IsSessionErrorThatNeedRefresh())
                return true;

            return false;
        }

        internal static bool IsSessionErrorThatNeedRefresh(this SbErrorCode inErrorCode)
        {
            if (inErrorCode == SbErrorCode.InvalidAccessToken || inErrorCode == SbErrorCode.SessionKeyExpired)
                return true;

            return false;
        }

        internal static SbError CreateInvalidParameterError(string inParamName)
        {
            return new SbError(SbErrorCode.InvalidParameter, SbErrorCode.InvalidParameter.ToDescriptionString(inParamName));
        }
    }
}