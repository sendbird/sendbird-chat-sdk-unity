// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal partial class SendbirdChatMain
    {
        internal void AddUserEventHandler(string inIdentifier, SbUserEventHandler inUserEventHandler)
        {
            if (string.IsNullOrEmpty(inIdentifier) || inUserEventHandler == null)
            {
                Logger.Warning(Logger.CategoryType.User, "AddUseEventHandler() : Invalid parameter.");
                return;
            }
            
            if (_userEventHandlersById.ContainsKey(inIdentifier) == false)
            {
                _userEventHandlersById.Add(inIdentifier, inUserEventHandler);
            }
            else
            {
                _userEventHandlersById[inIdentifier] = inUserEventHandler;
            }
        }

        internal void RemoveUserEventHandler(string inIdentifier)
        {
            _userEventHandlersById.RemoveIfContains(inIdentifier);
        }

        internal void RemoveAllUserEventHandlers()
        {
            _userEventHandlersById.Clear();
        }

        internal void UpdateCurrentUserInfo(SbUserUpdateParams inParams, SbErrorHandler inCompletionHandler)
        {
            if (inParams == null || CurrentUser == null)
            {
                if (inCompletionHandler == null)
                    return;

                SbError error = null;
                if (inParams == null)
                {
                    error = SbErrorCodeExtension.CreateInvalidParameterError("UserUpdateParams");
                }
                else if (CurrentUser == null)
                {
                    error = SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR;
                }

                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(error));

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(inError);
                    return;
                }

                if (inResponse is UpdateUserInfoApiCommand.Response response && response.UserDto != null)
                {
                    ChatMainContext.CurrentUserRef.UpdateFromDto(response.UserDto);
                    inCompletionHandler?.Invoke(null);
                }
                else
                {
                    inCompletionHandler?.Invoke(SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            UpdateUserInfoApiCommand.Request apiCommand = new UpdateUserInfoApiCommand.Request(ChatMainContext.CurrentUserId, inParams, OnCompletionHandler);
            ChatMainContext.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void UpdateCurrentUserPreferredLanguages(List<string> inPreferredLanguages, SbErrorHandler inCompletionHandler)
        {
            if (inPreferredLanguages == null)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("PreferredLanguages");
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(error));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(inError);
                    return;
                }

                if (inResponse is UpdateUserInfoApiCommand.Response response && response.UserDto != null)
                {
                    ChatMainContext.CurrentUserRef.UpdateFromDto(response.UserDto);
                    inCompletionHandler?.Invoke(null);
                }
                else
                {
                    inCompletionHandler?.Invoke(SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            UpdateUserInfoApiCommand.Request apiCommand = new UpdateUserInfoApiCommand.Request(ChatMainContext.CurrentUserId, inPreferredLanguages, OnCompletionHandler);
            ChatMainContext.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void BlockUser(string inUserId, SbUserHandler inCompletionHandler)
        {
            if (string.IsNullOrEmpty(inUserId))
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("UserId");
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(null, error));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                if (inResponse is BlockUserApiCommand.Response response && response.UserDto != null)
                {
                    SbUser user = new SbUser(response.UserDto, ChatMainContext);
                    inCompletionHandler?.Invoke(user, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            BlockUserApiCommand.Request apiCommand = new BlockUserApiCommand.Request(ChatMainContext.CurrentUserId, inUserId, OnCompletionHandler);
            ChatMainContext.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void UnblockUser(string inUserId, SbErrorHandler inCompletionHandler)
        {
            if (string.IsNullOrEmpty(inUserId))
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("UserId");
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(error));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            UnblockUserApiCommand.Request apiCommand = new UnblockUserApiCommand.Request(ChatMainContext.CurrentUserId, inUserId, OnCompletionHandler);
            ChatMainContext.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}