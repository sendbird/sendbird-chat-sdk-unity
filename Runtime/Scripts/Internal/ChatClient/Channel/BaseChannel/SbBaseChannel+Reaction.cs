// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbBaseChannel
    {
        private void AddReactionInternal(SbBaseMessage inMessage, string inKey, SbReactionEventHandler inCompletionHandler)
        {
            if (inMessage == null || inMessage.MessageId <= SbBaseMessage.INVALID_MESSAGE_ID_MIN)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("Message");
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

                if (inResponse is AddReactionApiCommand.Response response && response.ReactionEventDto != null)
                {
                    SbReactionEvent reactionEvent = new SbReactionEvent(response.ReactionEventDto);
                    inCompletionHandler?.Invoke(reactionEvent, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            AddReactionApiCommand.Request apiCommand = new AddReactionApiCommand.Request(Url, ChannelType, inMessage.MessageId,
                                                                                         chatMainContextRef.CurrentUserId, inKey, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void DeleteReactionInternal(SbBaseMessage inMessage, string inKey, SbReactionEventHandler inCompletionHandler)
        {
            if (inMessage == null || inMessage.MessageId <= SbBaseMessage.INVALID_MESSAGE_ID_MIN)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("Message");
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

                if (inResponse is DeleteReactionApiCommand.Response response && response.ReactionEventDto != null)
                {
                    SbReactionEvent reactionEvent = new SbReactionEvent(response.ReactionEventDto);
                    inCompletionHandler?.Invoke(reactionEvent, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            DeleteReactionApiCommand.Request apiCommand = new DeleteReactionApiCommand.Request(Url, ChannelType, inMessage.MessageId,
                                                                                               chatMainContextRef.CurrentUserId, inKey, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}