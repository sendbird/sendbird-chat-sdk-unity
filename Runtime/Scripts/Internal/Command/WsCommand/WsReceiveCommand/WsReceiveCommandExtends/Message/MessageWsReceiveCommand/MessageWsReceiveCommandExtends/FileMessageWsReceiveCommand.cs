//
//  Copyright (c) 2022 Sendbird, Inc.
//

namespace Sendbird.Chat
{
    internal class FileMessageWsReceiveCommand : MessageWsReceiveCommandAbstract
    {
        internal FileMessageWsReceiveCommand() : base(WsCommandType.FileMessage) { }

        internal static FileMessageWsReceiveCommand DeserializeFromJson(string inJsonString, int inOffset = 0)
        {
            StreamingMessageDtoReader.CommandFields commandFields = StreamingMessageDtoReader.Deserialize<FileMessageDto>(inJsonString, out FileMessageDto dto, inOffset);
            if (dto == null)
                return null;

            FileMessageWsReceiveCommand command = new FileMessageWsReceiveCommand();
            command.SetReqId(commandFields.reqId);
            command.SetRequestId(commandFields.requestId);
            command.SetUnreadMessageCountDto(commandFields.unreadMessageCountDto);
            command.BaseMessageDto = dto;
            return command;
        }
    }
}
