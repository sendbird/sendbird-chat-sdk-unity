// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal static class WsEventCommandFactory
    {
        private const int MESSAGE_TYPE_LENGTH = 4;
        private static readonly Dictionary<string, WsCommandType> _commandTypesByEnumString = new Dictionary<string, WsCommandType>((int)WsCommandType.Max);

        internal static WsReceiveCommandAbstract CreateCommandFromReceivedMessage(string inReceivedMessage)
        {
            if (string.IsNullOrEmpty(inReceivedMessage))
            {
                Logger.Warning(Logger.CategoryType.Command, "WsEventCommandFactory::CreateCommandFromReceivedMessage received message is null or empty");
                return null;
            }

            if (inReceivedMessage.Length < MESSAGE_TYPE_LENGTH)
            {
                Logger.Warning(Logger.CategoryType.Command, $"WsEventCommandFactory::CreateCommandFromReceivedMessage invalid received message:{inReceivedMessage}");
                return null;
            }

            if (_commandTypesByEnumString.Count <= 0)
            {
                for (WsCommandType wsCommandType = WsCommandType.Logi; wsCommandType < WsCommandType.Max; wsCommandType++)
                {
                    string jsonName = wsCommandType.ToJsonName();
                    _commandTypesByEnumString.Add(jsonName, wsCommandType);
                }
            }

            string commandTypeString = inReceivedMessage.Substring(0, MESSAGE_TYPE_LENGTH);
            if (_commandTypesByEnumString.TryGetValue(commandTypeString, out WsCommandType commandType) == false)
            {
                Logger.Warning(Logger.CategoryType.Command, $"WsEventCommandFactory::CreateCommandFromReceivedMessage invalid command type:{commandTypeString}");
                return null;
            }

            string jsonString = inReceivedMessage.Substring(MESSAGE_TYPE_LENGTH);
            switch (commandType)
            {
                case WsCommandType.AdminMessage:       return AdminMessageWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.BroadcastMessage:   return AdminMessageWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.ChannelEvent:       return ChannelReceiveWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.DeleteMessage:      return DeleteMessageWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.Delivery:           return DeliveryWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.EnterChannel:       return EnterChannelWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.Error:              return ErrorWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.ExitChannel:        return ExitChannelWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.FileMessage:        return FileMessageWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.Logi:               return LogiWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.MemberUpdateCount:  return MemberUpdateCountWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.Pong:               return PongWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.Reaction:           return ReactionWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.Reactions:          return null;
                case WsCommandType.Read:               return ReadWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.SessionExpired:     return SessionExpiredWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.Threads:            return ThreadsWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.TypingEnd:          return null;
                case WsCommandType.TypingStart:        return null;
                case WsCommandType.UpdateAdminMessage: return UpdateAdminMessageWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.UpdateFileMessage:  return UpdateFileMessageWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.UpdateUserMessage:  return UpdateUserMessageWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.UserEvent:          return UserEventWsReceiveCommand.DeserializeFromJson(jsonString);
                case WsCommandType.UserMessage:        return UserMessageWsReceiveCommand.DeserializeFromJson(jsonString);
                default:
                {
                    Logger.Warning(Logger.CategoryType.Command, $"WsEventCommandFactory::CreateEventCommandFromJson Invalid event command. type:{commandType}");
                    break;
                }
            }

            return null;
        }
    }
}