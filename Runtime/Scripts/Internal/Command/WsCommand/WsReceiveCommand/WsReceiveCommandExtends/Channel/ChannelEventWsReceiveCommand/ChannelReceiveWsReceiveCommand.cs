//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class ChannelReceiveWsReceiveCommand : WsReceiveCommandAbstract
    {
        internal enum CategoryType
        {
            None = 0,
            Join = 10000,
            Leave = 10001,
            UpdateOperator = 10002,
            Invite = 10020,
            DeclineInvite = 10022,
            Enter = 10102,
            Exit = 10103,
            Unmute = 10200,
            Mute = 10201,
            Unban = 10600,
            Ban = 10601,
            Unfrozen = 10700,
            Frozen = 10701,
            TypingStart = 10900,
            TypingEnd = 10901,
            PropChanged = 11000,
            MetaDataChanged = 11100,
            MetaCounterChanged = 11200,
            PinnedMessageUpdated = 11300,
            Deleted = 12000,
            Hidden = 13000,
            Unhidden = 13001,
        }

        private int _category;
        internal ChannelEventDataAbstract EventData { get; private set; }
        internal ChannelReceiveWsReceiveCommand() : base(WsCommandType.ChannelEvent) { }

        private static ChannelEventDataAbstract DeserializeEventData(CategoryType inCategoryType, string inJsonString)
        {
            if (inJsonString == null)
                return null;

            switch (inCategoryType)
            {
                case CategoryType.Ban:                  return BanChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.DeclineInvite:        return DeclineInviteChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Deleted:              return DeletedChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Enter:                return EnterChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Exit:                 return ExitChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Frozen:               return FrozenChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Hidden:               return HiddenChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Invite:               return InviteChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Join:                 return JoinChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Leave:                return LeaveChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.MetaCounterChanged:   return MetaCounterChangedChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.MetaDataChanged:      return MetaDataChangedChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Mute:                 return MuteChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.PinnedMessageUpdated: return PinnedMessageUpdatedChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.PropChanged:          return PropChangedChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.TypingEnd:            return TypingEndChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.TypingStart:          return TypingStartChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Unban:                return UnbanChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Unfrozen:             return UnfrozenChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Unhidden:             return UnhiddenChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Unmute:               return UnmuteChannelEventData.DeserializeFromJson(inJsonString);
                case CategoryType.UpdateOperator:       return UpdateOperatorChannelEventData.DeserializeFromJson(inJsonString);
                default:
                {
                    Logger.Warning(Logger.CategoryType.Command, $"ChannelEventWsEventCommand::DeserializeEventData Invalid event category type:{inCategoryType}");
                    return null;
                }
            }
        }

        internal static ChannelReceiveWsReceiveCommand DeserializeFromJson(string inJsonString)
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

            // Second pass: extract command base fields
            ChannelReceiveWsReceiveCommand eventCommand = JsonStreamingPool.ReadIgnoreException(inJsonString, reader =>
            {
                if (reader.TokenType != JsonToken.StartObject)
                    return null;

                ChannelReceiveWsReceiveCommand command = new ChannelReceiveWsReceiveCommand();
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

            // Event data deserialization uses the full JSON string (event data types handle their own parsing)
            if (eventCommand != null)
                eventCommand.EventData = DeserializeEventData((CategoryType)category, inJsonString);

            return eventCommand;
        }
    }
}
