// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbGroupChannel
    {
        private void PinMessageInternal(long inMessageId, SbErrorHandler inCompletionHandler = null)
        {
            if (inMessageId <= SbBaseMessage.INVALID_MESSAGE_ID_MIN)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("MessageId");
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(error));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            PinMessageApiCommand.Request apiCommand = new PinMessageApiCommand.Request(Url, ChannelType, inMessageId, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void UnpinMessageInternal(long inMessageId, SbErrorHandler inCompletionHandler = null)
        {
            if (inMessageId <= SbBaseMessage.INVALID_MESSAGE_ID_MIN)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("MessageId");
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(error));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            UnpinMessageApiCommand.Request apiCommand = new UnpinMessageApiCommand.Request(Url, ChannelType, inMessageId, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}