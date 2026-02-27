//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class DeleteMessageWsReceiveCommand : WsReceiveCommandAbstract
    {
        internal string channelUrl;
        private string _channelType;
        internal long msgId;
        internal long timestamp;

        internal DeleteMessageWsReceiveCommand() : base(WsCommandType.DeleteMessage) { }

        internal SbChannelType ChannelType { get; private set; }

        internal static DeleteMessageWsReceiveCommand DeserializeFromJson(string inJsonString, int inOffset = 0)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, inOffset, reader =>
            {
                if (reader.TokenType != JsonToken.StartObject)
                    return null;

                DeleteMessageWsReceiveCommand command = new DeleteMessageWsReceiveCommand();
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
                        case "channel_type": command._channelType = JsonStreamingHelper.ReadString(reader); break;
                        case "msg_id": command.msgId = JsonStreamingHelper.ReadLong(reader); break;
                        case "ts": command.timestamp = JsonStreamingHelper.ReadLong(reader); break;
                        default: JsonStreamingHelper.SkipValue(reader); break;
                    }
                }

                if (string.IsNullOrEmpty(command._channelType) == false)
                    command.ChannelType = SbChannelTypeExtension.JsonNameToType(command._channelType);

                return command;
            });
        }
    }
}
