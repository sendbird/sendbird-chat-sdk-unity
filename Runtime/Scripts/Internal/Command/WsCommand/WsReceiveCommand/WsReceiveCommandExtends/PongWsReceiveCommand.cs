//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class PongWsReceiveCommand : WsReceiveCommandAbstract
    {
        internal long ts;
        internal long sts;

        internal PongWsReceiveCommand() : base(WsCommandType.Pong) { }

        internal static PongWsReceiveCommand DeserializeFromJson(string inJsonString, int inOffset = 0)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, inOffset, reader =>
            {
                if (reader.TokenType != JsonToken.StartObject)
                    return null;

                PongWsReceiveCommand command = new PongWsReceiveCommand();
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
                        case "ts": command.ts = JsonStreamingHelper.ReadLong(reader); break;
                        case "sts": command.sts = JsonStreamingHelper.ReadLong(reader); break;
                        default: JsonStreamingHelper.SkipValue(reader); break;
                    }
                }

                return command;
            });
        }
    }
}
