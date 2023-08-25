// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class ErrorWsReceiveCommand : WsReceiveCommandAbstract
    {
        [JsonProperty("code")] internal readonly int errorCode = (int)SbErrorCode.UnknownError;
        [JsonProperty("message")] internal readonly string errorMessage = null;

        internal ErrorWsReceiveCommand() : base(WsCommandType.Error) { }

        internal static ErrorWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<ErrorWsReceiveCommand>(inJsonString);
        }
    }
}