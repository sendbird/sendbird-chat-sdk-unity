//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class UserEventWsReceiveCommand : WsReceiveCommandAbstract
    {
        internal enum CategoryType
        {
            Unblock = 20000,
            Block = 20001,
            RoleChange = 20100,
            FriendDiscoveryReady = 20900
        }

        private int _category;
        internal UserEventDataAbstract EventData { get; private set; }
        internal UserEventWsReceiveCommand() : base(WsCommandType.UserEvent) { }

        private static UserEventDataAbstract DeserializeEventData(CategoryType inCategoryType, string inJsonString)
        {
            if (inJsonString == null)
                return null;

            switch (inCategoryType)
            {
                case CategoryType.Unblock:              return UnblockUserEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Block:                return BlockUserEventData.DeserializeFromJson(inJsonString);
                case CategoryType.RoleChange:           return RoleChangeUserEventData.DeserializeFromJson(inJsonString);
                case CategoryType.FriendDiscoveryReady: return FriendDiscoveryReadyUserEventData.DeserializeFromJson(inJsonString);
                default:
                {
                    Logger.Warning(Logger.CategoryType.Command, $"UserEventWsEventCommand::DeserializeEventData Invalid event category type:{inCategoryType}");
                    return null;
                }
            }
        }

        internal static UserEventWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            // First pass: extract category quickly
            int category = JsonStreamingPool.ReadIgnoreException(inJsonString, reader =>
            {
                if (reader.TokenType != JsonToken.StartObject)
                    return 0;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.EndObject)
                        break;

                    string propName = reader.Value as string;
                    reader.Read();
                    if (propName == "cat")
                    {
                        return JsonStreamingHelper.ReadInt(reader);
                    }
                    else
                    {
                        JsonStreamingHelper.SkipValue(reader);
                    }
                }

                return 0;
            });

            // Second pass: read command base fields
            UserEventWsReceiveCommand eventCommand = JsonStreamingPool.ReadIgnoreException(inJsonString, reader =>
            {
                if (reader.TokenType != JsonToken.StartObject)
                    return null;

                UserEventWsReceiveCommand command = new UserEventWsReceiveCommand();
                command._category = category;

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
                        default: JsonStreamingHelper.SkipValue(reader); break;
                    }
                }

                return command;
            });

            if (eventCommand != null)
                eventCommand.EventData = DeserializeEventData((CategoryType)category, inJsonString);

            return eventCommand;
        }
    }
}
