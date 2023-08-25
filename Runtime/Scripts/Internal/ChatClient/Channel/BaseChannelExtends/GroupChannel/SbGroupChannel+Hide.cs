// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbGroupChannel
    {
        private void HideInternal(bool inHidePreviousMessages, bool inAllowAutoUnhide, SbErrorHandler inCompletionHandler)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is HideGroupChannelApiCommand.Response response)
                {
                    if (response.messageOffsetTimestamp != null)
                    {
                        SetMessageOffsetTimestamp(response.messageOffsetTimestamp.Value);
                    }

                    if (inHidePreviousMessages)
                        ClearAllUnreadCount();

                    _isHidden = true;
                    _hiddenState = inAllowAutoUnhide ? SbGroupChannelHiddenState.HiddenAllowAutoUnhide : SbGroupChannelHiddenState.HiddenPreventAutoUnhide;
                }

                inCompletionHandler?.Invoke(inError);
            }

            HideGroupChannelApiCommand.Request apiCommand = new HideGroupChannelApiCommand.Request(
                Url, chatMainContextRef.CurrentUserId, inHidePreviousMessages, inAllowAutoUnhide, OnCompletionHandler);

            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void UnhideInternal(SbErrorHandler inCompletionHandler)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError == null)
                {
                    _isHidden = false;
                    _hiddenState = SbGroupChannelHiddenState.Unhidden;
                }

                inCompletionHandler?.Invoke(inError);
            }

            UnhideGroupChannelApiCommand.Request apiCommand = new UnhideGroupChannelApiCommand.Request(Url, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}