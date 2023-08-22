// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal abstract class WsSendCommandAbstract
    {
        internal delegate void AckHandler(WsReceiveCommandAbstract inWsReceiveCommandAbstract, SbError inError);

        [JsonProperty("req_id")] internal string ReqId { get; } = null;
        internal WsCommandType CommandType { get; }
        internal SbErrorHandler SendCompletionHandler { get; } = null;
        internal AckHandler AckCompletionHandler { get; }

        internal WsSendCommandAbstract(WsCommandType inWsCommandType, AckHandler inAckHandler = null, string inReqId = null)
        {
            CommandType = inWsCommandType;
            ReqId = inReqId;
            AckCompletionHandler = inAckHandler;
            if (AckCompletionHandler != null && string.IsNullOrEmpty(ReqId))
            {
                ReqId = NetworkUtil.GenerateReqId();
            }
        }

        internal string ToJsonString()
        {
            string jsonString = SerializeToJsonString();
            return $"{CommandType.ToJsonName()}{jsonString}\n";
        }

        protected virtual string SerializeToJsonString()
        {
            return NewtonsoftJsonExtension.SerializeObjectIgnoreException(this);
        }
    }
}