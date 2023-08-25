// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbOpenChannel
    {
        private void UpdateChannelInternal(SbOpenChannelUpdateParams inChannelUpdateParams, SbOpenChannelCallbackHandler inCompletionHandler)
        {
            if (inChannelUpdateParams == null)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("OpenChannelUpdateParams");
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

                if (inResponse is UpdateOpenChannelApiCommand.Response response && response.OpenChannelDto != null)
                {
                    ResetFromChannelDto(response.OpenChannelDto);
                    inCompletionHandler?.Invoke(this, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            UpdateOpenChannelApiCommand.Request createOpenChannelApiCommand = new UpdateOpenChannelApiCommand.Request(Url, inChannelUpdateParams, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(createOpenChannelApiCommand);
        }

        private void DeleteInternal(SbErrorHandler inCompletionHandler)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError == null)
                {
                    chatMainContextRef.OpenChannelManager.RemoveEnteredChannelIfContains(Url);
                    chatMainContextRef.OpenChannelManager.RemoveCachedChannelIfContains(Url);
                }

                inCompletionHandler?.Invoke(inError);
            }

            DeleteChannelApiCommand.Request deleteChannelApiCommand = new DeleteChannelApiCommand.Request(Url, ChannelType, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(deleteChannelApiCommand);
        }
    }
}