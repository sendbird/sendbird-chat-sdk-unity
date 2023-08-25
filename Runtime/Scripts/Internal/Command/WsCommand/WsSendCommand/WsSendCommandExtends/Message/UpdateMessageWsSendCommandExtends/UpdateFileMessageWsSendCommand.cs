// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UpdateFileMessageWsSendCommand : UpdateMessageWsSendCommandAbstract
    {
        internal UpdateFileMessageWsSendCommand(string inChannelUrl, long inMessageId, SbFileMessageUpdateParams inFileMessageUpdateParams, AckHandler inAckHandler)
            : base(WsCommandType.UpdateFileMessage, inChannelUrl, inMessageId, inFileMessageUpdateParams, inAckHandler) { }
        
        internal UpdateFileMessageWsSendCommand(string inChannelUrl, long inMessageId, MessageMetaArrayUpdateDto inMetaArrayUpdateDto, AckHandler inAckHandler)
            : base(WsCommandType.UpdateFileMessage, inChannelUrl, inMessageId, inMetaArrayUpdateDto, inAckHandler) { }
    }
}