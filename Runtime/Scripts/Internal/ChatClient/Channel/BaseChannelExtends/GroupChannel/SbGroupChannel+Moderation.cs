// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbGroupChannel
    {
        private void FreezeInternal(bool inFreeze, SbErrorHandler inCompletionHandler)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            FreezeGroupChannelApiCommand.Request apiCommand = new FreezeGroupChannelApiCommand.Request(Url, inFreeze, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}