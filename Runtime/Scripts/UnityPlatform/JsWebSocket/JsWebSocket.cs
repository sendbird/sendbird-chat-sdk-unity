using System;
using System.Net;

namespace Sendbird.Chat
{
    internal class JsWebSocket : IWebSocket
    {
        private readonly int _clientId;
        private JsWebSocketStateType _webSocketState = JsWebSocketStateType.None;
        private bool _isInSending = false;
        private WebSocketClient.ConnectResultDelegate _connectResultHandler = null;
        private WebSocketClient.SendResultDelegate _sendResultHandler = null;
        private WebSocketClient.ReceiveMessageDelegate _receiveMessageHandler = null;
        private WebSocketClient.ErrorDelegate _errorHandler = null;
        private WebSocketClient.CloseResultDelegate _closeResultHandler = null;

        internal JsWebSocket(int inClientId)
        {
            _clientId = inClientId;
        }

        void IWebSocket.Connect(WsClientConnectParams inClientConnectParams, WebSocketClient.ConnectResultDelegate inResultHandler,
                                WebSocketClient.ReceiveMessageDelegate inReceiveMessageHandler, WebSocketClient.ErrorDelegate inErrorHandler)
        {
            Logger.Info(Logger.CategoryType.WebSocket, "JsWebSocket::Connect start");

            _webSocketState = JsWebSocketStateType.Connecting;
            _connectResultHandler = inResultHandler;
            _receiveMessageHandler = inReceiveMessageHandler;
            _errorHandler = inErrorHandler;
            _isInSending = false;
            string secProtocol = string.Empty;

            if (inClientConnectParams.CustomHeaders.TryGetValue(ConnectionHeaders.SENDBIRD_WS_TOKEN, out string authToken))
            {
                secProtocol = "{" + $" \"token\": \"{authToken}\"" + "}";
            }
            else if (inClientConnectParams.CustomHeaders.TryGetValue(ConnectionHeaders.SENDBIRD_WS_AUTH, out string sessionKey))
            {
                secProtocol = "{" + $" \"token\": \"{sessionKey}\"" + "}";
            }
            else
            {
                secProtocol = "{}";
            }

            JsNativeWebSocketBridge.Connect(_clientId, inClientConnectParams.Uri, WebUtility.UrlEncode(secProtocol));
        }

        void IWebSocket.Send(string inMessage, WebSocketClient.SendResultDelegate inResultHandler)
        {
            _isInSending = true;
            _sendResultHandler = inResultHandler;
            JsNativeWebSocketBridge.Send(_clientId, inMessage);
            //If an error occurs, _sendResultHandler is set to null.
            if (_sendResultHandler != null)
            {
                _sendResultHandler.Invoke(new WsClientSendResult(WsClientSendResult.ResultType.Succeeded));
                _sendResultHandler = null;
            }

            _isInSending = false;
        }

        void IWebSocket.Close(WebSocketClient.CloseResultDelegate inResultHandler)
        {
            Logger.Info(Logger.CategoryType.WebSocket, "JsWebSocket::Close start");
            _webSocketState = JsWebSocketStateType.Closing;
            _closeResultHandler = inResultHandler;
            JsNativeWebSocketBridge.Close(_clientId);
        }

        void IDisposable.Dispose()
        {
            JsNativeWebSocketBridge.DeleteWebSocketClientIfExist(_clientId);
        }

        internal void OnWebSocketConnected()
        {
            _webSocketState = JsWebSocketStateType.Open;
            if (_connectResultHandler != null)
            {
                _connectResultHandler.Invoke(new WsClientConnectResult(WsClientConnectResult.ResultType.Succeeded));
                _connectResultHandler = null;
            }
        }

        internal void OnWebSocketMessage(string inMessage)
        {
            if (_receiveMessageHandler != null)
            {
                _receiveMessageHandler.Invoke(inMessage);
            }
        }

        internal void OnWebSocketError()
        {
            switch (_webSocketState)
            {
                case JsWebSocketStateType.Connecting:
                {
                    if (_connectResultHandler != null)
                    {
                        _connectResultHandler.Invoke(new WsClientConnectResult(WsClientConnectResult.ResultType.CaughtException));
                        _connectResultHandler = null;
                    }

                    _webSocketState = JsWebSocketStateType.None;
                    break;
                }
                case JsWebSocketStateType.Open:
                {
                    if (_isInSending)
                    {
                        if (_sendResultHandler != null)
                        {
                            _sendResultHandler.Invoke(new WsClientSendResult(WsClientSendResult.ResultType.CaughtException));
                            _sendResultHandler = null;
                        }
                    }
                    else if (_errorHandler != null)
                    {
                        _errorHandler.Invoke(new WsClientError(WsClientErrorType.CaughtException));
                    }

                    break;
                }
                case JsWebSocketStateType.Closing:
                {
                    _webSocketState = JsWebSocketStateType.Closed;
                    if (_closeResultHandler != null)
                    {
                        _closeResultHandler.Invoke(new WsClientCloseResult(WsClientCloseResult.ResultType.CaughtException));
                        _closeResultHandler = null;
                    }

                    break;
                }
                default:
                {
                    Logger.Warning(Logger.CategoryType.WebSocket, $"JsWebSocket::OnWebSocketError state:{_webSocketState.ToString()}");
                    break;
                }
            }
        }

        internal void OnWebSocketClosed()
        {
            _webSocketState = JsWebSocketStateType.Closed;
            if (_closeResultHandler != null)
            {
                _closeResultHandler.Invoke(new WsClientCloseResult(WsClientCloseResult.ResultType.Succeeded));
                _closeResultHandler = null;
            }
        }
    }
}