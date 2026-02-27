//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class ReactionWsReceiveCommand : WsReceiveCommandAbstract
    {
        internal string channelUrl;
        private string _channelType;

        internal ReactionWsReceiveCommand() : base(WsCommandType.Reaction) { }

        internal SbChannelType ChannelType { get; private set; }
        internal ReactionEventDto ReactionEventDto { get; private set; }

        internal static ReactionWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, reader =>
            {
                if (reader.TokenType != JsonToken.StartObject)
                    return null;

                ReactionWsReceiveCommand command = new ReactionWsReceiveCommand();
                ReactionEventDto reactionEventDto = new ReactionEventDto();
                string reactionOperation = null;

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
                        // ReactionEventDto fields (shared in same JSON)
                        case "reaction": reactionEventDto.key = JsonStreamingHelper.ReadString(reader); break;
                        case "user_id": reactionEventDto.userId = JsonStreamingHelper.ReadString(reader); break;
                        case "updated_at": reactionEventDto.updatedAt = JsonStreamingHelper.ReadLong(reader); break;
                        case "msg_id": reactionEventDto.msgId = JsonStreamingHelper.ReadLong(reader); break;
                        case "operation": reactionOperation = JsonStreamingHelper.ReadString(reader); break;
                        default: JsonStreamingHelper.SkipValue(reader); break;
                    }
                }

                if (string.IsNullOrEmpty(command._channelType) == false)
                    command.ChannelType = SbChannelTypeExtension.JsonNameToType(command._channelType);

                // Manually set operation on ReactionEventDto
                command.ReactionEventDto = ReactionEventDto.ReadFromJsonString(inJsonString);

                return command;
            });
        }
    }
}
