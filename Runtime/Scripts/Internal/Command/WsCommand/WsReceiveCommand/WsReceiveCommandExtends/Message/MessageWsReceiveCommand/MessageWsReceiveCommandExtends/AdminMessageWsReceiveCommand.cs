//
//  Copyright (c) 2022 Sendbird, Inc.
//

namespace Sendbird.Chat
{
    internal class AdminMessageWsReceiveCommand : MessageWsReceiveCommandAbstract
    {
        internal AdminMessageWsReceiveCommand() : base(WsCommandType.AdminMessage) { }

        internal static AdminMessageWsReceiveCommand DeserializeFromJson(string inJsonString, int inOffset = 0)
        {
            StreamingMessageDtoReader.CommandFields commandFields = StreamingMessageDtoReader.Deserialize<AdminMessageDto>(inJsonString, out AdminMessageDto dto, inOffset);
            if (dto == null)
                return null;

            AdminMessageWsReceiveCommand command = new AdminMessageWsReceiveCommand();
            command.SetReqId(commandFields.reqId);
            command.SetRequestId(commandFields.requestId);
            command.SetUnreadMessageCountDto(commandFields.unreadMessageCountDto);
            command.BaseMessageDto = dto;
            return command;
        }
    }
}
