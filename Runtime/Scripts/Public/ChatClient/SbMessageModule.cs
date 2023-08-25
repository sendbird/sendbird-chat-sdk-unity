// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// @since 4.0.0
    public partial class SbMessageModule
    {
        /// <summary>
        /// Retrieves a message with a specified message ID.
        /// </summary>
        /// <param name="inParams">Contains a set of parameters you can set regarding the messages in the results.</param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetMessage(SbMessageRetrievalParams inParams, SbBaseMessageHandler inCompletionHandler)
        {
            SbBaseChannel.GetMessage(_chatMainRef.ChatMainContext, inParams, inCompletionHandler);
        }
    }
}