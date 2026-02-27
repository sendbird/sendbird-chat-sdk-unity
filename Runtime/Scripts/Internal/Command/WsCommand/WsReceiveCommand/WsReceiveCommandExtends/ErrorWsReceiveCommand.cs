//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class ErrorWsReceiveCommand : WsReceiveCommandAbstract
    {
        internal int errorCode = (int)SbErrorCode.UnknownError;
        internal string errorMessage;

        internal ErrorWsReceiveCommand() : base(WsCommandType.Error) { }

        internal static ErrorWsReceiveCommand DeserializeFromJson(string inJsonString, int inOffset = 0)
        {
            return JsonStreamingPool.ReadIgnoreException(inJsonString, inOffset, reader =>
            {
                if (reader.TokenType != JsonToken.StartObject)
                    return null;

                ErrorWsReceiveCommand command = new ErrorWsReceiveCommand();
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
                        case "code": command.errorCode = JsonStreamingHelper.ReadInt(reader); break;
                        case "message": command.errorMessage = JsonStreamingHelper.ReadString(reader); break;
                        default: JsonStreamingHelper.SkipValue(reader); break;
                    }
                }

                return command;
            });
        }
    }
}
