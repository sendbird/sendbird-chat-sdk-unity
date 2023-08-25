// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbGroupChannel
    {
        private void UpdateChannelInternal(SbGroupChannelUpdateParams inChannelUpdateParams, SbGroupChannelCallbackHandler inCompletionHandler)
        {
            if (inChannelUpdateParams == null)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("GroupChannelUpdateParams");
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(null, error));
                }

                return;
            }

            if (chatMainContextRef == null || chatMainContextRef.CurrentUserRef == null)
            {
                if (inCompletionHandler != null)
                {
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(null, SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR));
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

                if (inResponse is UpdateGroupChannelApiCommand.Response response && response.GroupChannelDto != null)
                {
                    ResetFromChannelDto(response.GroupChannelDto);
                    inCompletionHandler?.Invoke(this, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            UpdateGroupChannelApiCommand.Request createOpenChannelApiCommand = new UpdateGroupChannelApiCommand.Request(Url, inChannelUpdateParams, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(createOpenChannelApiCommand);
        }

        private void DeleteInternal(SbErrorHandler inCompletionHandler)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError == null)
                {
                    chatMainContextRef.GroupChannelManager.RemoveCachedChannelIfContains(Url);
                }

                inCompletionHandler?.Invoke(inError);
            }

            DeleteChannelApiCommand.Request apiCommand = new DeleteChannelApiCommand.Request(Url, ChannelType, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}