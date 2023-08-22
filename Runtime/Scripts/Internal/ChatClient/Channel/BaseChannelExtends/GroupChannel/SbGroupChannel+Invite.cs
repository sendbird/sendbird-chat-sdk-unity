// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbGroupChannel
    {
        private void InviteInternal(List<string> inUserIds, SbErrorHandler inCompletionHandler = null)
        {
            if (inUserIds == null || inUserIds.Count <= 0)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("Invitation UserIds");
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(error));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is GroupChannelInviteApiCommand.Response inviteResponse && inviteResponse.GroupChannelDto != null)
                {
                    chatMainContextRef.GroupChannelManager.CreateOrUpdateChannel(inviteResponse.GroupChannelDto);
                }

                inCompletionHandler?.Invoke(inError);
            }

            string inviterId = chatMainContextRef.CurrentUserId;
            GroupChannelInviteApiCommand.Request apiCommand = new GroupChannelInviteApiCommand.Request(Url, inUserIds, inviterId, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void AcceptInvitationInternal(string inAccessCode, SbErrorHandler inCompletionHandler = null)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            string userId = chatMainContextRef.CurrentUserRef?.UserId;
            GroupChannelAcceptInvitationApiCommand.Request apiCommand = new GroupChannelAcceptInvitationApiCommand.Request(Url, userId, inAccessCode, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void DeclineInvitationInternal(SbErrorHandler inCompletionHandler = null)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            GroupChannelDeclineInvitationApiCommand.Request apiCommand = new GroupChannelDeclineInvitationApiCommand.Request(Url, chatMainContextRef.CurrentUserId, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}