//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class SessionExpiredWsReceiveCommand : WsReceiveCommandAbstract
    {
        private long? _expiredIn;
        private int? _reason;

        internal long ExpiredIn { get; private set; }
        internal SbErrorCode Reason { get; private set; }

        internal SessionExpiredWsReceiveCommand() : base(WsCommandType.SessionExpired) { }

        internal static SessionExpiredWsReceiveCommand DeserializeFromJson(string inJsonString, int inOffset = 0)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, inOffset, reader =>
            {
                if (reader.TokenType != JsonToken.StartObject)
                    return null;

                SessionExpiredWsReceiveCommand command = new SessionExpiredWsReceiveCommand();
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
                        case "expires_in": command._expiredIn = JsonStreamingHelper.ReadNullableLong(reader); break;
                        case "reason": command._reason = JsonStreamingHelper.ReadNullableInt(reader); break;
                        default: JsonStreamingHelper.SkipValue(reader); break;
                    }
                }

                command.ExpiredIn = command._expiredIn ?? 0;
                command.Reason = command._reason != null ? (SbErrorCode)command._reason : SbErrorCode.SessionKeyExpired;
                return command;
            });
        }
    }
}
