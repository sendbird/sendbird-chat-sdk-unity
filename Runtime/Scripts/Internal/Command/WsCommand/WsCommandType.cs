// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal enum WsCommandType
    {
        [JsonName("LOGI")] Logi,
        [JsonName("MESG")] UserMessage,
        [JsonName("FILE")] FileMessage,
        [JsonName("ADMM")] AdminMessage,
        [JsonName("BRDM")] BroadcastMessage,
        [JsonName("MCNT")] MemberUpdateCount,
        [JsonName("MEDI")] UpdateUserMessage,
        [JsonName("FEDI")] UpdateFileMessage,
        [JsonName("AEDI")] UpdateAdminMessage,
        [JsonName("PEDI")] UpdatePoll,
        [JsonName("VOTE")] VotePoll,
        [JsonName("DELM")] DeleteMessage,
        [JsonName("DLVR")] Delivery,
        [JsonName("READ")] Read,
        [JsonName("MRCT")] Reaction,
        [JsonName("RXNS")] Reactions,
        [JsonName("MTHD")] Threads,
        [JsonName("USEV")] UserEvent,
        [JsonName("SYEV")] ChannelEvent,
        [JsonName("EROR")] Error,
        [JsonName("ENTR")] EnterChannel,
        [JsonName("EXIT")] ExitChannel,
        [JsonName("MACK")] MessageAck,
        [JsonName("TPST")] TypingStart,
        [JsonName("TPEN")] TypingEnd,
        [JsonName("PING")] Ping,
        [JsonName("PONG")] Pong,
        [JsonName("EXPR")] SessionExpired,

        [JsonName("")] None,
        Max = None
    }

    internal static class WsCommandTypeExtension
    {
        private static int _enumCount = 0;

        internal static bool IsAckRequired(this WsCommandType inWsCommandType)
        {
            switch (inWsCommandType)
            {
                case WsCommandType.UserMessage:
                case WsCommandType.FileMessage:
                case WsCommandType.UpdateUserMessage:
                case WsCommandType.UpdateFileMessage:
                case WsCommandType.EnterChannel:
                case WsCommandType.ExitChannel:
                case WsCommandType.Logi:
                case WsCommandType.VotePoll:
                case WsCommandType.Read:
                    return true;

                default:
                    return false;
            }
        }

        internal static WsCommandType JsonNameToType(string inJsonName)
        {
            if (_enumCount <= 0)
                _enumCount = Enum.GetValues(typeof(WsCommandType)).Length;

            for (WsCommandType enumType = 0; enumType < (WsCommandType)_enumCount; enumType++)
                if (enumType.ToJsonName().Equals(inJsonName))
                    return enumType;

            return WsCommandType.None;
        }
    }
}