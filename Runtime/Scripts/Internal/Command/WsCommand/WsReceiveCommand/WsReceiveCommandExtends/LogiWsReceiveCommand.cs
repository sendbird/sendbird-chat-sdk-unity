// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class LogiWsReceiveCommand : WsReceiveCommandAbstract
    {
        [JsonProperty("key")] internal readonly string sessionKey = null;
        [JsonProperty("ekey")] internal readonly string eKey = null;
        [JsonProperty("new_key")] internal readonly string newKey = null;
        [JsonProperty("login_ts")] internal readonly long loginTimeStamp;
        [JsonProperty("max_unread_cnt_on_super_group")] internal readonly int maxUnreadCountOnSuperGroup;
        [JsonProperty("reconnect")] internal readonly ReconnectionDto reconnect;
        [JsonProperty("ping_interval")] internal readonly double pingInterval;
        [JsonProperty("pong_timeout")] internal readonly double pongTimeout;
        [JsonProperty("error")] internal readonly bool hasError = false;
        [JsonProperty("message")] internal readonly string errorMessage = null;
        [JsonProperty("code")] internal readonly int errorCode;

        internal UserDto UserDto { get; private set; }
        internal AppInfoDto AppInfoDto { get; private set; }

        internal LogiWsReceiveCommand() : base(WsCommandType.Logi) { }

        internal static LogiWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            LogiWsReceiveCommand wsReceiveCommand = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<LogiWsReceiveCommand>(inJsonString);
            if (wsReceiveCommand != null)
            {
                wsReceiveCommand.UserDto = UserDto.DeserializeFromJson(inJsonString);
                wsReceiveCommand.AppInfoDto = AppInfoDto.DeserializeFromJson(inJsonString);
            }

            return wsReceiveCommand;
        }
    }
}