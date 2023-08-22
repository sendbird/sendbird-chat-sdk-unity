// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbGroupChannel
    {
        private void ResetMyHistoryInternal(SbErrorHandler inCompletionHandler = null)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is ResetGroupChannelHistoryApiCommand.Response resetResponse)
                {
                    SetMessageOffsetTimestamp(resetResponse.messageOffsetTimestamp);
                }

                inCompletionHandler?.Invoke(inError);
            }

            string userId = chatMainContextRef.CurrentUserRef?.UserId;
            ResetGroupChannelHistoryApiCommand.Request apiCommand = new ResetGroupChannelHistoryApiCommand.Request(Url, userId, inResetAll: false, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}