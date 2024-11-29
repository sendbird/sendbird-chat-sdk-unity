// 
//  Copyright (c) 2024 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbGroupChannel
    {
        private void GetMyPushTriggerOptionInternal(SbGroupChannelGetMyPushTriggerOptionHandler inCompletionHandler)
        {
            if (inCompletionHandler == null)
                return;

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is GetMyPushTriggerOptionApiCommand.Response response)
                {
                    inCompletionHandler.Invoke(response.GroupChannelPushTriggerOption, inError);
                    return;
                }

                inCompletionHandler.Invoke(inMyPushTriggerOption: null, inError);
            }

            GetMyPushTriggerOptionApiCommand.Request apiCommand = new GetMyPushTriggerOptionApiCommand.Request(
                chatMainContextRef.CurrentUserId, Url, OnCompletionHandler);

            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void SetMyPushTriggerOptionInternal(SbGroupChannelPushTriggerOption inGroupChannelPushTriggerOption, SbErrorHandler inCompletionHandler)
        {
            if (inCompletionHandler == null)
                return;

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is SetMyPushTriggerOptionApiCommand.Response response)
                {
                    _myPushTriggerOption = response.GroupChannelPushTriggerOption;
                }

                inCompletionHandler.Invoke(inError);
            }

            SetMyPushTriggerOptionApiCommand.Request apiCommand = new SetMyPushTriggerOptionApiCommand.Request(
                chatMainContextRef.CurrentUserId, Url, inGroupChannelPushTriggerOption, OnCompletionHandler);

            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}