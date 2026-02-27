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

            switch (commandType)
            {
                case WsCommandType.AdminMessage:       return AdminMessageWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.BroadcastMessage:   return AdminMessageWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.DeleteMessage:      return DeleteMessageWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.Delivery:           return DeliveryWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.EnterChannel:       return EnterChannelWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.Error:              return ErrorWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.ExitChannel:        return ExitChannelWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.FileMessage:        return FileMessageWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.MemberUpdateCount:  return MemberUpdateCountWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.Pong:               return PongWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.Reactions:          return null;
                case WsCommandType.Read:               return ReadWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.SessionExpired:     return SessionExpiredWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.Threads:            return ThreadsWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.TypingEnd:          return null;
                case WsCommandType.TypingStart:        return null;
                case WsCommandType.UpdateAdminMessage: return UpdateAdminMessageWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.UpdateFileMessage:  return UpdateFileMessageWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.UpdateUserMessage:  return UpdateUserMessageWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                case WsCommandType.UserMessage:        return UserMessageWsReceiveCommand.DeserializeFromJson(inReceivedMessage, MESSAGE_TYPE_LENGTH);
                // Commands with secondary string parsing — use Substring (small payloads)
                case WsCommandType.ChannelEvent:       return ChannelReceiveWsReceiveCommand.DeserializeFromJson(inReceivedMessage.Substring(MESSAGE_TYPE_LENGTH));
                case WsCommandType.Logi:               return LogiWsReceiveCommand.DeserializeFromJson(inReceivedMessage.Substring(MESSAGE_TYPE_LENGTH));
                case WsCommandType.Reaction:           return ReactionWsReceiveCommand.DeserializeFromJson(inReceivedMessage.Substring(MESSAGE_TYPE_LENGTH));
                case WsCommandType.UserEvent:          return UserEventWsReceiveCommand.DeserializeFromJson(inReceivedMessage.Substring(MESSAGE_TYPE_LENGTH));
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