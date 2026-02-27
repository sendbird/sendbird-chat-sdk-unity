//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class UpdateUserMessageWsSendCommand : UpdateMessageWsSendCommandAbstract
    {
        private readonly string _message;
        private readonly string _mentionedMessageTemplate;

        internal UpdateUserMessageWsSendCommand(string inChannelUrl, long inMessageId, SbUserMessageUpdateParams inUserMessageUpdateParams, AckHandler inAckHandler)
            : base(WsCommandType.UpdateUserMessage, inChannelUrl, inMessageId, inUserMessageUpdateParams, inAckHandler)
        {
            _message = inUserMessageUpdateParams.Message;
            _mentionedMessageTemplate = inUserMessageUpdateParams.MentionedMessageTemplate;
        }

        internal UpdateUserMessageWsSendCommand(string inChannelUrl, long inMessageId, MessageMetaArrayUpdateDto inMetaArrayUpdateDto, AckHandler inAckHandler)
            : base(WsCommandType.UpdateUserMessage, inChannelUrl, inMessageId, inMetaArrayUpdateDto, inAckHandler) { }

        protected override void WriteFields(JsonTextWriter inWriter)
        {
            base.WriteFields(inWriter);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "message", _message);
            JsonStreamingHelper.WritePropertyIfNotNull(inWriter, "mentioned_message_template", _mentionedMessageTemplate);
        }
    }
}
