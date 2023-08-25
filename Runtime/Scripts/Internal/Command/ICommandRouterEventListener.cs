// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal interface ICommandRouterEventListener
    {
        void OnWsError(SbError inError);
        void OnReceiveWsEventCommand(WsReceiveCommandAbstract inWsReceiveCommandAbstract);
    }
}