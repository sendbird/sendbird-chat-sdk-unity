// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal class SessionExpiredWsReceiveCommand : WsReceiveCommandAbstract
    {
        [JsonProperty("expires_in")] private readonly long? _expiredIn;
        [JsonProperty("reason")] private readonly int? _reason;

        internal long ExpiredIn { get; private set; }
        internal SbErrorCode Reason { get; private set; }

        internal SessionExpiredWsReceiveCommand() : base(WsCommandType.SessionExpired) { }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            ExpiredIn = _expiredIn ?? 0;
            Reason = _reason != null ? (SbErrorCode)_reason : SbErrorCode.SessionKeyExpired;
        }

        internal static SessionExpiredWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<SessionExpiredWsReceiveCommand>(inJsonString);;
        }
    }
}