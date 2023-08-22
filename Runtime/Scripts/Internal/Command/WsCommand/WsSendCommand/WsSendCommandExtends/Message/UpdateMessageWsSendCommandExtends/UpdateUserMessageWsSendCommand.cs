// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UpdateUserMessageWsSendCommand : UpdateMessageWsSendCommandAbstract
    {
        [JsonProperty("message")] private readonly string _message;
        [JsonProperty("mentioned_message_template")] private readonly string _mentionedMessageTemplate;

        internal UpdateUserMessageWsSendCommand(string inChannelUrl, long inMessageId, SbUserMessageUpdateParams inUserMessageUpdateParams, AckHandler inAckHandler)
            : base(WsCommandType.UpdateUserMessage, inChannelUrl, inMessageId, inUserMessageUpdateParams, inAckHandler)
        {
            _message = inUserMessageUpdateParams.Message;
            _mentionedMessageTemplate = inUserMessageUpdateParams.MentionedMessageTemplate;
        }

        internal UpdateUserMessageWsSendCommand(string inChannelUrl, long inMessageId, MessageMetaArrayUpdateDto inMetaArrayUpdateDto, AckHandler inAckHandler)
            : base(WsCommandType.UpdateUserMessage, inChannelUrl, inMessageId, inMetaArrayUpdateDto, inAckHandler) { }
    }
}