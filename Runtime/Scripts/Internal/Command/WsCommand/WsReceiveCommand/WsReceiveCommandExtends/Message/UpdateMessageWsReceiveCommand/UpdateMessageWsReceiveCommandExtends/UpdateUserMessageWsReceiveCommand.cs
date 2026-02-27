//
//  Copyright (c) 2022 Sendbird, Inc.
//

namespace Sendbird.Chat
{
    internal class UpdateUserMessageWsReceiveCommand : UpdateMessageWsReceiveCommandAbstract
    {
        internal UpdateUserMessageWsReceiveCommand() : base(WsCommandType.UpdateUserMessage) { }

        internal static UpdateUserMessageWsReceiveCommand DeserializeFromJson(string inJsonString, int inOffset = 0)
        {
            StreamingMessageDtoReader.CommandFields commandFields = StreamingMessageDtoReader.Deserialize<UserMessageDto>(inJsonString, out UserMessageDto dto, inOffset);
            if (dto == null)
                return null;

            UpdateUserMessageWsReceiveCommand command = new UpdateUserMessageWsReceiveCommand();
            command.SetReqId(commandFields.reqId);
            command.SetRequestId(commandFields.requestId);
            command.SetUnreadMessageCountDto(commandFields.unreadMessageCountDto);
            command.BaseMessageDto = dto;
            command.SetOldValues(commandFields.oldValuesMentionType, commandFields.oldValuesMentionedUserIds);
            return command;
        }
    }
}
