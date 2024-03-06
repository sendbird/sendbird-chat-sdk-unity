// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbBaseChannel
    {
        private void BanUserInternal(string inUserId, int inSeconds, string inDescription, SbErrorHandler inCompletionHandler)
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

            BanUserApiCommand.Request apiCommand = new BanUserApiCommand.Request(Url, ChannelType, inUserId, inSeconds, inDescription, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void UnbanUserInternal(string inUserId, SbErrorHandler inCompletionHandler)
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

            UnbanUserApiCommand.Request apiCommand = new UnbanUserApiCommand.Request(Url, ChannelType, inUserId, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void MuteUserInternal(string inUserId, int inSeconds, string inDescription, SbErrorHandler inCompletionHandler)
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

            MuteUserApiCommand.Request apiCommand = new MuteUserApiCommand.Request(Url, ChannelType, inUserId, inSeconds, inDescription, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void UnmuteUserInternal(string inUserId, SbErrorHandler inCompletionHandler)
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

            UnmuteUserApiCommand.Request apiCommand = new UnmuteUserApiCommand.Request(Url, ChannelType, inUserId, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void GetMyMutedInfoInternal(SbMuteInfoHandler inCompletionHandler)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(inIsMuted: false, inDescription: null, inStartAt: 0, inEndAt: 0, inRemainingDuration: 0, inError);
                    return;
                }

                if (!(inResponse is GetMyMutedInfoApiCommand.Response muteInfoResponse))
                {
                    inCompletionHandler?.Invoke(inIsMuted: false, inDescription: null, inStartAt: 0, inEndAt: 0, inRemainingDuration: 0, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                    return;
                }

                inCompletionHandler?.Invoke(muteInfoResponse.isMuted, muteInfoResponse.description, muteInfoResponse.startAt,
                                            muteInfoResponse.endAt, muteInfoResponse.remainingDuration, null);
            }

            GetMyMutedInfoApiCommand.Request apiCommand = new GetMyMutedInfoApiCommand.Request(Url, ChannelType, chatMainContextRef.CurrentUserId, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private SbBannedUserListQuery CreateBannedUserListQueryInternal(SbBannedUserListQueryParams inParams = null)
        {
            if (inParams == null)
                inParams = new SbBannedUserListQueryParams();

            return new SbBannedUserListQuery(ChannelType, _url, inParams, chatMainContextRef);
        }

        private SbMutedUserListQuery CreateMutedUserListQueryInternal(SbMutedUserListQueryParams inParams = null)
        {
            if (inParams == null)
                inParams = new SbMutedUserListQueryParams();

            return new SbMutedUserListQuery(ChannelType, _url, inParams, chatMainContextRef);
        }
    }
}