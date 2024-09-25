using System;

namespace Sendbird.Chat
{
    internal interface IWebSocket : IDisposable
    {
        void Connect(WsClientConnectParams inClientConnectParams, WebSocketClient.ConnectResultDelegate inResultHandler,
                     WebSocketClient.ReceiveMessageDelegate inReceiveMessageHandler, WebSocketClient.ErrorDelegate inErrorHandler);

        void Send(string inMessage, WebSocketClient.SendResultDelegate inResultHandler = null);
        void Close(WebSocketClient.CloseResultDelegate inResultHandler);
    }
}