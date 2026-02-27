//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class MemberUpdateCountWsReceiveCommand : WsReceiveCommandAbstract
    {
        internal List<GroupChannelMemberCountDto> groupChannelMemberCountObjects;
        internal List<OpenChannelMemberCountDto> openChannelMemberCountObjects;

        internal MemberUpdateCountWsReceiveCommand() : base(WsCommandType.MemberUpdateCount) { }

        internal static MemberUpdateCountWsReceiveCommand DeserializeFromJson(string inJsonString, int inOffset = 0)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, inOffset, reader =>
            {
                if (reader.TokenType != JsonToken.StartObject)
                    return null;

                MemberUpdateCountWsReceiveCommand command = new MemberUpdateCountWsReceiveCommand();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.EndObject)
                        break;

                    string propName = reader.Value as string;
                    reader.Read();
                    switch (propName)
                    {
                        case "req_id": command.SetReqId(JsonStreamingHelper.ReadString(reader)); break;
                        case "unread_cnt": command.SetUnreadMessageCountDto(UnreadMessageCountDto.ReadFromJson(reader)); break;
                        case "group_channels": command.groupChannelMemberCountObjects = GroupChannelMemberCountDto.ReadListFromJson(reader); break;
                        case "open_channels": command.openChannelMemberCountObjects = OpenChannelMemberCountDto.ReadListFromJson(reader); break;
                        default: JsonStreamingHelper.SkipValue(reader); break;
                    }
                }

                return command;
            });
        }
    }
}
