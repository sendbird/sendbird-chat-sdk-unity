//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class EnterChannelWsReceiveCommand : WsReceiveCommandAbstract
    {
        internal string channelUrl;
        internal int participantCount;
        internal long enterTimestamp;
        internal long edgeTimestamp;
        internal string subChannelId;

        internal EnterChannelWsReceiveCommand() : base(WsCommandType.EnterChannel) { }

        internal static EnterChannelWsReceiveCommand DeserializeFromJson(string inJsonString, int inOffset = 0)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, inOffset, reader =>
            {
                if (reader.TokenType != JsonToken.StartObject)
                    return null;

                EnterChannelWsReceiveCommand command = new EnterChannelWsReceiveCommand();
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
                        case "channel_url": command.channelUrl = JsonStreamingHelper.ReadString(reader); break;
                        case "participant_count": command.participantCount = JsonStreamingHelper.ReadInt(reader); break;
                        case "enter_ts": command.enterTimestamp = JsonStreamingHelper.ReadLong(reader); break;
                        case "edge_ts": command.edgeTimestamp = JsonStreamingHelper.ReadLong(reader); break;
                        case "subchannel_id": command.subChannelId = JsonStreamingHelper.ReadString(reader); break;
                        default: JsonStreamingHelper.SkipValue(reader); break;
                    }
                }

                return command;
            });
        }
    }
}
