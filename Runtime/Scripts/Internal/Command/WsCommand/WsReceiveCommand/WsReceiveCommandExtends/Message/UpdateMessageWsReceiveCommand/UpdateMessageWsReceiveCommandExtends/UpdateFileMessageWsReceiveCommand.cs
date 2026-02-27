//
//  Copyright (c) 2022 Sendbird, Inc.
//

namespace Sendbird.Chat
{
    internal class UpdateFileMessageWsReceiveCommand : UpdateMessageWsReceiveCommandAbstract
    {
        internal UpdateFileMessageWsReceiveCommand() : base(WsCommandType.UpdateFileMessage) { }

        internal static UpdateFileMessageWsReceiveCommand DeserializeFromJson(string inJsonString, int inOffset = 0)
        {
            StreamingMessageDtoReader.CommandFields commandFields = StreamingMessageDtoReader.Deserialize<FileMessageDto>(inJsonString, out FileMessageDto dto, inOffset);
            if (dto == null)
                return null;

            UpdateFileMessageWsReceiveCommand command = new UpdateFileMessageWsReceiveCommand();
            command.SetReqId(commandFields.reqId);
            command.SetRequestId(commandFields.requestId);
            command.SetUnreadMessageCountDto(commandFields.unreadMessageCountDto);
            command.BaseMessageDto = dto;
            command.SetOldValues(commandFields.oldValuesMentionType, commandFields.oldValuesMentionedUserIds);
            return command;
        }
    }
}
