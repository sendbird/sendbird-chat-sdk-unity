//
//  Copyright (c) 2022 Sendbird, Inc.
//

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal abstract class WsReceiveCommandAbstract
    {
        private string _reqId;
        internal UnreadMessageCountDto unreadMessageCountDto;

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

        internal void SetReqId(string inValue)
        {
            _reqId = inValue;
        }

        internal void SetUnreadMessageCountDto(UnreadMessageCountDto inDto)
        {
            unreadMessageCountDto = inDto;
        }
    }
}
