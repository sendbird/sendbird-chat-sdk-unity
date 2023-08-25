// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal abstract class WsReceiveCommandAbstract
    {
        [JsonProperty("req_id")] private readonly string _reqId = null;
        [JsonProperty("unread_cnt")] internal readonly UnreadMessageCountDto unreadMessageCountDto;

        internal string ReqId => _reqId;

        internal WsCommandType CommandType { get; private set; }

        internal WsReceiveCommandAbstract(WsCommandType inWsCommandType)
        {
            CommandType = inWsCommandType;
        }

        internal bool IsAckFromCurrentDeviceRequest()
        {
            return string.IsNullOrEmpty(_reqId) == false;
        }

        internal static WsReceiveCommandAbstract DeserializeFromJson(WsCommandType inCommandType, string inJsonString)
        {
            WsReceiveCommandAbstract wsReceiveCommand = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<WsReceiveCommandAbstract>(inJsonString);
            wsReceiveCommand.CommandType = inCommandType;
            return wsReceiveCommand;
        }
    }
}