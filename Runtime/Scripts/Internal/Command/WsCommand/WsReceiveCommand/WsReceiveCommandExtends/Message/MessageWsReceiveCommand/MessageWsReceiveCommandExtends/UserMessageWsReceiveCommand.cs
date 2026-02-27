//
//  Copyright (c) 2022 Sendbird, Inc.
//

namespace Sendbird.Chat
{
    internal class UserMessageWsReceiveCommand : MessageWsReceiveCommandAbstract
    {
        internal UserMessageWsReceiveCommand() : base(WsCommandType.UserMessage) { }

        internal static UserMessageWsReceiveCommand DeserializeFromJson(string inJsonString, int inOffset = 0)
        {
            StreamingMessageDtoReader.CommandFields commandFields = StreamingMessageDtoReader.Deserialize<UserMessageDto>(inJsonString, out UserMessageDto dto, inOffset);
            if (dto == null)
                return null;

            UserMessageWsReceiveCommand command = new UserMessageWsReceiveCommand();
            command.SetReqId(commandFields.reqId);
            command.SetRequestId(commandFields.requestId);
            command.SetUnreadMessageCountDto(commandFields.unreadMessageCountDto);
            command.BaseMessageDto = dto;
            return command;
        }
    }
}
