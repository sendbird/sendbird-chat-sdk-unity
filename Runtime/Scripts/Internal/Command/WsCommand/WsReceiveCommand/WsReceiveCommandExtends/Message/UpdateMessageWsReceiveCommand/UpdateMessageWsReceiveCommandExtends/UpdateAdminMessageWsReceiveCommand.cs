//
//  Copyright (c) 2022 Sendbird, Inc.
//

namespace Sendbird.Chat
{
    internal class UpdateAdminMessageWsReceiveCommand : UpdateMessageWsReceiveCommandAbstract
    {
        internal UpdateAdminMessageWsReceiveCommand() : base(WsCommandType.UpdateAdminMessage) { }

        internal static UpdateAdminMessageWsReceiveCommand DeserializeFromJson(string inJsonString, int inOffset = 0)
        {
            StreamingMessageDtoReader.CommandFields commandFields = StreamingMessageDtoReader.Deserialize<AdminMessageDto>(inJsonString, out AdminMessageDto dto, inOffset);
            if (dto == null)
                return null;

            UpdateAdminMessageWsReceiveCommand command = new UpdateAdminMessageWsReceiveCommand();
            command.SetReqId(commandFields.reqId);
            command.SetRequestId(commandFields.requestId);
            command.SetUnreadMessageCountDto(commandFields.unreadMessageCountDto);
            command.BaseMessageDto = dto;
            command.SetOldValues(commandFields.oldValuesMentionType, commandFields.oldValuesMentionedUserIds);
            return command;
        }
    }
}
