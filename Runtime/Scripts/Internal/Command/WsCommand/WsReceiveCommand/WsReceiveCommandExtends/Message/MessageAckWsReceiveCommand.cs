//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class MessageAckWsReceiveCommand : WsReceiveCommandAbstract
    {
        internal string channelUrl;
        internal long msgId;

        internal MessageAckWsReceiveCommand() : base(WsCommandType.MessageAck) { }

        internal static MessageAckWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, reader =>
            {
                if (reader.TokenType != JsonToken.StartObject)
                    return null;

                MessageAckWsReceiveCommand command = new MessageAckWsReceiveCommand();
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
                        case "msg_id": command.msgId = JsonStreamingHelper.ReadLong(reader); break;
                        default: JsonStreamingHelper.SkipValue(reader); break;
                    }
                }

                return command;
            });
        }
    }
}
