// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sendbird.Chat
{
    internal class WsClient
    {
        internal delegate void WsConnectResultHandler(WsClientConnectResultType inConnectResultType, WsClientError inWsClientErrorNullable);
        internal delegate void WsSendResultHandler(WsClientSendResultType inSendResultType, WsClientError inWsClientErrorNullable);

        private ClientWebSocket _webSocket;
        private IWsClientEventListener _eventListeners = null;
        private readonly BlockingCollection<string> _receiveQueue = new BlockingCollection<string>();
        private CancellationTokenSource _connectCancellationTokenSource = null;
        private CancellationTokenSource _receiveCancellationTokenSource = null;
        private readonly UnityEngine.Object _lockObject = new UnityEngine.Object();
        internal WsClientStateType StateType { get; private set; } = WsClientStateType.None;
        private readonly BlockingCollection<Action> _asyncSequenceQueue = new BlockingCollection<Action>();
        private bool _isInAsyncProcessing = false;
        private bool _isTerminated = false;

        internal void Initialize(IWsClientEventListener inWsClientEventListener)
        {
            _isTerminated = false;
            _eventListeners = inWsClientEventListener;
            StateType = WsClientStateType.None;
        }

        internal void Terminate()
        {
            _isTerminated = true;
            FlushAllReceivedMessage();

            while (0 < _asyncSequenceQueue.Count)
            {
                _asyncSequenceQueue.Take().Invoke();
            }

            _eventListeners = null;
            StateType = WsClientStateType.None;
        }

        internal void Connect(WsClientConnectParams inWsClientConnectParams, WsConnectResultHandler inResultHandler = null)
        {
            Logger.Info(Logger.CategoryType.WebSocket, $"WsClient::Connect");

            if (_isInAsyncProcessing || 0 < _asyncSequenceQueue.Count)
            {
                _asyncSequenceQueue.Add(() => { ConnectAsync(inWsClientConnectParams, inResultHandler); });
            }
            else
            {
                ConnectAsync(inWsClientConnectParams, inResultHandler);
            }
        }

        internal void Send(string inTextMessage, WsSendResultHandler inResultHandler = null)
        {
            SendAsync(inTextMessage, inResultHandler);
        }

        internal void Close(Action<WsClientCloseResultType> inResultHandler = null)
        {
            Logger.Info(Logger.CategoryType.WebSocket, $"WsClient::Close");
            DisposeConnectCancellationTokenWithBlocking(inWithCancel: true);
            if (_isInAsyncProcessing || 0 < _asyncSequenceQueue.Count)
            {
                _asyncSequenceQueue.Add(() => { CloseAsync(inResultHandler); });
            }
            else
            {
                CloseAsync(inResultHandler);
            }
        }

        internal void Update()
        {
            if (_isInAsyncProcessing == false && 0 < _asyncSequenceQueue.Count)
            {
                _asyncSequenceQueue.Take().Invoke();
            }

            FlushAllReceivedMessage();
        }

        private void FlushAllReceivedMessage()
        {
            while (0 < _receiveQueue.Count)
            {
                string stringMessage = _receiveQueue.Take();
                Logger.Debug(Logger.CategoryType.WebSocket, $"Flush received queue\n {stringMessage}");
                _eventListeners?.OnReceive(stringMessage);
            }
        }

        private void SetIsInAsyncProcessingWithBlocking(bool inIsInAsyncProcessing)
        {
            lock (_lockObject)
            {
                _isInAsyncProcessing = inIsInAsyncProcessing;
            }
        }

        private async void ConnectAsync(WsClientConnectParams inWsClientConnectParams, WsConnectResultHandler inResultHandler = null)
        {
            if (_isTerminated)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => { inResultHandler?.Invoke(WsClientConnectResultType.Terminated, null); });
                return;
            }

            if (inWsClientConnectParams == null || string.IsNullOrEmpty(inWsClientConnectParams.Uri))
            {
                CoroutineManager.Instance.CallOnNextFrame(() => { inResultHandler?.Invoke(WsClientConnectResultType.InvalidParams, null); });
                return;
            }

            Logger.Info(Logger.CategoryType.WebSocket, $"WsClient::ConnectAsync start uri:{inWsClientConnectParams.Uri}");
            SetIsInAsyncProcessingWithBlocking(true);
            try
            {
                lock (_lockObject)
                {
                    _webSocket = new ClientWebSocket();
                }

                foreach (KeyValuePair<string, string> keyValuePair in inWsClientConnectParams.CustomHeaders)
                {
                    _webSocket.Options.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
                }

                ChangeWebsocketStateWithBlocking(WsClientStateType.Connecting);

                try
                {
                    Uri uri = new Uri(inWsClientConnectParams.Uri);
                    Logger.Info(Logger.CategoryType.WebSocket, $"WsClient::ConnectAsync connect to URI:{uri}");
                    lock (_lockObject)
                    {
                        _connectCancellationTokenSource = new CancellationTokenSource();
                    }

                    await _webSocket.ConnectAsync(uri, _connectCancellationTokenSource.Token);
                }
                catch (WebSocketException webSocketException)
                {
                    if (webSocketException.WebSocketErrorCode != WebSocketError.Success && webSocketException.WebSocketErrorCode != WebSocketError.InvalidState)
                    {
                        Logger.Warning(Logger.CategoryType.WebSocket, $"WsClient::ConnectAsync WebSocketException ErrorCode:{webSocketException.WebSocketErrorCode} " +
                                                                      $"NativeErrorCode:{webSocketException.NativeErrorCode} Message:{webSocketException.Message}");
                    }

                    WsClientConnectResultType resultType = WsClientConnectResultType.CaughtException;
                    if (_connectCancellationTokenSource == null)
                        resultType = WsClientConnectResultType.Canceled;

                    DisposeConnectCancellationTokenWithBlocking(inWithCancel: false);
                    ChangeWebsocketStateWithBlocking(WsClientStateType.None);
                    WsClientError wsClientError = new WsClientError(webSocketException.Message, webSocketException.ErrorCode);
                    CoroutineManager.Instance.CallOnNextFrame(() => { inResultHandler?.Invoke(resultType, wsClientError); });
                    SetIsInAsyncProcessingWithBlocking(false);
                    return;
                }

                DisposeConnectCancellationTokenWithBlocking(inWithCancel: false);
                ChangeWebsocketStateWithBlocking(WsClientStateType.Open);
                WaitingForReceiveAsync();
                CoroutineManager.Instance.CallOnNextFrame(() => { inResultHandler?.Invoke(WsClientConnectResultType.Succeeded, null); });
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.WebSocket, $"WsClient::ConnectAsync Exception:{exception.Message}");
                CoroutineManager.Instance.CallOnNextFrame(() => { inResultHandler?.Invoke(WsClientConnectResultType.CaughtException, new WsClientError(exception.Message)); });
            }

            SetIsInAsyncProcessingWithBlocking(false);
            Logger.Info(Logger.CategoryType.WebSocket, "WsClient::ConnectAsync end");
        }

        private async void SendAsync(string inTextMessage, WsSendResultHandler inResultHandler = null)
        {
            Logger.Debug(Logger.CategoryType.WebSocket, $"WsClient::SendAsync start [{inTextMessage}]");

            try
            {
                byte[] encodedBytes = Encoding.UTF8.GetBytes(inTextMessage);
                ArraySegment<byte> buffer = new ArraySegment<byte>(encodedBytes, 0, encodedBytes.Length);
                await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception exception)
            {
                Logger.Info(Logger.CategoryType.WebSocket, $"WsClient::SendAsync Exception:{exception.Message}");
                WsClientError wsClientError = null;
                if (exception is WebSocketException webSocketException)
                {
                    wsClientError = new WsClientError(webSocketException.Message, webSocketException.ErrorCode);
                }
                else
                {
                    wsClientError = new WsClientError(exception.Message);
                }

                CoroutineManager.Instance.CallOnNextFrame(() => { inResultHandler?.Invoke(WsClientSendResultType.CaughtException, wsClientError); });
            }

            CoroutineManager.Instance.CallOnNextFrame(() => { inResultHandler?.Invoke(WsClientSendResultType.Succeeded, null); });
            Logger.Debug(Logger.CategoryType.WebSocket, "WsClient::SendAsync end");
        }

        private async void CloseAsync(Action<WsClientCloseResultType> inResultHandler = null)
        {
            if (_isTerminated)
            {
                inResultHandler?.Invoke(WsClientCloseResultType.Terminated);
                return;
            }

            Logger.Info(Logger.CategoryType.WebSocket, $"WsClient::CloseAsync start");

            if (StateType == WsClientStateType.Closing || StateType == WsClientStateType.Closed)
            {
                Logger.Warning(Logger.CategoryType.WebSocket, $"WsClient::CloseAsync Already closed State:{StateType}");
                CoroutineManager.Instance.CallOnNextFrame(() => { inResultHandler?.Invoke(WsClientCloseResultType.AlreadyClosingOrClosed); });
                return;
            }

            SetIsInAsyncProcessingWithBlocking(true);

            try
            {
                ChangeWebsocketStateWithBlocking(WsClientStateType.Closing);

                WsClientCloseResultType wsClientCloseResultType = WsClientCloseResultType.Succeeded;

                if (_webSocket != null && _webSocket.State == WebSocketState.Open)
                {
                    Logger.Info(Logger.CategoryType.WebSocket, $"WsClient::CloseAsync start if valid connection");

                    try
                    {
                        await _webSocket.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None);
                    }
                    catch (WebSocketException webSocketException)
                    {
                        if (webSocketException.WebSocketErrorCode != WebSocketError.Success && webSocketException.WebSocketErrorCode != WebSocketError.InvalidState)
                        {
                            Logger.Warning(Logger.CategoryType.WebSocket, $"WsClient::CloseAsync WebSocketException ErrorCode:{webSocketException.WebSocketErrorCode} " +
                                                                          $"NativeErrorCode:{webSocketException.NativeErrorCode} Message:{webSocketException.Message}");
                        }

                        wsClientCloseResultType = WsClientCloseResultType.CaughtException;
                    }

                    DisposeReceiveCancellationTokenWithBlocking(inWithCancel: true);

                    lock (_lockObject)
                    {
                        _webSocket.Dispose();
                        _webSocket = null;
                    }

                    Logger.Info(Logger.CategoryType.WebSocket, "WsClient::CloseAsync end if valid connection");
                }

                ChangeWebsocketStateWithBlocking(WsClientStateType.Closed);
                CoroutineManager.Instance.CallOnNextFrame(() => { inResultHandler?.Invoke(wsClientCloseResultType); });
            }
            catch (Exception exception)
            {
                Logger.Info(Logger.CategoryType.WebSocket, $"WsClient::CloseAsync Exception:{exception.Message}");
                CoroutineManager.Instance.CallOnNextFrame(() => { inResultHandler?.Invoke(WsClientCloseResultType.CaughtException); });
            }

            SetIsInAsyncProcessingWithBlocking(false);
            Logger.Info(Logger.CategoryType.WebSocket, "WsClient::CloseAsync end");
        }

        private void DisposeConnectCancellationTokenWithBlocking(bool inWithCancel)
        {
            if (_connectCancellationTokenSource == null)
                return;

            lock (_lockObject)
            {
                if (_connectCancellationTokenSource.IsCancellationRequested == false && inWithCancel)
                    _connectCancellationTokenSource.Cancel();

                _connectCancellationTokenSource.Dispose();
                _connectCancellationTokenSource = null;
            }
        }

        private void DisposeReceiveCancellationTokenWithBlocking(bool inWithCancel)
        {
            if (_receiveCancellationTokenSource == null)
                return;

            lock (_lockObject)
            {
                if (_receiveCancellationTokenSource.IsCancellationRequested == false && inWithCancel)
                    _receiveCancellationTokenSource.Cancel();

                _receiveCancellationTokenSource.Dispose();
                _receiveCancellationTokenSource = null;
            }
        }

        private void ChangeWebsocketStateWithBlocking(WsClientStateType inWsStateType)
        {
            if (StateType != inWsStateType)
            {
                Logger.Info(Logger.CategoryType.WebSocket, $"WsClient::ChangeWebsocketState {StateType}->{inWsStateType}");
                lock (_lockObject)
                {
                    StateType = inWsStateType;
                }
            }
        }

        private async void WaitingForReceiveAsync()
        {
            Logger.Info(Logger.CategoryType.WebSocket, "WsClient::WaitingForReceiveAsync start");

            ArraySegment<byte> buffer = WebSocket.CreateClientBuffer(1024, 1024);
            if (buffer.Array == null)
            {
                Logger.Error(Logger.CategoryType.WebSocket, "WsClient::WaitingForReceiveAsync buffer.Array is null");
                OnDispatchReceiveErrorOnNextFrame(WsClientErrorType.CreateReceiveBufferFailed);
                return;
            }

            lock (_lockObject)
            {
                _receiveCancellationTokenSource = new CancellationTokenSource();
            }

            WsClientErrorType wsClientErrorType = WsClientErrorType.SocketClosed;
            WsClientError wsClientError = null;
            while (_webSocket?.State == WebSocketState.Open)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    WebSocketReceiveResult webSocketReceiveResult = null;
                    bool hasException = false;
                    do
                    {
                        try
                        {
                            webSocketReceiveResult = await _webSocket.ReceiveAsync(buffer, _receiveCancellationTokenSource.Token);
                            memoryStream.Write(buffer.Array, buffer.Offset, webSocketReceiveResult.Count);
                        }
                        catch (Exception exception)
                        {
                            hasException = true;
                            wsClientErrorType = WsClientErrorType.CaughtException;
                            if (exception is WebSocketException webSocketException)
                            {
                                if (webSocketException.WebSocketErrorCode != WebSocketError.Success && webSocketException.WebSocketErrorCode != WebSocketError.InvalidState)
                                {
                                    Logger.Warning(Logger.CategoryType.WebSocket, $"WsClient::WaitingForReceiveAsync WebSocketException ErrorCode:{webSocketException.WebSocketErrorCode} " +
                                                                                  $"NativeErrorCode:{webSocketException.NativeErrorCode} Message:{webSocketException.Message}");
                                }

                                wsClientError = new WsClientError(webSocketException.Message, webSocketException.ErrorCode);
                            }
                            else
                            {
                                Logger.Warning(Logger.CategoryType.WebSocket, $"WsClient::WaitForReceiveAsync Exception:{exception.Message}");
                                wsClientError = new WsClientError(exception.Message);
                            }

                            break;
                        }
                    } while (!webSocketReceiveResult.EndOfMessage && _webSocket?.State == WebSocketState.Open);

                    if (webSocketReceiveResult == null || !webSocketReceiveResult.EndOfMessage || hasException)
                    {
                        wsClientErrorType = WsClientErrorType.CaughtException;
                        break;
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    if (webSocketReceiveResult.MessageType == WebSocketMessageType.Text)
                    {
                        using (StreamReader streamReader = new StreamReader(memoryStream, Encoding.UTF8))
                        {
                            Task<string> readToEndTask = streamReader.ReadToEndAsync();
                            await readToEndTask;

                            Logger.Info(Logger.CategoryType.WebSocket, $"WsClient::Receive {readToEndTask.Result}");
                            _receiveQueue.Add(readToEndTask.Result);
                        }
                    }
                    else if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        Logger.Info(Logger.CategoryType.WebSocket, $"WsClient::Receive result is close CloseStatus:{webSocketReceiveResult.CloseStatus?.ToString()} CloseStatusDescription:{webSocketReceiveResult.CloseStatusDescription}");
                        wsClientErrorType = WsClientErrorType.ReceivedClose;
                        break;
                    }
                    else
                    {
                        Logger.Warning(Logger.CategoryType.WebSocket, $"WsClient::Receive result is binary");
                    }
                }
            }

            bool isForcedClose = _receiveCancellationTokenSource == null;
            DisposeReceiveCancellationTokenWithBlocking(inWithCancel: false);
            Logger.Info(Logger.CategoryType.WebSocket, "WsClient::WaitForReceiveAsync end");
            
            if( isForcedClose == false)
                OnDispatchReceiveErrorOnNextFrame(wsClientErrorType, wsClientError);

            return;
            void OnDispatchReceiveErrorOnNextFrame(WsClientErrorType inClientReceiveErrorType, WsClientError inWsClientErrorNullable = null)
            {
                Logger.Warning(Logger.CategoryType.WebSocket, $"WsClient::DispatchReceiveErrorOnNextFrame ErrorType:{inClientReceiveErrorType}");
                if (_eventListeners != null)
                {
                    CoroutineManager.Instance.CallOnNextFrame(() => { _eventListeners.OnErrorInOpenState(inClientReceiveErrorType, inWsClientErrorNullable); });
                }
            }
        }
    }
}