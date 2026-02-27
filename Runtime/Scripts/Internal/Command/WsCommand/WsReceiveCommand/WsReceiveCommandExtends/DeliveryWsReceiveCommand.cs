//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class DeliveryWsReceiveCommand : WsReceiveCommandAbstract
    {
        internal string channelUrl;
        internal Dictionary<string, long> updated;

        internal DeliveryWsReceiveCommand() : base(WsCommandType.Delivery) { }

        internal static DeliveryWsReceiveCommand DeserializeFromJson(string inJsonString, int inOffset = 0)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, inOffset, reader =>
            {
                if (reader.TokenType != JsonToken.StartObject)
                    return null;

                DeliveryWsReceiveCommand command = new DeliveryWsReceiveCommand();
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
                        case "updated": command.updated = JsonStreamingHelper.ReadStringLongDictionary(reader); break;
                        default: JsonStreamingHelper.SkipValue(reader); break;
                    }
                }

                return command;
            });
        }
    }
}
