//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class LogiWsReceiveCommand : WsReceiveCommandAbstract
    {
        internal string sessionKey;
        internal string eKey;
        internal string newKey;
        internal long loginTimeStamp;
        internal int maxUnreadCountOnSuperGroup;
        internal ReconnectionDto reconnect;
        internal double pingInterval;
        internal double pongTimeout;
        internal bool hasError;
        internal string errorMessage;
        internal int errorCode;

        internal UserDto UserDto { get; private set; }
        internal AppInfoDto AppInfoDto { get; private set; }

        internal LogiWsReceiveCommand() : base(WsCommandType.Logi) { }

        internal static LogiWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, reader =>
            {
                if (reader.TokenType != JsonToken.StartObject)
                    return null;

                LogiWsReceiveCommand command = new LogiWsReceiveCommand();

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
                        case "key": command.sessionKey = JsonStreamingHelper.ReadString(reader); break;
                        case "ekey": command.eKey = JsonStreamingHelper.ReadString(reader); break;
                        case "new_key": command.newKey = JsonStreamingHelper.ReadString(reader); break;
                        case "login_ts": command.loginTimeStamp = JsonStreamingHelper.ReadLong(reader); break;
                        case "max_unread_cnt_on_super_group": command.maxUnreadCountOnSuperGroup = JsonStreamingHelper.ReadInt(reader); break;
                        case "reconnect": command.reconnect = ReadReconnectionDto(reader); break;
                        case "ping_interval": command.pingInterval = JsonStreamingHelper.ReadDouble(reader); break;
                        case "pong_timeout": command.pongTimeout = JsonStreamingHelper.ReadDouble(reader); break;
                        case "error": command.hasError = JsonStreamingHelper.ReadBool(reader); break;
                        case "message": command.errorMessage = JsonStreamingHelper.ReadString(reader); break;
                        case "code": command.errorCode = JsonStreamingHelper.ReadInt(reader); break;
                        default: JsonStreamingHelper.SkipValue(reader); break;
                    }
                }

                // UserDto and AppInfoDto are also deserialized from the same JSON
                command.UserDto = UserDto.DeserializeUserDtoFromJson(inJsonString);
                command.AppInfoDto = AppInfoDto.DeserializeFromJson(inJsonString);

                return command;
            });
        }

        private static ReconnectionDto ReadReconnectionDto(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            ReconnectionDto dto = new ReconnectionDto();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string propName = inReader.Value as string;
                inReader.Read();
                switch (propName)
                {
                    case "interval": dto.interval = JsonStreamingHelper.ReadFloat(inReader); break;
                    case "max_interval": dto.maxInterval = JsonStreamingHelper.ReadFloat(inReader); break;
                    case "mul": dto.mul = JsonStreamingHelper.ReadInt(inReader); break;
                    case "retry_cnt": dto.retryCount = JsonStreamingHelper.ReadInt(inReader); break;
                    default: JsonStreamingHelper.SkipValue(inReader); break;
                }
            }

            return dto;
        }
    }
}
