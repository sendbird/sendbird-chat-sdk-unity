//
//  Copyright (c) 2022 Sendbird, Inc.
//

namespace Sendbird.Chat
{
    internal abstract class MessageWsReceiveCommandAbstract : WsMessageReceiveCommandAbstract
    {
        protected internal BaseMessageDto BaseMessageDto { get; protected set; }

        protected MessageWsReceiveCommandAbstract(WsCommandType inWsCommandType) : base(inWsCommandType) { }

        internal bool IsCreatedFromCurrentDevice()
        {
            return BaseMessageDto != null && string.IsNullOrEmpty(BaseMessageDto.MessageCreatedRequestId) == false && BaseMessageDto.MessageCreatedRequestId == ReqId;
        }
    }
}
