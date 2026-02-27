//
//  Copyright (c) 2022 Sendbird, Inc.
//

namespace Sendbird.Chat
{
    internal abstract class WsMessageReceiveCommandAbstract : WsReceiveCommandAbstract
    {
        /// For more information, see [req_id vs request_id](https://sendbird.atlassian.net/wiki/spaces/SDK/pages/1723140230/req+id+vs+request+id)
        private string _requestId;

        internal string MessageCreatedRequestId => _requestId;

        protected WsMessageReceiveCommandAbstract(WsCommandType inWsCommandType) : base(inWsCommandType) { }

        internal void SetRequestId(string inValue)
        {
            _requestId = inValue;
        }
    }
}
