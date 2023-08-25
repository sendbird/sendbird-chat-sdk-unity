// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbReactionEvent
    {
        private readonly string _key;
        private readonly long _messageId;
        private readonly long _updatedAt;
        private readonly string _userId;
        private readonly SbReactionEventAction _operation;

        internal SbReactionEvent(ReactionEventDto inReactionEventDto)
        {
            if (inReactionEventDto != null)
            {
                _key = inReactionEventDto.key;
                _messageId = inReactionEventDto.msgId;
                _updatedAt = inReactionEventDto.updatedAt;
                _userId = inReactionEventDto.userId;
                _operation = inReactionEventDto.Operation;
            }
        }
    }
}